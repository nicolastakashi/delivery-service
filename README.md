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
- GZip
- Redis
- XUnit
- Fluent Assertions

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

`` $ docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d``

That done we will have our application running, just access the address below and you will see the Swagger homepage with all the specification of service endpoints:

Service Address: https://localhost:44372

There is a data load on the service with all points and connections between points that existed in the problem statement.

There are currently two registered users:

### Admin: 
Admin can manage all information regarding Points, Point Connections and Routes.

**Email:** admin@deliveryservice.com

**Password:** admin

### User:
User can only queries the platform information.

**Email:** user@deliveryservice.com

**Password:** user

## Running the tests

Currently there are two types of tests implemented, unit tests and integration tests, were implemented in separate projects, so we can run either test separately.

### Unit Tests:
To perform unit tests through the console just run the command below:

`` $ dotnet test .\DeliveryService.Test.Unit\DeliveryService.Test.Unit.csproj``

### Integration Tests:
Integration tests have a high execution cost when compared to unit tests, because every time we run the command to run our service integration tests, a dedicated docker container for provisioning tests is provisioned, so we have the warm up time for this infrastructure.

To run the integration tests through the console just run the command below:

`` $ dotnet test .\DeliveryService.Test.Integration\DeliveryService.Test.Integration.csproj``

## Improvements and evolutions

### Resilience
Implementation of resilience patterns such as Retry Pattern and Circuit Breaker.

To ensure the resiliency of our service, we can perform this implementation using Polly which is an extremely simple and consolient library in the .NET world.

### Rate Limit
In the current solution the service is not addressing the rate limit issue, but it is an implementation that should be performed if the company in question does not have an API Management, there are some options to perform this implementation either through an API Management/API Gateway or using a package called **AspNetCoreRateLimit**

### Performance Tests
Due to task priority, no performance testing has been implemented, this implementation could be prioritized at or near time, so we can implement it using Apache JMeter or some similar tool.

### Build Pipeline
Adding the service within an automated build and deploy pipeline to ensure the integration of developed code and streamline the deployment process in multiple environments.

This kind of process can be done by tools such as:

- Jenkins
- Azure DevOps
- Circle CI

### Message Queue

A necessary implementation due to the design adopted would be background processes using a message queue reacting to our service event.

Currently the entities of routes and connections are aggregates that contain their information and a value object that represents a point.

It is necessary to implement a mechanism that every time a point is updated, it has a service that reacts to the event in order to update the routes and connections that use the point in question.

### Code Coverage

At this first moment no code coverage tool was implemented for unit testing.

This type of implementation can be done using **Open Cover** and **Report Generator**.

This would allow us to have reports of our project code coverage within our automated build process.

### XML Docs
Improve API specification by using Swagger integration with XML Docs, so we can make it clear to those who are consuming the service what each item does.

And it would make integration with client-generating tools for services based on the Open API specification even better.

### HATOAS
Implement HATOAS to make service consumption simpler and more fluid so that the service is high explanatory.

### SDK and Code Samples

In the case of a public service it is necessary to implement code examples to assist customers who are consuming the service.

And in an ideal world there is an SDK abstracting all the complexity and providing a Fluent API for customers who consume the service.

### Refresh Token

To increase the security of our service, we need to implement a refresh token strategy so that authentication tokens would have a lifetime and every time a token expires we would be able to renew your access.

### Architecture Evolution

Currently we have a simple and decoupled architecture, but with the evolution of the project it is necessary to emerge the architecture.

Perform further separation of responsibilities from the domain layer, perhaps by separating into contexts or creating a core domain layer.

Continue the implementation of unit tests in order to have greater test coverage, especially in the Dijkstra algorithm part.