import { Injectable } from '@angular/core';
import { Version } from '../models/version';
import { version, versionDate } from 'src/_versions';

@Injectable({
  providedIn: 'root'
})
export class VersionService {

  constructor() { }

  get version() {
    return {
      version,
      versionDate: versionDate == null ? null : new Date(Date.parse(versionDate))
    } as Version;
  }

  get singleVersion() {
    const v = this.version;
    return `v${v.version}-${v.versionDate == null ? '' : v.versionDate.toISOString()}`;
  }
}
