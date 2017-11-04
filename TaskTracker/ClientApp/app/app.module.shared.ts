import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http } from '@angular/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { TaskListComponent } from './components/task-list/task-list.component';
import { AddTaskComponent } from './components/add-task/add-task.component';
import { ConfigurationComponent } from './components/configuration/configuration.component';
import { TaskService } from "./services/task.service";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { NgbTimeStructPipe } from "./pipes/ngb-time.pipe";
import { ConfigurationService } from "./services/configuration.service";
import { ColorPickerModule } from "ngx-color-picker";
import { SystemService } from "./services/system.service";
import { TaskTableComponent } from "./components/task-table/task-table.component";
import { TaskDetailsComponent } from "./components/task-details/task-details.component";
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { MomentFormatPipe } from "./pipes/moment-format.pipe";
import { DurationFormatPipe } from "./pipes/duration-format.pipe";
import { TaskTracker } from "./services/task.tracker";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        TaskTableComponent,
        TaskDetailsComponent,
        TaskListComponent,
        AddTaskComponent,
        ConfigurationComponent,
        NgbTimeStructPipe,
        MomentFormatPipe,
        DurationFormatPipe
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'task-list', pathMatch: 'full' },
            { path: 'add-task', component: AddTaskComponent },
            { path: 'task-list', component: TaskListComponent },
            { path: 'settings', component: ConfigurationComponent },
            { path: '**', redirectTo: 'tasklist' }
        ]),
        NgbModule.forRoot(),
        ColorPickerModule,
        InfiniteScrollModule
    ],
    providers: [
        SystemService,
        TaskService,
        ConfigurationService,
        TaskTracker,
        NgbTimeStructPipe
    ]
})
export class AppModuleShared {
}
