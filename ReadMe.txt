Book-Reader
===========



EF Migrations
=============

Create:
1. Right click on your main project and choose "Set as startup project"
2. Run tools>nuget package manager>package manager console
3. Select the project 'Book-Reader.Data' which contains the DbContext from drop-down list
4. Run: Add-Migration
5. Enter the name of the new migration

Alternative to Add-Migration:
- dotnet ef migrations add MigrationName -s Book-Reader -p Book-Reader.Data

