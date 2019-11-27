#FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
#WORKDIR /app
#EXPOSE 9090
#
#FROM microsoft/dotnet:2.2-sdk AS build
#WORKDIR /src
#COPY ["Super.Core.Mvc/Super.Core.Mvc.csproj", "Super.Core.Mvc/"]
#RUN dotnet restore "Super.Core.Mvc/Super.Core.Mvc.csproj"
#COPY . .
#WORKDIR "/src/Super.Core.Mvc"
#RUN dotnet build "Super.Core.Mvc.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "Super.Core.Mvc.csproj" -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "Super.Core.Mvc.dll"]

FROM microsoft/dotnet:2.2-aspnetcore-runtime
MAINTAINER yangyangang
WORKDIR /app
COPY . .
VOLUME super.core.mvc /app/Config
EXPOSE 9090
ENTRYPOINT ["dotnet", "Super.Core.Mvc.dll"]