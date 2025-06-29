# Dynamic-Configuration-Management
This project is a Dynamic Configuration Management System developed with ASP.NET Core MVC and MongoDB.

### _Architecture_
- ConfigurationLibrary         
- ConfigurationWebUI
- ConfigurationLibrary.Tests      

### _Technologies Used_
- ASP.NET Core MVC (.NET 8)
- MongoDB (via Docker)
- xUnit for Testing
- Bootstrap for UI

## Setup Instructions
### _MongoDB via Docker_
```ruby
	docker run -d -p 27017:27017 --name mongodb mongo
```

### _Running the Project_
```ruby
	dotnet build
  dotnet run --project ConfigurationWebUI
```
### _Running Tests_
```ruby
	dotnet test ConfigurationLibrary.Tests
```
