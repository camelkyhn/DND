set /p migrationName=Please enter a migration name to unapply: 
dotnet ef database update %migrationName% --project DND.Storage --startup-project  DND.Web\DND.Web.Server