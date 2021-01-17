import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { VersionService } from './services/version.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private translateService: TranslateService, public versionService: VersionService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use('en');
  }

  ngOnInit() {

  }
}
