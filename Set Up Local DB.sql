--create the database
CREATE DATABASE BookReader
GO

--Using SQL Auth
CREATE LOGIN ReaderOfBooks WITH PASSWORD = 'Passw0rd';
GO

--create the user from the login
Use BookReader
CREATE USER [ReaderOfBooks] FOR LOGIN [ReaderOfBooks]
GO

--To give user SELECT/UPDATE/INSERT/DELETE on all tables
EXEC sp_addrolemember 'db_datareader', 'ReaderOfBooks'
EXEC sp_addrolemember 'db_datawriter', 'ReaderOfBooks'
EXEC sp_addrolemember 'db_ddladmin', 'ReaderOfBooks'

GO
