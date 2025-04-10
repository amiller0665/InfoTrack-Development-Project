# **InfoTrack Development Project**

## **Overview**
This project consists of 2 components, in the back-end is the **GoogleAppearances Scraper** and in the front-end the **infotrack-scraper-client**. 

The GoogleAppearances Scraper is a web service designed to scrape Google search results and identify the positions where a specific URL appears. This API accepts a search query and a target URL, performs a Google search, and returns a list of indices where the target URL appears in the search results.

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
git clone https://github.com/amiller0665/InfoTrack-Development-Project
cd InfoTrack-Development-Project
```

---

### **Backend Setup**

1. **Install Requirements**
   Install the required .NET SDK (7.0 or higher).

2. **Run the Project**
   You can either use Visual Studio or the CLI to run the application:
   - **Via Visual Studio:**
     Open the solution and press `F5` to start the application.
   - **Via `dotnet` CLI:**
     ```bash
     dotnet run --project GoogleAppearances
     ```

3. **Default URLs for Local Development**
   After starting the application:
   - HTTP: `http://localhost:5287`
   - HTTPS: `https://localhost:7048`

4. **Edit Your Environment**
   - Ensure the CORS policy in `Program.cs` matches the domain of any front-end app.
     Default setup:
     ```csharp
     policy.WithOrigins("http://localhost:3000") // React development server
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
  
- **Parameters:**
  - `query` (string): The Google search query.
  - `url` (string): The target URL to search for in the results.
  
- **Response:**
  - **Status 200 OK:**
    Returns a list of indices where the target URL appears:
    ```json
    [1, 3, 8]
    ```
  - **Status 400 Bad Request:**
    Indicates that either `query` or `url` is missing or invalid:
    ```json
    {
        "statusCode": 400,
        "message": "Request parameters cannot be null or empty."
    }
    ```
  - **Status 500 Server Error:**
    Returns an error message if the scraper fails:
    ```json
    {
        "message": "An error occurred while scraping..."
    }
    ```

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
- Add a database and repository service - use this to store previous results and then collate them into graphs for a visualisation of previous searches. 