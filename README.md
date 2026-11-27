# E-Commerce API
REST API for an online store with accounts, orders and Stripe payments.
Live demo: https://react-ecommerce-romza.runasp.net/ | Frontend: https://github.com/Romzaa/E_commerce_Frontend

## Stack
.NET 10, ASP.NET Core Web API, EF Core, SQL Server, ASP.NET Identity, JWT, Redis (StackExchange.Redis), Stripe.net, AutoMapper, Swagger

## Features
- Catalog: products, brands, types, pagination, sorting, filtering
- Authentication: register, login, current user, saved delivery address (JWT bearer)
- Role-based admin API (SuperAdmin): manage products, brands, types and orders
- Orders with delivery methods
- Stripe payments: payment intents per cart, and a webhook that verifies the Stripe signature and updates payment status (succeeded, failed)
- Redis: 120-second response cache on the product list, shopping carts stored in Redis with a 7-day expiry
- Database migration and seed data on startup
- Serves the built React app from the same host

## Architecture
API, Application, Domain, Infrastructure layers. Generic repository, unit of work, specification pattern, result pattern.

## Configuration
Never commit real secrets. Copy appsettings.Example.json to appsettings.Development.json and set:
ConnectionStrings:DefaultConnection, ConnectionStrings:IdentityConnection, ConnectionStrings:RedisConnection, Jwt:SecretKey, Jwt:Issuer, Jwt:Audience, Stripe:SecretKey, Stripe:WebhookSecret, Stripe:DefaultCurrency, UrlSettings:BaseUrl

## Run
dotnet run --project E_Commerce.API, then open /swagger
