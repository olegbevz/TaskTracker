import { Component, Input, Output, EventEmitter, ChangeDetectorRef, OnInit, OnDestroy } from "@angular/core";
import { TaskService } from "../../services/task.service";
import { Task, TaskStatus, SortOrder } from "../../models/task";
import { ConfigurationService } from "../../services/configuration.service";
import { Configuration } from "../../models/configuration";
import { Duration } from "moment";
import * as moment from "moment";
import { Observable, Subscription } from "rxjs";
import { TaskTracker } from "../../services/task.tracker";

@Component({
    selector: 'task-table',
    templateUrl: './task-table.component.html',
    styleUrls: ['./task-table.component.css']
})
export class TaskTableComponent implements OnInit, OnDestroy {

    private initialScrollWindow: number = 100;
    private scrollWindow: number = this.initialScrollWindow;
    private scrollStartIndex: number = 0;
    private scrollStep: number = this.scrollWindow / 2;
    private taskStatus?: TaskStatus;
    private sortOrder?: SortOrder; 
    private configuration: Configuration;
    private timer: Subscription;
    public tasks: Task[] = []; 

    @Input() public selectedTask: Task;
    @Output() public selectedTaskChange = new EventEmitter();
    @Output() public tasksReload = new EventEmitter();

    constructor(
        private taskService: TaskService,
        private taskTracker: TaskTracker,
        private configurationService: ConfigurationService,
        private changeDetector: ChangeDetectorRef) {

            this.taskTracker.tasks.subscribe(tasks => this.onTasksReloaded(tasks));
            this.configuration = configurationService.getConfiguration();
    }

    ngOnInit(): void {
        this.reloadTasks();

        this.timer = Observable.interval(1).subscribe(() => {
            this.changeDetector.detectChanges();
        });
    }

    ngOnDestroy(): void {
        if (this.timer) {
            this.timer.unsubscribe();
        }

        if (this.taskTracker) {
            this.taskTracker.unsubscribe();
        }
    }

    public selectTask(task: Task): void {
        this.selectedTask = task;
        this.selectedTaskChange.emit(this.selectedTask);
    }

    public setTaskStatus(status?: TaskStatus) {
        this.taskStatus = status;
        this.reloadTasks();
    }

    public setSortOrder(sortOrder: SortOrder) {
        if (this.sortOrder != sortOrder) {
            this.sortOrder = sortOrder;
            this.reloadTasks();
        }
    }

    public getRowColor(task: Task, index: number): string {
        if (this.selectedTask && task.id === this.selectedTask.id) {
            return '#F0FFF0';
        }

        if (index % 2 === 0) {
            return this.configuration.altRowsColor;
        }

        return '#FFFFFF';
    }

    public onCompleteButtonClick(task: Task): void {
        const updatedTask = <Task>{
            ...task,
            status: 'completed'
        };
        this.taskService.updateTask(updatedTask).subscribe(() => { }, error => {
            console.log(error);
        });
    }

    public onRemoveButtonClick(task: Task): void {
        this.taskService.removeTask(task.id).subscribe(() => { }, error => {
            console.log(error);
        });
    }

    public onTableScrollDown(event: any): void {
        this.scrollWindow += this.scrollStep;
        this.reloadTasks();
    }

    public onTableScrollUp(event: any): void {
        if (this.scrollWindow > this.initialScrollWindow) {
            this.scrollWindow = this.initialScrollWindow;
            this.reloadTasks();
        }
    }

    private reloadTasks() {
        this.taskTracker.subscribe({
            taskStatus: this.taskStatus,
            sortOrder: this.sortOrder,
            skip: this.scrollStartIndex,
            take: this.scrollWindow
        });
    }

    private onTasksReloaded(tasks: Task[]): void {
        if (tasks.length > 0 && this.selectedTask == null) {
            this.selectTask(tasks[0]);
        }

        if (tasks.length > 0 && this.selectedTask != null) {
            var task = tasks.find(task => task.id === this.selectedTask.id);
            if (task) {
                this.selectTask(task);
            }
        }

        if (tasks.length < this.scrollWindow && tasks.length > this.initialScrollWindow) {
            this.scrollWindow = tasks.length;
        }
        else if (tasks.length < this.scrollWindow && tasks.length < this.initialScrollWindow) {
            this.scrollWindow = this.initialScrollWindow;
        }
        else if (tasks.length > this.scrollWindow) {
            this.scrollWindow = tasks.length;
        }

        this.tasksReload.emit();
    }

    public getTimeToComplete(task: Task): Duration | null {
        if (task.status == 'completed') {
            return null;
        }

        const expiresAt = moment.utc(task.added)
            .add(task.duration);
        const now = moment.utc();

        if (expiresAt.isAfter(now)) {
            const diff = expiresAt.diff(now);
            return moment.duration(diff);
        }

        return null;
    }
}