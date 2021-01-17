import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { SharedModule } from './shared/shared.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NotFoundComponent } from './not-found/not-found.component';
import { EditorComponent } from './editor/editor.component';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { SidebarComponent } from './sidebar/sidebar.component';
import { AngularSplitModule } from 'angular-split';

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent,
    EditorComponent,
    SidebarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    FlexLayoutModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: (createTranslateLoader),
          deps: [HttpClient]
      }
  }),

    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    MonacoEditorModule.forRoot(),
    AngularSplitModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
