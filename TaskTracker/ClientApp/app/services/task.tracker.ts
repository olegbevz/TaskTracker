import { Task, TaskStatus, SortOrder } from "../models/task";
import { Observable, Subject } from "rxjs";
import { HubConnection } from '@aspnet/signalr-client';
import { SystemService } from "./system.service";
import { Inject, Injectable } from "@angular/core";

@Injectable()
export class TaskTracker {

    private readonly subject: Subject<Task[]> = new Subject<Task[]>();
    private readonly connection: HubConnection;
    private active: boolean = false;

    constructor() {
        this.connection = new HubConnection('/task');
        this.connection.on('push', tasks => {
            this.subject.next((<any[]>tasks).map(x => Task.fromJson(x)));
        });
    }

    get tasks(): Observable<Task[]> {
        return this.subject;
    }

    public subscribe(subscription: any): void {
        if (!this.active) {
            this.connection.start().then(() => {
                this.connection.invoke('subscribe', subscription);
                this.active = true;
            });
        } else {
            this.connection.invoke('subscribe', subscription);
        }
    }

    public unsubscribe(): void {
        this.connection.invoke('unsubscribe');
    }
}