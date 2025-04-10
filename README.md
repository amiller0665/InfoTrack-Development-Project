# **InfoTrack Development Project**

## **Overview**
This project consists of 2 components, in the back-end is the **GoogleAppearances WebApi** and in the front-end the **infotrack-scraper-client**. 

The GoogleAppearances Web Api is a web service designed to scrape Google search results and identify the positions where a specific URL appears. This API accepts a search query and a target URL, performs a Google search, and returns a list of indices where the target URL appears in the search results. The project also contains a repository that has a database attached to it. I added the database so that the user does not need to search google again everytime they want to see a result. Instead the days search is stored in the database, and we check if there is already a search result for that day if the user searches again and return that instead.

The react front-end is a simple SPA that takes a google search phrase and matching url and fetches the data from the backend and then displays the results.

## **Requirements**
To run and develop this project, the following tools are required:
- **.NET 7+ or higher** (for the API backend)
- **Node.js** (for the React front-end)
- **Visual Studio 2022/JetBrains Rider** (or any compatible C# IDE)
- **Google search API accessibility** (via HTTP scraping)

## Getting Started

### **Clone the Repository**
Clone the repository to get started:
```bash
git clone https://github.com/amiller0665/InfoTrack-Development-Project.git
cd InfoTrack-Development-Project
```

---

### **Backend Setup**

1. **Install Requirements**
  Install the required .NET SDK (7.0 or higher). To Set-up the database, we need to create it via EntityFrameworkCore. Please ensure that you have dotnet tools installed and dotnet-ef.

  ```bash
  dotnet tool install --global dotnet-ef
  dotnet add package Microsoft.EntityFrameworkCore.Design
  ```

  You will also need to have SQL Express locally installed. You can check that with the following command:

  ```bash
  sqllocaldb info
  ```

2. **Ensure the database is correctly created**
  Please run the following in the .\GoogleAppearances\GoogleAppearances directory:

  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

  You should be able to see the table created in your local db server: (localdb)\MSSQLLocalDB

  The default connection string is **"Server=(localdb)\\MSSQLLocalDB;Database=GoogleAppearancesDb;Trusted_Connection=True;"**

  Within the GoogleAppearances project (GoogleAppearances/FakeDataScript/CreateGoogleAppearancesData.sql) there is a SQL script which will insert fake data into the DB so that the graph function has data to display. Please Run that on the table created. 

3. **Run the Project**
   You can either use Visual Studio or the CLI to run the application:
   - **Via Visual Studio:**
     Open the solution and press `F5` to start the application.
   - **Via `dotnet` CLI:**
     ```bash
     dotnet run --project GoogleAppearances
     ```

4. **Default URLs for Local Development**
   After starting the application:
   - HTTP: `http://localhost:5287`
   - HTTPS: `https://localhost:7048`

   **Please ensure you start the https://localhost:7048 port as that is what the react app points to**

5. **Edit Your Environment**
   - Ensure the CORS policy in `Program.cs` matches the domain of any front-end app.
     Default setup:
     ```csharp
     policy.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
     ```
---

### **Front-End (React) Setup**
To start the React page, please navigate to `.\infotrack-scraper-client\` and run the following commands:

1. Install dependencies:
    ```bash
    npm install
    ```
2. Start the development server:
    ```bash
    npm start
    ```
---

## **API Endpoints**

#### **GET** `/api/GoogleAppearances/GetCurrentAppearances/{query}/{url}`
- **Description:** Scrapes Google search results for the specified query and identifies appearances of the target URL.

#### **GET** `/api/SearchResults/Query/{searchQuery}`
- **Description:** Retrieves all search results for the specified search query from the database.

#### **GET** `/api/SearchResults/Url/{url}`
- **Description:** Retrieves all search results for the specified URL from the database.

#### **GET** `/api/SearchResults/QueryUrl/{searchQuery}/{url}`
- **Description:** Retrieves all search results for the specified search query and URL combination from the database.

#### **GET** `/api/SearchResults/AfterDate?searchDate={searchDate}`
- **Description:** Retrieves all search results that were created after the specified date.

#### **GET** `/api/SearchResults/ByQueryUrlDate?searchDate={searchDate}&searchQuery={searchQuery}&url={url}`
- **Description:** Retrieves all search results for the specified search query, URL, and date combination.

---

## **CORS Policy**
The API is pre-configured with a CORS policy for cross-origin access, primarily to support the React front-end (`http://localhost:3000`).

If you want to consume the endpoint yourself, update the `WithOrigins` line in `Program.cs`.

Example:
```csharp
policy.WithOrigins("https://yourfrontenddomain.com")
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
```

---

## **Future Improvements**
- Add more endpoints that scrape other search engines. Add a dropdown option to the react component that lets you specify which search engine to use.
- Utilise the data stored in more ways, to give CEO more useful information. 