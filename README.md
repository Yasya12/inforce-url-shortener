# URL Shortener Application

This project is a URL Shortener application built with ASP.NET Core MVC(backend) and Angular (frontend). It allows users to shorten URLs, manage them, and navigate using shortened links.

---

## Features

- **Login View:** Allows users to login. Supports Admin and regular users.
- **Short URLs Table View:** Displays all URLs with their shortened equivalents. Authorized users can add, view details, and delete URLs.
  - Users can only delete URLs they created.
  - Admin users can delete any URL.
  - Anonymous users can only view the list.
- **Short URL Info View:** Shows detailed information about a shortened URL. Only accessible to authorized users.
- **About View:** Describes the URL shortening algorithm. Visible to everyone but editable only by Admin users.

---

## Technology Stack

- ASP.NET Core MVC (Backend)
- Angular (Latest version, Frontend)
- Entity Framework Core (Code First)
- JWT Authentication
- xUnit and Moq (Unit Testing)

---

## Running the Application

To run the backend API:

1. Open a terminal or command prompt.
2. Navigate to the backend project directory: ..\inforce-url-shortener\backend\backend
3. Run the application: dotnet run

Alternatively, you can run the backend using your preferred IDE (e.g., Visual Studio or VS Code).

---

## Notes

- The database will be automatically created and migrated on startup.
- The frontend Angular app communicates with the backend API.
- Make sure to configure the correct API URL in the Angular environment files depending on your setup.

---

## URL Shortening Algorithm

The URL shortening logic converts the numeric ID of a URL entry into a short alphanumeric code using a custom base62 encoding consisting of `a-z`, `A-Z`, and `0-9`. This ensures short, unique codes for each URL.

---

## Author
Laiter Yaryna
