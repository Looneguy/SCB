using Microsoft.EntityFrameworkCore;
using SCB_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Database>();

var connectionString = builder.Configuration.GetConnectionString("default");

builder.Services.AddDbContext<SCBDbContext>(options => options.UseSqlServer(connectionString));
// TODO Add SeriLog as logger if i have time

var app = builder.Build();

// Configure the HTTP request pipeline.
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

    if (app.Environment.IsProduction())
    {
        await database.CreateIfNotExist();
    }

    if (app.Environment.IsDevelopment())
    {
        // TODO Fetch from API on startup, Maybe in a new method in database service?
        await database.RecreateAndSeedAsync();
    }
}

app.Run();
