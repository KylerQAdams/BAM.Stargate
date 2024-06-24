import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CardComponent } from './features/card/card.component';
import { CoreModule } from './core/core.module';
import { ViewAllComponent } from './features/view-all/view-all.component';
import { AddPersonComponent } from './features/add-person/add-person.component';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CardComponent, CoreModule, ViewAllComponent, AddPersonComponent],
  providers: [DatePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  title = 'astro-app';

  addPersonEvent() {
    throw new Error('Method not implemented.');
    }
}
