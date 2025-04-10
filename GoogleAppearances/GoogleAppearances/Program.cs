using GoogleAppearances.Repository.Repositories;
using GoogleAppearances.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IGoogleSearchScraperService, GoogleSearchScraperService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = false // Disable cookies - otherwise google will return accept cookies landing page
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string"
                                                           + "'DefaultConnection' not found.");

builder.Services.AddScoped<ISearchResultRepository, SearchResultRepository>();
builder.Services.AddDbContext<SearchResultRepository>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("GoogleAppearances")));

builder.Services.AddCors(options =>  
{  
    options.AddPolicy( "ReactCorsPolicy",
        policy  =>  
        {  
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()  
                .AllowAnyMethod()
                .AllowCredentials();
        });  
});  

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("ReactCorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

