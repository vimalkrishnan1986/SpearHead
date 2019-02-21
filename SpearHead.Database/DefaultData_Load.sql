/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

print 'Loading default data into packet Table'

Insert into dbo.Packets(PacketId,PacketCode) values(NEWID(),'P1');
Insert into dbo.Packets(PacketId,PacketCode) values(NEWID(),'P2');
Insert into dbo.Packets(PacketId,PacketCode) values(NEWID(),'P3');

print 'End of loading packet table'

print 'Loading produt Table'

Insert into dbo.Products(ProductId,ProductCode) values(NEWID(),'PD1');
Insert into dbo.Products(ProductId,ProductCode) values(NEWID(),'PD2');
Insert into dbo.Products(ProductId,ProductCode) values(NEWID(),'PD3');

print 'End of loading product Table'