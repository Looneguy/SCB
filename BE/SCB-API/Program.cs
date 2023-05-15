using Microsoft.EntityFrameworkCore;
using SCB_API.Services;

var allowAnyOrigin = "_allowAnyOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DatabaseHandler>();
builder.Services.AddScoped<SCBHandler>();

builder.Services.AddHttpClient("SCB", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.scb.se/OV0104/v1/doris/en/ssd/");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy
    (
        name: allowAnyOrigin,
        builder =>
        {
            builder.AllowAnyOrigin();
        }
    );
});

var connectionString = builder.Configuration.GetConnectionString("default");

builder.Services.AddDbContext<SCBDbContext>(options => options.UseSqlServer(connectionString));
// TODO Add SeriLog as logger if i have time

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(allowAnyOrigin);
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<DatabaseHandler>();
    var scbHandler = scope.ServiceProvider.GetRequiredService<SCBHandler>();

    if (app.Environment.IsProduction())
    {
        await database.CreateIfNotExist();
    }

    if (app.Environment.IsDevelopment())
    {
        await database.Recreate();

        var scbContent = await scbHandler.GetSCBRegionAndGenderTemplate();
        await database.FillDbWithRegionAndGenderTemplateAsync(scbContent);

        // TODO Exctract which years that can be used in API from SCB and put them in an constant instead 
        // of hardcoding them here
        string[] filterYears = { "2016","2017","2018","2019","2020"};
        var bornStatistics = await scbHandler.GetBornStatisticsFromSCBAsync(filterYears);

        if (bornStatistics.Success && bornStatistics.Value != null)
        {
            await database.FillDbWithBornStatistics(bornStatistics.Value);
        }
        else
        {
            // TODO - Log error -> retry
            Console.WriteLine(bornStatistics.ErrorMessage);
        }
    }
}

app.Run();
