## Database setup

Option 1 – Using SQL Server Management Studio

1. Open SQL Server Management Studio
2. Connect to localhost\SQLEXPRESS
3. Open the script located at /database/init.sql
4. Execute the script (F5)

Option 2 – Using sqlcmd

sqlcmd -S localhost\SQLEXPRESS -E -i database/init.sql
