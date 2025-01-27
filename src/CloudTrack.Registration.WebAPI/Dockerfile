#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG NUGET_URL
ARG NUGET_USERNAME
ARG NUGET_PASSWORD

WORKDIR /src
COPY ["src/CloudTrack.Registration.WebAPI/CloudTrack.Registration.WebAPI.csproj", "src/CloudTrack.Registration.WebAPI/"]
COPY ["src/CloudTrack.Registration.Application/CloudTrack.Registration.Application.csproj", "src/CloudTrack.Registration.Application/"]
COPY ["src/CloudTrack.Registration.Domain/CloudTrack.Registration.Domain.csproj", "src/CloudTrack.Registration.Domain/"]
COPY ["src/CloudTrack.Registration.Infrastructure/CloudTrack.Registration.Infrastructure.csproj", "src/CloudTrack.Registration.Infrastructure/"]
RUN dotnet nuget add source --username $NUGET_USERNAME --password $NUGET_PASSWORD --store-password-in-clear-text --name beniamin_github "$NUGET_URL"
RUN dotnet restore "src/CloudTrack.Registration.WebAPI/CloudTrack.Registration.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/CloudTrack.Registration.WebAPI"
RUN dotnet build "CloudTrack.Registration.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudTrack.Registration.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudTrack.Registration.WebAPI.dll"]