How to launch the application:

1. Go to Household Service folder. Open the solution file->Tools->NuGet Package Manager-> Package Manager Console.
2. Run the below 3 steps for creating database as Code first Approach is used for creating database.
	Step-1:Enable-Migrations
	Step-2:Add-Migration user
3. Go to Configuration.cs file Under Migrations Folder. Create an object for UserInfo_Table with valid name under Seed method . Type
	    UserInfo_Table admin = new UserInfo_Table();
            admin.password = "123";
            admin.role = "Admin";
            admin.username = "Admin";
            context.userDetails.Add(admin);
            context.SaveChanges();
	Step-3: Update-database -v
4. Build the HouseHoldApp_Service solution. 
5. Make HouseHoldApp_Service as the start up project.
6. Make service1.svc.cs as the start up page.
7. Launch the service application by pressing F5 in visual studio.
8. While Service application is running in the background , 
   open HouseholdRelationship solution in visual studio.
9. Click on the arrow mark near the Connected Services. 
10.Right click on Service Reference and click on Update Service Reference. 
11.To update reference service should be running in background.
12.Build the solution and run it by pressing F5.
