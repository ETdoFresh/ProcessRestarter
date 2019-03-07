# Process Restarter
A simple utility that restarts programs gracefully or forcefully.
In programs.csv, specify the program and action you would like to take.

## Program 
Name of executable program (without .exe)

## Actions
  - Restart: Gracefully exits and waits for user to take action. Once action is taken, restarts the executable.
  - KillRestart: Kills process without any user input. Starts executable again.
  - Kill: Kills process without any user input.

Note: If multiple executables with same name is running, only the first one is processed.

## Examples of programs.csv
Restarts notepad gracefully.
```
Program,Action
notepad,Restart
```
Forces restart of DropBox.
```
Program,Action
Dropbox,KillRestart
```