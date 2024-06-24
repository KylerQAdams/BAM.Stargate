import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import * as GeneratedClients from "./services/api.generated"
import * as AppConfigJosn from "../../environments/app-config.json"


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class CoreModule { 
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [{provide: GeneratedClients.API_BASE_URL, useValue: AppConfigJosn.API_BASE_URL}]
    }
  }
}


