cd Monytor.Scheduler.NetCore
dotnet publish --source https://api.nuget.org/v3/index.json -c Release -o ./bin/publish/Release/

cd ..

cd Monytor.WebApi
start cmd /c gulp -b --color default
dotnet publish --source https://api.nuget.org/v3/index.json -c Release -o ./bin/publish/Release/
cd..