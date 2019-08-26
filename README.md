# Delivery Service

A company has a need to ship products purchased by its customers from one point of origin to a destination point, currently this process is done manually which makes the process painful and extremely costly.

It is necessary to develop a service to automate this process so that we have quick and correct answers about what is the best delivery route based on cost or time.

## Solution
Based on what was required and the information contained in the specification, a Rest Service was developed using the following technologies and standards:

### Technologies
- ASP.NET Core
- .NET Core
- C#
- Docker
- MongoDb
- MediatR
- Bogus
- Docker & Docker Compose
- Fluent Assertions
- Swagger

### Patterns and Principles

- OOP
- SOLID
- CQS
- Design Patterns
- Clean Code
- Automated Tests (Unit and Integration)

## Tools

To get the localhost solution running we just need the following tools:

- .NET Core SDK
- Docker & Docker Compose
- Text editor - (Preferably Visual Studio Code)
- Console

## Running the app

After cloning the project and having the above tools installed, simply run the command below to run the localhost application:

`` $ docker-compose -f docker-compose.yml -f docker-compose.override.yml``

## Running the tests.

Currently there are two types of tests implemented, unit tests and integration tests, were implemented in separate projects, so we can run either test separately.

### Unit Tests:
To perform unit tests through the console just run the command below:

`` $ dotnet test .\DeliveryService.Test.Unit\DeliveryService.Test.Unit.csproj``

### Integration Tests:
Integration tests have a high execution cost when compared to unit tests, because every time we run the command to run our service integration tests, a dedicated docker container for provisioning tests is provisioned, so we have the warm up time for this infrastructure.

To run the integration tests through the console just run the command below:

`` $ dotnet test .\DeliveryService.Test.Integration\DeliveryService.Test.Integration.csproj``
