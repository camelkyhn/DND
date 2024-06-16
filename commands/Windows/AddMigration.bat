set /p migrationName=Please enter a migration name: 
dotnet ef migrations add %migrationName% --project DND.Storage --startup-project  DND.Web\DND.Web.Server