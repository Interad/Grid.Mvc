# Recreate NORTHWND demo database

### Download the SQL script for NORTHWND.MDF from:
https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs

The file is called _instnwnd.sql_

### Add IsVip column with the following script:

```SQL
ALTER TABLE Customers ADD IsVip BIT NULL;
GO
UPDATE Customers SET IsVip=ABS(CHECKSUM(NEWID())) % 2;
GO
ALTER TABLE Customers ALTER COLUMN IsVip BIT NOT NULL;
GO
```
