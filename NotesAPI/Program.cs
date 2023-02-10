using Microsoft.EntityFrameworkCore;
using NotesAPI.Database;
using NotesAPI.Models;
using OpenIddict.Validation.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt => {
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

string conn = builder.Configuration.GetConnectionString("devDB") ?? "NaN";

builder.Services.AddDbContext<DatabaseContext>(opt => { opt.UseSqlServer(conn); opt.UseOpenIddict(); });

builder.Services.AddIdentityCore<UserModel>().AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddOpenIddict()
    .AddValidation(opt => { 
        opt.UseSystemNetHttp();
        opt.UseAspNetCore();
        opt.SetIssuer("https://localhost:8000");
});

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddHttpClient();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();
