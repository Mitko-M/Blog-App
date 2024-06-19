# BlogApp

Welcome to BlogApp, a simple yet powerful blogging platform built with ASP.NET MVC and SQL. This application allows you to create, edit, and manage your blog posts with ease.

## Features

- **User Authentication**: Secure login and registration functionality.
- **Blog Post Management**: Create, edit, and delete blog posts.
- **Comment System**: Readers can leave comments on your posts.
- **Category Filtering**: Organize posts by categories.
- **Search Functionality**: Find posts quickly with an integrated search feature.
- **Responsive Design**: Enjoy a mobile-friendly interface.
- **Report function**: You can report posts that you think are inaproparate or should be deleted.

## Getting Started

### Prerequisites

- .NET Framework
- SQL Server

### Installation

1. Clone the repository: https://github.com/Mitko-M/Blog-App.git
2. Open the solution in Visual Studio.
3. Restore the NuGet packages.
4. Update the connection string in `appsettings.json` to match your SQL server.
5. Run the application.

## Usage

After logging in, you can start creating your blog posts using the intuitive editor provided. You can categorize your posts, manage comments, and more.

## Built With

- [ASP.NET MVC](https://dotnet.microsoft.com/apps/aspnet/mvc) - The web framework used
- [Entity Framework](https://docs.microsoft.com/en-us/ef/) - Object-relational mapping framework
- [SQL Server](https://www.microsoft.com/en-us/sql-server) - Database system
- [Enums.NET](https://github.com/TylerBrinkley/Enums.NET) - Library for robust enum handling in .NET
- [NUnit](https://nunit.org/) - Framework used for unit testing
- [Moq](https://github.com/moq/moq4) - Mocking framework for .NET
- [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer) - To sanitize HTML to prevent XSS attacks

## Testing

This application includes a suite of unit tests using NUnit and Moq frameworks to ensure functionality works as expected and to mock the database context for testing. HtmlSanitizer is used to clean user input and prevent XSS attacks. To run the tests:

1. Navigate to the test project directory.
2. Use the following command: ```dotnet test``` or click the button in the test explorer to run all tests

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the [LICENSE.txt](https://github.com/Mitko-M/Blog-App/blob/master/LICENSE.txt) file for details.

## Acknowledgments

- Hat tip to anyone whose code was used
- Inspiration
- etc

**Feel free to message me via social media**
