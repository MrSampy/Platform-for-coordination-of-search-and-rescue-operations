FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /app/build/OperationsService.API

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

WORKDIR /app

EXPOSE 5160

EXPOSE 7088

EXPOSE 8080

ENV TZ=Europe/Kiev

COPY --from=build /app/build/OperationsService.API .

CMD ["dotnet", "OperationsService.API.dll"]