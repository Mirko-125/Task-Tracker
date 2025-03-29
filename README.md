
# Task tracker

A brief cli app to track tasks  

# How to use
You can start by typing this command and will receive the app usage manual
```console
dotnet run help
```
the manual:
`add "Task description"`  
  - Adds a new task with the given description.

- `update <taskId> "New description"`  
  - Updates the description of the specified task.

- `delete <taskId>`  
  - Deletes the task with the given ID.

- `list [--notdone | --done | --inprogress]`  
  - Lists tasks based on their status:
    - `--notdone`: Shows tasks that are not done.
    - `--done`: Shows completed tasks.
    - `--inprogress`: Shows tasks currently in progress.

- `mark <taskId> [1 | 2]`  
  - Updates the status of a task:
    - `1`: Marks the task as **In Progress**.
    - `2`: Marks the task as **Done**.

afterwards you can enter a desired command by adding next to `dotnet run` arguments from the top 

# Requirements

The application should run from the command line, accept user actions and inputs as arguments, and store the tasks in a JSON file. The user should be able to:

- Add, Update, and Delete tasks
- Mark a task as in progress or done
- List all tasks
- List all tasks that are done
- List all tasks that are not done
- List all tasks that are in progress

# Task model

- id: A unique identifier for the task
- description: A short description of the task
- status: The status of the task (not done, in-progress, done)
- createdAt: The date and time when the task was created
- updatedAt: The date and time when the task was last updated

This project followed and completed the tasks from [this](https://roadmap.sh/projects/task-tracker) part of a roadmap.