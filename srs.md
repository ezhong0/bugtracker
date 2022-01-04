## BugTracker
Universal issue and project tracker!
### 1. Document Overview
BugTracker is a Web-based bug tracking system written in .NET and C#, while employing MVC and SQL technologies. This SRS will lay out the project goals, specifications and requirements
### 2. Purpose
This platform gives a development team a tool to track and kill bugs, as well as a space to collaborate and discuss all aspects of a project. Keeping this record of software project growth greatly facilitates development
### 3. Intended Audience / Use
Bug tracking software is widely used by both corporations and private individuals or groups. Any party seeking to develop software can make use of this universal bug tracking tool.
### 4. Assumptions and Dependencies
BugTracker, as a Web-based application, will be written in C#, utilising the .NET framework through Microsoft's Visual Studio Code. The frontend will be built with HTML5 and Bootstrap. Database management will be through SQL databases. Version Control will be through git at https://github.com/ezhong0/bugtracker
### 5. Requirements
This section will describe both functional and non-functional requirements
#### 5a. Functional Requirements
##### End User Requirements
| Description | Notes |
| --- | --- |
| Create user/admin account | Account linked through email. (MFA? Demo account? ) |
| Login account | "Forgot password" functionality |
| Create project | Restrict to admin |
| Assign users to project| Restrict to admin |
| Create ticket | Add attachments to ticket (images, docs) |
| Assign ticket to user | |
| Edit ticket | |
| Ticket edit history | |
| Ticket comments | Forum-style comment section for tickets |
| Ticket organization | List tickets by owner/developer/projects |
| Ticket filter/sorting | List tickets by priority, type, status, creation/update date etc |
| Ticket search | Text search for tickets |

##### UI Requirements
| Page | Notes |
| --- | --- |
| Dashboard | Overview of all tickets |
| Manage Users | Admin only: Assign roles, list of all developers, add/remove users |
| Projects | List of Projects |
| Project Details | List of users for a project, list of tickets |
| Tickets | List of all tickets |
| Ticket Details | All information regarding ticket, incl. history, comments |
| User Profile | Edit information, user image, password change, info box |

#### 5b. Non Functional Requirements