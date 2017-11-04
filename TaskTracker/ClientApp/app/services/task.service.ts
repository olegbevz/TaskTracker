import { Task, TaskStatus, SortOrder } from "../models/task";
import { Observable } from "rxjs/Observable";
import { Http, Response, RequestOptions, URLSearchParams } from "@angular/http";
import { Injectable, Inject } from "@angular/core";
import "rxjs/add/operator/map";

@Injectable()
export class TaskService {

    private readonly API_URL = "api/tasks";

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
    }

    public getTasks(status?: TaskStatus, sortOrder?: SortOrder, skip?: number, take?: number)
            : Observable<Task[]>  {
        const options = new RequestOptions();
        options.params = new URLSearchParams();

        if (status) {
            options.params.append("status", status);
        }

        if (sortOrder) {
            options.params.append("sortOrder", sortOrder);
        }

        if (skip) {
            options.params.append("skip", skip.toString());
        }

        if (take) {
            options.params.append("take", take.toString());
        }

        return this.http.get(this.API_URL, options)
            .map((response: Response) => <any[]>response.json())
            .map(json => json.map(x => Task.fromJson(x)));
    }

    public getTask(taskId: number): Observable<Task> {
        const options = new RequestOptions({
            params: { "taskId": taskId }
        });

        return this.http.get(this.API_URL + "/task", options)
            .map((response: Response) => Task.fromJson(response.json()));
    }

    public addTask(task: Task): Observable<any> {
        return this.http.post(this.baseUrl + this.API_URL, Task.json(task));
    }

    public removeTask(taskId: number): Observable<any> {
        const options = new RequestOptions({
            params: { "id": taskId }
        });

        return this.http.delete(this.API_URL, options);
    }

    public updateTask(task: Task): Observable<any> {
        return this.http.put(this.API_URL, Task.json(task));
    }
}