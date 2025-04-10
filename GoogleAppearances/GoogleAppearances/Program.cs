using GoogleAppearances.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddHttpClient<IGoogleSearchScraperService, GoogleSearchScraperService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = false // Disable cookies - otherwise google will return accept cookies landing page
    });

builder.Services.AddCors(options =>  
{  
    options.AddDefaultPolicy(
        policy  =>  
        {  
            policy.WithOrigins("http://localhost:3000/")
                .AllowAnyHeader()  
                .AllowAnyMethod();
        });  
});  

builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors();

app.MapControllers();

app.Run();

