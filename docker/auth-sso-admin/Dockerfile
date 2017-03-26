FROM microsoft/dotnet:runtime
#FROM ubuntu:latest
WORKDIR /app
COPY src/GovITHub.Auth.Admin/bin/Release/netcoreapp1.1/publish .

EXPOSE 5002

# Restore the necessary packages
#RUN dotnet restore

ENTRYPOINT ["dotnet", "GovITHub.Auth.Admin.dll"]
#SHELL ["/bin/bash", "-c"] 