import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

declare var DotNet: any;

@Injectable({
  providedIn: 'root'
})
export class DotnetService {

  constructor() { }

  invoke<T>(assmName: string, methodName: string, ...params: any[]): Observable<T> {
    const subject = new Subject<T>();
    let p = 'params';
    if (params != null && params.length > 0) {
      const x: string[] = [];
      for (let i = 0; i < params.length; i++) {
        x.push(`params[${i}]`);
      }
      p = x.join(', ');
    }
    eval(`DotNet.invokeMethodAsync(assmName, methodName, ${p}).then(x => { subject.next(x); }).catch(e => { subject.error(e); })`);
    return subject.asObservable();
  }
}
