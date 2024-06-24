import { Injectable } from "@angular/core";
import { BehaviorSubject, Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class PeopleChangeService {
    people = new Subject<void>();

    alertAChange()
    {
        this.people.next();
    }
}