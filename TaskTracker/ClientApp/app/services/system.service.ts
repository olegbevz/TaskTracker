import { PLATFORM_ID, Inject, Injectable } from '@angular/core';
import { isPlatformServer, isPlatformBrowser } from '@angular/common';

@Injectable()
export class SystemService {
    constructor( @Inject(PLATFORM_ID) private platformId: Object) {
    }

    public isBrowser(): boolean {
        return isPlatformBrowser(this.platformId);
    }

    public isServer(): boolean {
        return isPlatformServer(this.platformId);
    }
}