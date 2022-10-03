# PlanetariumDesk
A software product for the internal system of the Planetarium - a fat client (course project).
## PlanetariumDesk functionality
- Registration of a new employee.
- User authorization.

The following functions are available to the "Administrator":
- output of information about the events of the planetarium;
- displaying the schedule of the event selected by the user;
- displaying the schedule of the employee selected by the user;
- displaying the schedule of the room selected by the user;
- search for events by date;
- the ability to add and edit events from the repertoire;
- the ability to add, edit and delete entries from the schedule;
- displaying information about ordered tickets;
- the ability to delete canceled orders and orders whose contact details are unrealistic;
- displaying the rating of events.

The following functions are available to the "Employee":
- output of the employee's schedule;
- displaying the schedule of the room selected by the user;
- search for the schedule of the selected room by date;
- search for an employee's schedule by date.
## Building and running the project
###  Server
See section "Building and running the project" in the project [PlanetariumWeb](https://github.com/EvgeniaSap/PlanetariumWeb).
### PC
1. Find the `BDUtils.cs` file in the "Planetarium" project folder.

2. Find the following lines in the file.
```
string host = "localhost";
string database = "planetarium";
string username = "root";
string password = "";
```

3. Replace the values in quotes with the appropriate ones for your server.
- `string host` - server IP address.
- `string database` - name of the database (doesn't need to be changed if you didn't change the name of the imported database).
- `string username` - database username.
- `string password` - database user password.

4. Run the project.
## Using the PlanetariumDesk
See the ![Use Case Diagram](https://github.com/EvgeniaSap/PlanetariumDesk/issues/1#issue-1395116457) for details on the available features.
