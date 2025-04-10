using GoogleAppearances.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IGoogleSearchScraperService, GoogleSearchScraperService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = false // Disable cookies - otherwise google will return accept cookies landing page
    });

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

