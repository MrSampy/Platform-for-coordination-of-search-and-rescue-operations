FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /app/build/VolunteerService.API

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

WORKDIR /app

EXPOSE 5083

EXPOSE 7105

EXPOSE 8080

ENV TZ=Europe/Kiev

COPY --from=build /app/build/VolunteerService.API .

CMD ["dotnet", "VolunteerService.API.dll"]