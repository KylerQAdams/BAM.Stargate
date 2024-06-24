import { Component } from '@angular/core';
import { AstronautDuty, AstronautDutyClient, GetAstronautDutiesByNameResult, GetPeopleResult, PersonAstronaut, PersonClient } from '../../core/services/api.generated';
import { CommonModule, DatePipe } from '@angular/common';
import { PeopleChangeService } from '../../core/services/people-change-service';
import { CardComponent } from "../card/card.component";

@Component({
    selector: 'view-all',
    standalone: true,
    templateUrl: './view-all.component.html',
    styleUrl: './view-all.component.css',
    imports: [CommonModule, CardComponent]
})
export class ViewAllComponent {
  people: PersonAstronaut[] = [];
  duties: AstronautDuty[] = [];
  dutySubject: string = '';


  constructor(private personClient: PersonClient,
    peopleChangeService: PeopleChangeService,
    private dutyClient: AstronautDutyClient,
    public datePipe: DatePipe)
  {
    peopleChangeService.people.subscribe( () => this.peopleChangedReceived());
    this.peopleChangedReceived();
  }

  peopleChangedReceived()
  {
    this.people = [];
    this.personClient.getPeople().subscribe((data) => this.loadData(data))
  }

  loadData(data: GetPeopleResult) {
    this.people = data.people ?? [];

  }

  viewDuties(person:PersonAstronaut)
  {
    if (person.name)
    {
      this.dutyClient.getAstronautDutiesByName(person.name).subscribe( (data) => this.showDuty(data));
    }
  }

  showDuty(data: GetAstronautDutiesByNameResult)
  {
   this.duties = data.astronautDuties?.sort((a,b) => b.id! - a.id!) ?? [];
   this.dutySubject = data.person?.name ? `Duties: ${data.person?.name}` : '';
  }
}
