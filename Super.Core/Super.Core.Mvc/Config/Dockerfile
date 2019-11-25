FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 9090

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Pims.Psa.Web/Pims.Psa.Web.csproj", "Pims.Psa.Web/"]
RUN dotnet restore "Pims.Psa.Web/Pims.Psa.Web.csproj"
COPY . .
WORKDIR "/src/Pims.Psa.Web"
RUN dotnet build "Pims.Psa.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pims.Psa.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pims.Psa.Web.dll"]