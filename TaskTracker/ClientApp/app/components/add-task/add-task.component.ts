import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { NgbTimeStruct } from "@ng-bootstrap/ng-bootstrap";
import { Task } from "../../models/task"
import { TaskService } from "../../services/task.service";
import { NgbTimeStructPipe } from "../../pipes/ngb-time.pipe";
import * as moment from "moment";

@Component({
    selector: "add-task",
    templateUrl: "./add-task.component.html",
    styleUrls: ["./add-task.component.css"]
})
export class AddTaskComponent {

    public readonly task: Task;
    public submitting: boolean;
    public completed: boolean;
    public faulted: boolean;

    constructor(private taskService: TaskService, private timeStructPipe: NgbTimeStructPipe) {
        this.task = this.newTask();
    }

    private newTask(): Task {
        var task = new Task();
        task.added = moment();
        task.edited = moment();
        task.priority = 5;
        task.duration = moment.duration('04:00:00');
        return task;
    }

    public onDurationChange(ngbTimeStruct: NgbTimeStruct): void {
        this.task.duration = this.timeStructPipe.transformBack(ngbTimeStruct);
    }

    public onSubmit(): void {
        this.submitting = true;

        this.taskService.addTask(this.task).subscribe(() => {
            this.completed = true;
        }, (error) => {
            this.faulted = true;
        }, () => {
            this.submitting = false;
        });
    }
}
