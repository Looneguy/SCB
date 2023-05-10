using Microsoft.EntityFrameworkCore;
using SCB_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Database>();
builder.Services.AddScoped<SCBHandler>();
builder.Services.AddHttpClient("SCB", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.scb.se/OV0104/v1/doris/en/ssd/");
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
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<Database>();
    var scbHandler = scope.ServiceProvider.GetRequiredService<SCBHandler>();

    if (app.Environment.IsProduction())
    {
        await database.CreateIfNotExist();
    }

    if (app.Environment.IsDevelopment())
    {
        // TODO Fetch from API on startup, Maybe in a new method in database service?
        //await database.RecreateAndSeedAsync();
        await database.Recreate();
        var scbContent = await scbHandler.GetSCBRegionAndGenderTemplate();
        await database.FillDbWithRegionAndGenderTemplateAsync(scbContent);
    }
}

app.Run();
