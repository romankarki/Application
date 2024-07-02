# Application

Project For Acme

## Using Docker Compose

currently docker compose is not fully configured so go through the manual step

```docker-compose up

```

## Manual Setup

1. First create a docker container for mysql (make sure your port 3306 is not occupied)

```docker run --name mydbconiner -e MYSQL_ROOT_PASSWORD=romankarki -p 3306:3306 -d mysql:latest

```

2. go to application, then in application.api/API and run the dotnet services

```
cd Application/application.api/API
```

```
dotnet build
dotnet run
```

try running the dotnet service at port 5102

```
ASPNETCORE_URLS="http://localhost:5102" dotnet run
```

3. finally install the package and run angular

```
cd Application/application.client
```

```
npm install
ng serve
```
