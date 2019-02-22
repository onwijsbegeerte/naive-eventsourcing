FROM microsoft/dotnet:sdk AS build-env
RUN pwd
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./Naive-Eventsourcing.sln ./
COPY front-end/*.fsproj ./front-end/
COPY application-services/*.fsproj ./application-services/
COPY core-domain/*.fsproj ./core-domain/

RUN dotnet restore
# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out
RUN pwd
# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/front-end/out .
ENTRYPOINT ["dotnet", "Api.dll"]