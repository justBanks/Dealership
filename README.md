# Car Dealership Inventory
ASP.NET Core 2.2 + OAuth2 sample 

- **Domain:** Car Dealership
- **Interface:** Web API
- **Front end:** ASP.NET Core MVC

## OAuth2

The OAuth implementation demonstrates the full redirect flow for obtaining an access token. The user must first authenticate with the authorization server, then grant the application permission to act on the user's behalf. This implementation was chosen because I deem it to be most appropriate for JavaScript and mobile clients.

**Login credentials** >> ```test-user:test ```

## Startup
- The solution is set to lauch three startup projects in IIS Express, plus one browser window for the UI:

![img](/options.png "Startup projects")

## Notes
The Car Options list includes automatic transmission, which is omitted in the sample data. I took this as an oversight and included it in the model.

When the automatic transmission option is selected, records are filtered accordingly; all transmission types are returned otherwise.

By default, a car has 'low miles' if its mileage does not exceed 15,000.

For the sake of expediency, several tradeoffs were made:
- Implementing OAuth2 using a React front end proved to be a big chore, so ASP.NET MVC was used instead
- The data reposiory uses an in-memory data store
- A proper Entity Framework implementation was abandoned, along with a thorough dependency graph with my usual abstractions (such as IContext and IRepository)
- In a typical work setting, I always take the time to ask clarifying questions when there is ambiguity in my mind about any requirement 

[ Postman collection for testing the API](https://www.getpostman.com/collections/87ea3d0c3a2c4f2cff4b)

