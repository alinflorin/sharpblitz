import 'hammerjs';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

declare var DotNet: any;

if (environment.production) {
  enableProdMode();
}

function tryLoadDotnet() {
  try {
    DotNet.invokeMethodAsync('SharpBlitz', 'Healthcheck').then(x => {
      platformBrowserDynamic().bootstrapModule(AppModule);
    }, err => {
      setTimeout(() => {
        tryLoadDotnet();
      }, 100);
    });
  } catch (err) {
    setTimeout(() => {
      tryLoadDotnet();
    }, 100);
  }
}
tryLoadDotnet();
