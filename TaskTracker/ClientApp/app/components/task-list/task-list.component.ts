import { Component, OnInit, ViewChild } from '@angular/core';
import { TaskService } from "../../services/task.service";
import { Task } from "../../models/task";
import { Location } from '@angular/common';
import { URLSearchParams } from "@angular/http";
import { ActivatedRoute } from "@angular/router";
import { TaskTableComponent } from "../task-table/task-table.component";
import { HubConnection } from '@aspnet/signalr-client';
import { SystemService } from "../../services/system.service";

@Component({
    selector: 'task-list',
    templateUrl: './task-list.component.html'
})
export class TaskListComponent implements OnInit {

    public selectedTask: Task | null;

    @ViewChild(TaskTableComponent)
    private taskTableComponent: TaskTableComponent;

    constructor(private taskService: TaskService,
        private location: Location,
        private route: ActivatedRoute,
        private systemService: SystemService) {
    }

    ngOnInit(): void {
        this.selectInitialTask();
    }

    onSelectedTaskChange(task: Task) {
        this.location.replaceState(`task-list?id=${task.id}`);
    }

    private selectInitialTask(): void {
        this.route.queryParams.subscribe(params => {
            const taskId = +params['id'];
            if (taskId) {
                this.selectTaskById(taskId);
            }
        });
    }

    private selectTaskById(taskId: number) {
        this.taskService.getTask(taskId).subscribe(task => {
            this.selectedTask = task;
        }, error => {
            this.selectedTask = null;
        });
    }

    private onTasksReload() {
        if (this.selectedTask) {
            this.selectTaskById(this.selectedTask.id);
        }
    }
}
