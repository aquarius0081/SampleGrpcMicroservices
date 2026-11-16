# SampleGrpcMicroservices
I recommend to open solution in the latest version of Visual Studio 2026 with all updates.
.NET 11 should be installed on computer.
Solution is written using modern technologies including .NET 11 WebApi, gRPC, Swagger, FluentValidation.
Nuget is used as .NET package manager.

## Local development environment setup
* Build solution
* Select "Both" startup projects profile. Then it will run "InventoryService" microservice and "OrderService" microservice both at the same time
* Run solution

## Usage
Requests can be sent through swagger of "OrderService" project.
"OrderService" will call "InventoryService" through gRPC and return result from "OrderService" API.