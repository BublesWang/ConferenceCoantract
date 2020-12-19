dotnet publish -c Release -r linux-x64
docker build -f DockerfilePublish -t 47.104.249.194:5000/conferencecontractserver:latest .
docker push 47.104.249.194:5000/conferencecontractserver:latest
curl -u onlinetest:564b8580782317fc7c85dc7846251251 http://47.104.249.194:7500/job/SNEC_ONLINE_DEBUG/build?token=rebuildtest
pause