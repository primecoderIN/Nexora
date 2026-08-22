using Nexora.Api.Extensions;
using Nexora.Api.Middleware;
using Nexora.Modules.Identity.API.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerServices();

var app = builder.Build();

// Enable Global Exception Handling as the first middleware
app.UseExceptionHandler();

// Ensure Correlation ID is generated and injected into logs as early as possible
app.UseCorrelationId();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexora API v1");
    });
}

app.UseHttpsRedirection();

// Use CORS before Authentication
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

// This MUST come after UseAuthentication. It extracts the 'sub' claim from the JWT
// and queries the DB to find the user's TenantId, caching it for EF Core global query filters.
app.UseTenantResolutionMiddleware();

app.MapControllers();

app.Run();
