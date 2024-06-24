using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("RequestLog")]
    public class RequestLog
    {
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;

        public string RequestBody { get; set; } = string.Empty;

        public string ResponseBody { get; set; } = string.Empty;


        public DateTime? RequestReceivedDate { get; set; }

        public DateTime? ResponseSentDate { get; set; }

    }

    public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
