sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=xxxxx!xxx" \
   -p 1433:1433 --name SMSSQL -h sql1 \
   -d mcr.microsoft.com/mssql/server:2019-latest

   sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Tmhctrdocker load -i uiretail-dev-157.tar "    -p 1433:1433 --name SMSSQL -h sql1    -d mcr.microsoft.com/mssql/server:2019-latest


password "Tmhctrdocker load -i uiretail-dev-157.tar "
 
dotnet ef dbcontext scaffold "Server=xxx.xx.xx.xxx;Initial Catalog=ERXTest;User ID=admin;Password=xxxx!xx;Connection Timeout=120;" Microsoft.EntityFrameworkCore.SqlServer -o Models