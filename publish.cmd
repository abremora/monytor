cd Monytor.Sheduler.NetCore
dotnet publish --source https://api.nuget.org/v3/index.json -c Release -o ./bin/publish/Release/

cd..
cd Monytor.WebApi
cmd.exe /c gulp -b --color default
dotnet publish --source https://api.nuget.org/v3/index.json -c Release -o ./bin/publish/Release/

cd..