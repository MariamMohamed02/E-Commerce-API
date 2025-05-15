# E-Commerce API

A robust, scalable, and secure RESTful API for an e-commerce platform, developed using ASP.NET Core. This project showcases the implementation of clean architecture principles, including Onion Architecture, Repository Pattern, and Unit of Work, ensuring maintainability and testability.



## Features

* **Product Management**: CRUD operations for products with filtering, sorting, and pagination.
* **User Authentication**: Secure login and registration using JWT tokens.
* **Shopping Cart**: Manage user baskets with add, remove, and update functionalities.
* **Order Processing**: Create and manage orders with payment status tracking.
* **Payment Integration**: Seamless integration with Stripe for handling payments.
* **Caching**: Utilizes Redis for caching frequently accessed data and user baskets.


## Architecture

The project follows the **Onion Architecture** to promote a clear separation of concerns:

* **Core Layer**: Contains the domain entities and interfaces.
* **Application Layer**: Implements business logic and service interfaces.
* **Infrastructure Layer**: Handles data access, external services, and repositories.
* **API Layer**: Exposes HTTP endpoints and handles HTTP requests/responses.

This structure ensures that the core business logic remains independent of external frameworks and technologies.

## Tech Stack

* **Backend Framework**: ASP.NET Core
* **Database**: SQL Server
* **ORM**: Entity Framework Core
* **Authentication**: JWT (JSON Web Tokens)
* **Caching**: Redis
* **Payment Gateway**: Stripe
* **Design Patterns**: Repository Pattern, Unit of Work
* **Architecture**: Onion Architecture
* **API Documentation**: Swagger 


## API Endpoints
![image](https://github.com/user-attachments/assets/bfae5b1d-e5f9-4cd6-b0c3-1635420ce055)
![image](https://github.com/user-attachments/assets/ed99c99d-676b-484e-845b-513ad478c9af)
![image](https://github.com/user-attachments/assets/207c9eea-f8df-4aa3-b54c-5bbae2c210a8)




## Authentication & Authorization

* **Registration & Login**: Users can register and log in using their email and password.
* **JWT Tokens**: Upon successful login, a JWT token is issued for authenticating subsequent requests.

## Payment Integration

Payments are handled using Stripe:

* **Payment Intent**: Created when an order is placed.
* **Webhook**: Listens for payment success events to update order status.


## Redis

Redis is used to:

* **Product Listings**: To reduce database load for frequently accessed data.
* **User Baskets**: To provide quick access and updates to user baskets.

