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

## Steps

2. **Backend Setup:**

   - **Navigate to the backend directory:**

     ```bash
     cd backend
     ```

   - **Install the required dependencies:**

     Ensure you have the .NET Core SDK installed, then run:

     ```bash
     dotnet restore
     ```

   - **Run the project:**

     Start the backend server using the following command:

     ```bash
     dotnet run
     ```

   - **Verify the backend:**

     The server will be running at `http://localhost:5000` by default. You can test the API using tools like [Postman](https://www.postman.com/) or `curl` to ensure it's working correctly.

3. **Frontend Setup:**

   - **Navigate to the frontend directory:**

     ```bash
     cd ../frontend
     ```

   - **Install the dependencies:**

     Make sure you have Node.js and npm installed, then run:

     ```bash
     npm install
     ```

   - **Start the Angular application:**

     Launch the frontend development server using:

     ```bash
     ng serve
     ```

   - **Verify the frontend:**

     Open your browser and go to `http://localhost:4200` to see the application running.

4. **Database Configuration:**

   - **Set up your SQL Server:**

     Ensure that your SQL Server instance is running and accessible.

   - **Configure the connection string:**

     Update the `appsettings.json` file in the backend project with your SQL Server connection details. Here is an example of what the configuration might look like:

     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ChatAppDB;User Id=YOUR_USER_ID;Password=YOUR_PASSWORD;"
       }
     }
     ```

   - **Apply database migrations:**

     Run the following command to ensure your database schema is up to date:

     ```bash
     dotnet ef database update
     ```

## Usage

1. **Access the application:**

   - Open your web browser and navigate to `http://localhost:4200`.

2. **Register or log in:**

   - Use the registration page to create a new account or log in with existing credentials.

3. **Start messaging:**

   - Once logged in, you can start sending messages and sharing photos with other users.

## Contact

- **Proje Owner:** [Mehmet Özdemir]
- **Email:** [mehmetozdmirr@icloud.com]
- **LinkedIn:** [https://www.linkedin.com/in/mehmet-%C3%B6zdemir-685340299/]

These contact details will help users and potential contributors get in touch with you for more information. Feel free to add more social media or contact channels as needed!
