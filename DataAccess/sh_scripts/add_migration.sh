cd ../../
export ASPNETCORE_ConnectionStrings__Projects="Host=localhost;Port=55432;Database=Projects;Username=postgres;Password=root"
dotnet ef migrations add Comments -o Data/Migrations --startup-project 'ProjectManager\ProjectManager.csproj' --project 'DataAccess\DataAccess.csproj'
read -p "Press enter to continue"