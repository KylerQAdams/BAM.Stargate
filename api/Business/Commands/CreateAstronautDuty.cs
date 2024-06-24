using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Models;
using System.Text.Json.Serialization;

namespace StargateAPI.Business.Commands
{
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        public required string Name { get; set; }

        public required string Rank { get; set; }

        public required string DutyTitle { get; set; }

        public DateTime DutyStartDate { get; set; }

        [JsonIgnore]
        public Person? Person { get; set; }

    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

            if (person is null) throw new BadHttpRequestException("Bad Request - Person not found");

            var verifyNoFutureDuty = _context.AstronautDuties.FirstOrDefault(z => z.DutyStartDate >= request.DutyStartDate.AddDays(-1) && z.PersonId == person.Id);
            if (verifyNoFutureDuty is not null) throw new BadHttpRequestException("Bad Request - Duty start date conflicts with other duties");

            request.Person = person;
            return Task.CompletedTask;
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        {

            // Person should be set by the PreProcessor
            var personId = request.Person?.Id
                ?? (await _context.People.AsNoTracking().FirstOrDefaultAsync(z => z.Name == request.Name))?.Id
                ?? throw new BadHttpRequestException("Bad Request - Person not found");

            var astronautDetail = await _context.AstronautDetails.FirstOrDefaultAsync(z => z.PersonId == personId);
            var astronautDuty = await _context.AstronautDuties.FirstOrDefaultAsync(z => z.PersonId == personId && z.DutyEndDate == null);


            if (astronautDetail == null)
            {
                // New Astronaut Detail
                astronautDetail = new AstronautDetail();
                astronautDetail.PersonId = personId;
                astronautDetail.CurrentDutyTitle = request.DutyTitle;
                astronautDetail.CurrentRank = request.Rank;
                astronautDetail.CareerStartDate = request.DutyStartDate.Date;
                if (string.Equals(request.DutyTitle, "RETIRED"))
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }


                await _context.AstronautDetails.AddAsync(astronautDetail);
            }
            else
            {
                // Update Existing Astronaut Detail
                astronautDetail.CurrentDutyTitle = request.DutyTitle;
                astronautDetail.CurrentRank = request.Rank;
                if (string.Equals(request.DutyTitle, "RETIRED"))
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }
                else
                {
                    astronautDetail.CareerEndDate = null;
                }
            }

            // Record prior duty as completed.
            if (astronautDuty != null)
            {
                astronautDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
            }

            // Insert new duty.
            var newAstronautDuty = new AstronautDuty()
            {
                PersonId = personId,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate.Date,
                DutyEndDate = null
            };

            await _context.AstronautDuties.AddAsync(newAstronautDuty);
            await _context.SaveChangesAsync();

            return new CreateAstronautDutyResult()
            {
                Id = newAstronautDuty.Id
            };
        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}
