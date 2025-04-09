using ScrapingService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddHttpClient<IGoogleSearchScraperService, GoogleSearchScraperService>()
    .ConfigurePrimaryHttpMessageHandler(() => { 
        return new HttpClientHandler
        {
            UseCookies = false // Disable cookies
        };
    });

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

