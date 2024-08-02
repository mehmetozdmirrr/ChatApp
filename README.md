# ChatApp


## Project Description

ChatApp is a modern chat application that allows users to send real-time messages, share photos, and make video calls in the future. The project is developed using .NET Core and Angular technologies and ensures security with JWT-based authentication.

## Features

- **Real-Time Messaging:** Instant message exchange using SignalR.
- **Photo Sharing:** Share image files between users.
- **Video Calls (Coming Soon):** Video conferencing between users.
- **JWT-Based Authentication:** Ensures security of user data using JSON Web Tokens.
- **User Registration and Login:** New user registration and login for existing users.

## Technologies

- **Backend:** .NET Core, SignalR, Entity Framework Core
- **Frontend:** Angular, TypeScript, HTML, CSS
- **Database:** SQL Server

## Installation

Follow the steps below to run ChatApp on your local machine.

### Requirements

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- SQL Server

### Steps

1. **Clone the Project:**
   ```bash
   git clone https://github.com/yourusername/chatapp.git
   cd chatapp

   Backend Setup:

Navigate to the backend directory:
bash
Kodu kopyala
cd backend
Install the required dependencies and run the project:
bash
Kodu kopyala
dotnet restore
dotnet run
It will run at http://localhost:5000 by default.
Frontend Setup:

Navigate to the frontend directory:
bash
Kodu kopyala
cd ../frontend
Install the dependencies:
bash
Kodu kopyala
npm install
Start the Angular application:
bash
Kodu kopyala
ng serve
The app will run at http://localhost:4200 by default.
Database Settings:

Update the SQL Server connection string in the appsettings.json file.
Usage
Open your browser and navigate to http://localhost:4200.
Create a new account or log in with an existing account.
Start messaging or send photos!
Contributing
If you would like to contribute, please follow these steps:

Fork this repository.
Create your branch (git checkout -b feature/your-feature-name).
Commit your changes (git commit -m 'Add new feature: your-feature-name').
Push to your branch (git push origin feature/your-feature-name).
Open a pull request.
License
This project is licensed under the MIT License.

Contact
Project Owner: Mehmet Özdemir
Email: mehmetozdmirr@icloud.com
