# BugTracker

this software developed by edward for bug tracking and agile development :)  
thanks for looking <3  

#### Update 10/26:
I tried hosting it on Heroku but there are so many issues with the mySQL database I don't think it's going to work easily. I'll include some screenshots and feel free to ask me for a demo!
![home](home.png)
![createacc](createacc.png)
![login](login.png)
![dash](dash.png)
![proj](proj.png)
![db](db.png)


#### Update 2/22:
App functional, but Azure hosting turns out to be expensive upon the expiration of the 1 month free trial. Live site is currently temporarily unavailable, as I've disabled the subscription. Will work out how to host on Azure without incurring costs  

#### Update 2/15:
Live server established
https://bugtracker20220124002955.azurewebsites.net/  
mySQL server not connected, but login etc. is fully functional on local  


It's mostly done, two things have to be done:
App is missing some functionality, displaying various tables and charts  
mySQL server is not connected to live site via Azure, I have to figure out how to do that  
Also, some cleanup could be needed to make it look nice. that is not a pretty url  

#### (Old) Progress:
**Jan 4:**  
SRS written  
Barebones web app functional  
Designed relational database in databaseSpecification.md  
Established model in BugTracker/Models/Database.cs  
Created and established mySQL database  
Connected mySQL database to C# model via Entity Framework Core  

**Jan 5:**  
Created Basic Webpage for login  

**To Do:**  
Create Basic Webpage for account creation  
Establish RESTAPI protocol for communication  