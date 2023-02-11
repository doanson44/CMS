## FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
## WORKDIR /app
## EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0
#WORKDIR /src
#EXPOSE 80
## COPY ["CMS_Backend.sln", "CMS_Backend/"]
## COPY ["CMS.Core/CMS.Core.csproj", "CMS_Backend/"]
## COPY ["CMS.Infrastructure/CMS.Infrastructure.csproj", "CMS_Backend/"]
## COPY ["CMS.Scheduler/CMS.Scheduler.csproj", "CMS_Backend/"]
#COPY ["CMS.WebApi/CMS.WebApi.csproj", "CMS.WebApi/"]
#RUN dotnet restore "CMS.WebApi/CMS.WebApi.csproj"
#COPY . .
#WORKDIR "/src/CMS.WebApi"
#RUN dotnet publish "CMS.WebApi.csproj" -c Release -o ./publish
#WORKDIR /app
#COPY ./publish .
#ENTRYPOINT ["dotnet", "CMS.WebApi.dll"]
## FROM build AS publish
## RUN dotnet publish "CMS.WebApi.csproj" -c Release -o ./publish
#
## FROM base AS final
## WORKDIR /app
## COPY /app/publish .
## ENTRYPOINT ["dotnet", "CMS.WebApi.dll"]
#
