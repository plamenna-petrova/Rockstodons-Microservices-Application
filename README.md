
## Architectural Principles

The project is based on the principles of the CAT architectural pattern, which is often implemented when building Single Page Applications / SPAs that communicate with server-side APIs. The project's architecture can be divided into three main components:

 - Clients : They represent the front-end part of the application and are usually built using JavaScript / TypeScript frameworks such as Angular, React or Vue.js. The clients are responsible for the processing of requests, which result from user actions, for displaying the user interface, and for managing the application's state.
 - API resources: They define the server side of the application and handle client's requests, whereby they execute business logic and interact with databases.
 - Security Token Service (STS) : This is a commonly used service that is responsible for managing authentication and authorization of users and clients.

Following the principles of the CAT architectural pattern, clients communicate with API resources through a Web API to retrieve and update data. API resources can also interact with the Security Token Service (STS) to validate user credentials and to issue JWT tokens.

## Codebase Organization

The codebase of the project is organized in one repository, which is conditionally divided into Back-End and Front-End directories. In the Services subdirectory of the application (under Back-End) the .NET microservices can be found, which correspond to the Security Token Service (STS) as well as the microservice for the main API resource. Furthermore, in the same directory an administrative MVC client has been added, belonging to the STS service. In the Front-End directory, the main web client is also located, which is a SPA application, developed with Angular and which interacts with the Security Token Service and the main API resource.

In this project, two client applications have been created:
- A MVC application, built with ASP.NET Core for interaction with the Security Token Service (STS).
- A single page application (Single Page Application / SPA), written in TypeScript with a chosen Front-End framework - Angular, through which communication is carried out with the Security Token Servcie (STS) and the main API resource. The music streaming logic is also developed in it.

The project also includes the back-end services:
- A microservice, which integrates the ASP.NET Core Identity and Duende IdentityServer frameworks.
- A catalog-type microservice, which has the role of a data management service and is responsible for performing the basic CRUD operations - create, read, update and delete records to a consumed SQL database using the Entity Framework Core framework. It also contains the logic for audio files and images management. Its ASP.NET Core Web API is modeled as the main API resource in the IdentityServer's context.

## How to run the project

- Back-End : Make sure to select the option 'Multiple Startup Projects'. The projects that need to be configured for start up are : Catalog.API, TokenService.Admin,
TokenService.Admin.Api and TokenService.STS.Identity.
- Front-End : Install the dependencies and run the project with npm run start / serve commands.
