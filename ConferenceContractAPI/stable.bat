dotnet publish -c Release -r linux-x64
docker build -f DockerfilePublish_stable -t 47.104.249.194:5000/conferencecontractserver:stable .
docker push 47.104.249.194:5000/conferencecontractserver:stable
curl -u onlinetest:564b8580782317fc7c85dc7846251251 http://47.104.249.194:7500/job/MIRROR_PRODUCTION/build?token=MIRROR_PRODUCTION
pause