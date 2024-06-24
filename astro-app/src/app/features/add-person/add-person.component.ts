import { Component, EventEmitter, Input } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { PersonClient } from '../../core/services/api.generated';
import { CommonModule } from '@angular/common';
import { PeopleChangeService } from '../../core/services/people-change-service';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'add-person',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './add-person.component.html',
  styleUrl: './add-person.component.css'
})
export class AddPersonComponent {
  newPersonControl = new FormControl('', [Validators.required, Validators.minLength(3)]);
  errorMsg: string = '';

  constructor(private personClient: PersonClient, private peopleChangeService: PeopleChangeService)
  { }

  submitPerson() {
    if (this.newPersonControl.valid)
    {
      this.errorMsg = '';
      this.newPersonControl.markAsPristine();
        this.personClient.createPerson(this.newPersonControl.value!)
        .pipe(catchError(() => of(null)))
        .subscribe( (result) => {
          if (result?.success)
          {
            this.newPersonControl.reset();
            this.peopleChangeService.alertAChange();
          }
          else
          {
            this.errorMsg = `Unable to add Person '${this.newPersonControl.value!}'.`;
          }
      });
    }
  }
}
