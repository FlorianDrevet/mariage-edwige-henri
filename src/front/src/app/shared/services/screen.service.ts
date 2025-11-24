import { Injectable } from '@angular/core';
import {map, Observable} from "rxjs";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";

@Injectable({
  providedIn: 'root'
})
export class ScreenService {

  isSmallScreen$: Observable<boolean>;

  constructor(private breakpointObserver: BreakpointObserver) {
    this.isSmallScreen$ = this.breakpointObserver.observe(`(max-width: 1024px)`).pipe(
      map(result => result.matches)
    );
  }
}
