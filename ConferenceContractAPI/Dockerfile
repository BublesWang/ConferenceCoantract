FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 51275
EXPOSE 44328

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ConferenceContractAPI/ConferenceContractAPI.csproj ConferenceContractAPI/
RUN dotnet restore ConferenceContractAPI/ConferenceContractAPI.csproj
COPY . .
WORKDIR /src/ConferenceContractAPI
RUN dotnet build ConferenceContractAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ConferenceContractAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConferenceContractAPI.dll"]
