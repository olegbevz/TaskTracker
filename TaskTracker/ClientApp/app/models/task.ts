import * as moment from "moment";
import { Moment, Duration } from "moment";

export type TaskStatus = 'active' | 'completed';

export type SortOrder = 'name' | 'priority' | 'added' | 'duration';

export class Task {
    public id: number;
    public name: string;
    public description: string;
    public priority: number;
    public added: Moment;
    public edited: Moment;
    public duration: Duration;
    public status?: TaskStatus;

    public static fromJson(json: any): Task {
        return <Task>{
            ...json,
            duration: moment.duration(json.duration),
            added: moment(json.added),
            edited: moment(json.edited),
        }
    }

    public static json(task: Task): any {
        return {
            ...task,
            duration: moment().startOf('day').add(task.duration).format("hh:mm:ss"),
            added: task.added.format("YYYY-MM-DD hh:mm:ss"),
            edited: task.edited.format("YYYY-MM-DD hh:mm:ss")
        }
    }
}

