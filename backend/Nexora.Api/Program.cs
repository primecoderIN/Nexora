using Nexora.Api.Extensions;
using Nexora.Modules.Identity.API.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// This MUST come after UseAuthentication. It extracts the 'sub' claim from the JWT
// and queries the DB to find the user's TenantId, caching it for EF Core global query filters.
app.UseTenantResolutionMiddleware();

app.MapControllers();

app.Run();
