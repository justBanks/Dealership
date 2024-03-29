# Car Dealership Inventory
ASP.NET Core 2.2 + OAuth2 sample 

- **Domain:** Car Dealership
- **Interface:** Web API
- **Front end:** ASP.NET Core MVC

## OAuth2

The OAuth implementation demonstrates the full redirect flow for obtaining an access token. The user must first authenticate with the authorization server, then grant the application permission to act on the user's behalf. This implementation was chosen because at the outset I deemed it to be the most appropriate for JavaScript and mobile clients.

**Login credentials** >> ```test-user:test ```

## Startup
- The solution is set to lauch three startup projects in IIS Express, plus one browser window for the UI:

![img](/options.png "Startup projects")

- The project targets version 2.2 of ASP.NET Core&mdash;hopefully this won't cause issues for anyone 

## Notes
In the documentation, the Car Options list includes _automatic transmission,_ which is omitted in the sample data. I took this as an oversight and included it in the model.

When the automatic transmission option is selected, records are filtered accordingly; all transmission types are returned otherwise.

By default, a car has 'low miles' if its mileage does not exceed 15,000.

Postman collection for requesting tokens and testing the API:  
Import https://www.getpostman.com/collections/87ea3d0c3a2c4f2cff4b
