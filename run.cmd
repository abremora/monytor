cd Monytor.Scheduler.NetCore\bin\publish\Release
start cmd /c dotnet Monytor.Scheduler.NetCore.dll
cd ../../../../Monytor.WebApi/bin/publish/Release

start cmd /c dotnet Monytor.WebApi.dll
cd../../../..