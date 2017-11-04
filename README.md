# TaskTracker
Test Task for the Sr. Web Developer position in one company

## Technology stack 

* ASP.NET Core 2.0
* Entity Framework Core
* Angular 4
* SQL Server 2012

## Test task description

The application consists of three screens and has a sidebar with buttons on the left to switch the screens. 
The screens are:
*	Add task form
*	Tasks list
*	User settings

Each screen has its own URL and switching the screens should highlight the button of currently selected screen.
Add task screen represents the form to add a new task with the next fields: name, description, priority, time to complete; and a button to save. By default, all the tasks have ‘active’ status.

Tasks list shows a grid of tasks in the system with the following columns: name, priority, added date, time to complete and the one with Complete/Remove button. Time to complete column should back tick every second up to zero and show current remaining time. Button column displays ‘Complete’ button for active tasks and ‘Remove’- for completed. Removed task should disappear from the grid. Clicking on a task should reveal the task details beneath the grid and change URL in the browser. Task details displays a set of fields with task info including task description and its status.

On top of the grid there is a toolbar with refresh button (allows to update the grid with new data) and set of filters to filter out tasks by status.

User settings screen allows specifying date/time format and the color of alt rows in the grid with tasks. If date/time setting is specified the grid should format added date column according to this setting.

Nice to have:
-	Real-time updates in the grid from server
-	Usage of some client-side build system / module bundler
-	Grid should support up to 100k rows with infinity scrolling
-	As a user having an URL of task details I want to see them only copying and pasting this URL in a new browser window.
-	If new tasks arrives to the grid, it should not close the currently opened task details.
-	If a task disappears from the grid, task details should also disappear.

## Screenshot
![image](https://user-images.githubusercontent.com/1709945/32403788-e8dfbee8-c153-11e7-8312-095f2808266c.png)


