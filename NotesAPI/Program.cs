using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt => {
    opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

string conn = builder.Configuration.GetConnectionString("devDB") ?? "NaN";

builder.Services.AddDbContext<NotesAPI.Database.DatabaseContext>(opt => { opt.UseSqlServer(conn); });


builder.Services.AddSession(opt => {
    var minutes = TimeSpan.FromMinutes(10);
        opt.IdleTimeout = minutes;
        opt.Cookie.MaxAge = minutes;
        opt.Cookie.HttpOnly = true;
        opt.Cookie.IsEssential = true;
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(NotesAPI.AppConfig.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
