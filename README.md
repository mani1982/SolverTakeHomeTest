# SolverTakeHomeTest


At the beginning "Background" section made me really confuse and brought questions to me, but the problem by itself is clear and reasoable to do. 
To solve this problem I decided to use ADO.NET over EF, because I wasn't sure if they are going to provide the connection string, and this way is easier and faster to pull data.

I created multiple endpoints:

1) SetConnectionString => this end point set the connection string if it is not set on configuration. This method just return 200 if connection was successful. 
the url for this end point looks like => https://localhost:44360/data/SetConnectionString?connectionString=Server=MANI-DELL\SQLEXPRESS;Trusted_Connection=True;MultipleActiveResultSets=true

2) ListOfDBByConnectionString endpoint is for get list of databases by connection string, and url would be:
 https://localhost:44360/data/ListOfDBByConnectionString?connectionString=Server=MANI-DELL\SQLEXPRESS;Trusted_Connection=True;MultipleActiveResultSets=true
 
3) GetTableData endpoint is the solution for this challenge, and returns the amount of data based on offset and count parameters for pagination.
Based on explation of Background section, since we dont have much idea about how the tables shema and structure looks like it made it hard to use EF, and ADO becomes
more handy and flexible to use to fetch data. My Assumption here is either they already entered connection string in config file or they used "SetConnectionString" 
endpoint to set connection string.
url => https://localhost:44360/data/DimOrganization?offset=10&count=1


I am sure my solution still has some room to improve if there was more details on challenge. Also we can have more parameter checking for more 
security purposes like SQL injection. 

q) Can we make the amount of JSON returned smaller in any way?
I think by portioning (paginating) the return data, we can make the json file smaller, for instance, just fetch 10 records at a time.

q) Can we add support for paging?
I already implement it in row SQL.

q) Can we add a simple front-end to show it working?
for sure, I will prepare it for next challenge to show.

q) Can we follow foreign keys to display values?
since easily we can get information about each table schema, like each column type, so we can find the foreign key and based on that run more query in deeper level. 


