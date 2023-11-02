# PROJEKT NA MVC


### DEV: MIGRACJE
za wczasu zainstaluj ef-tools: ```dotnet tool install --global dotnet-ef```

przejdź cmd do katalogu z plikiem .sln, a następnie 
- Uruchomienie migracji: 
```dotnet ef migrations add NazwaMigracji --startup-project MvcProject.API --project MvcProject.Infrastructure```
- Update struktury bazki: 
```dotnet ef database update --startup-project MvcProject.API --project MvcProject.Infrastructure```

Na sam koniec odpal na bazce plik ```dane_z_bazki.sql```