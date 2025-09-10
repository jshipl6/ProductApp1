# ProductApp

This is a simple ASP.NET Core MVC app created for class.  
It displays a list of products, allows adding new ones, and uses Tag Helpers and logging.

## Features
- MVC pattern (Models, Views, Controllers)
- `ProductController` with strongly typed `Index` view
- Tag Helpers for navigation and form posts
- ILogger logging useful info in controller actions
- Bootstrap styling for layout and forms

## Requirements
- Visual Studio 2022 (with ASP.NET and web workload installed)
- .NET 6 SDK or newer

## Design note: Separation of Concerns & DI

I introduced a domain service `IPriceCalculator` with an implementation `PriceCalculator`.  
The service is **stateless**, so I registered it as a **Singleton** in `Program.cs`:
```csharp
builder.Services.AddSingleton<IPriceCalculator, PriceCalculator>();

## Route Map (Assignment 3)

### Controllers using attribute routing
- **HomeController**
  - `GET /` → Home/Index
  - `GET /home` and `/home/index` → Home/Index
  - `GET /privacy` (also `/home/privacy`) → Home/Privacy
  - `GET /error` → Home/Error

- **ProductController** (feature folder: `/Features/Products`)
  - `GET /products` → list products (optional `?q=` filter)
    - Examples:
      - `/products`
      - `/products?q=pick`
  - `GET /products/create` → form
  - `POST /products/create` → create product
  - `GET /products/{id}` → details (e.g., `/products/2`)
  - `GET /products/{id}/edit` → edit form
  - `POST /products/{id}/edit` → save edit
  - `GET /products/{id}/delete` → confirm delete
  - `POST /products/{id}/delete` → delete

### View locations
- Feature-first search: `/Features/{ControllerNameWithoutSuffix}/{ViewName}.cshtml`
- Shared feature views: `/Features/Shared/{View}.cshtml`
- Fallback to MVC defaults: `/Views/{Controller}/{View}.cshtml`, `/Views/Shared/{View}.cshtml`

## Persistence
The app uses **Entity Framework Core (SQLite)** for persistence. Products are stored in `app.db`.
Migrations are included (`InitialCreate`). To recreate the DB:
- Run `Update-Database` in Package Manager Console.




## How to Run
1. Clone the repo:
   ```bash
   git clone https://github.com/jshipl6/ProductApp2.git
