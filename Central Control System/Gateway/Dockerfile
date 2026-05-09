FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /app/build/Gateway.Integration.Api

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

WORKDIR /app

EXPOSE 5065

EXPOSE 7038

EXPOSE 8080

ENV TZ=Europe/Kiev

COPY --from=build /app/build/Gateway.Integration.Api .

CMD ["dotnet", "Gateway.Integration.Api.dll"]