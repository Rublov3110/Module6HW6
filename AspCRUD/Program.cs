using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AspCRUD.Services;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiResources(Configurations.Apis)
    .AddInMemoryClients(Configurations.Clients)
    .AddInMemoryIdentityResources(Configurations.IdentityResources())
    .AddResourceOwnerValidator<UserPasswordValidator>();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        builder => builder.AllowAnyOrigin())
           );
builder.Services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.Authority = "http://localhost:56001"; // Your Identity Server URL
                    options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false
                   };
               });

var app = builder.Build();

app.UseIdentityServer();
app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
