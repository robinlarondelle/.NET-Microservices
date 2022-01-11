# The first step is to build the application in the docker image using the sdk
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the .csproj file of the PlatformService to the /app working directory
# this is used to build the dependencies of the project
COPY *.csproj ./
RUN dotnet restore

# with the dependencies installed, we can copy the source files to the workdir and build the project using 'publish'
# the build files are placed in the '/app/out/' dir
COPY . ./
RUN dotnet publish -c Release -o out

# To make the image smaller, multistage building is used to create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# here we copy the output of the building environment, that is placed in the /app/out dir to the workingdir
COPY --from=build-env /app/out .

# The entrypoint specifies what the image will actually run, in this case it will be 'dotnet PlatformService.dll'
ENTRYPOINT ["dotnet", "CommandService.dll"]