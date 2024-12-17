# TravelExpertsMVC
Set TravelExpertsData as Startup project
Uncomment onConfiguring() method in TravelExpertsContext File in TravelExpertsData Model
Start by running the Migration file in the console "Update-Database -Migration identityUpdate"
Set TravelExpertsMVC to startUp Project
Comment onConfiguring() method in TravelExpertsContext File in TravelExpertsData Model
Run modified TravelExpertsMssqlFile
Run Compiled SQL queries
For web register an account,
For Desktop find login details below.
Find attached modified TravelExperts  mssql file and compile Queries

Test accounts for desktop app


Admin Email 'johndoe@gmail.com' Password 'AdminPassword123'
Admin Email: 'femi@yahoo.com' Password: 'FemiSecurePass!'
Agent Email: 'janet.delton@travelexperts.com' Password: 'JanetAgentPass$'

To add creditCard to your account
-- Insert a new credit card for CustomerId 127 with a balance of 50000 dollars
INSERT INTO [dbo].[CreditCards] ([CCName], [CCNumber], [CCExpiry], [CustomerId], [Balance])
VALUES ('Visa', '1234-5678-9012-3456', '2025-12-31', 127, 50000);
go
