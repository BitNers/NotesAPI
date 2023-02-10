using Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    { 
        options.UseSqlServer(connectionString);
        options.UseOpenIddict();
    }) ;
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddOpenIddict()
    .AddCore(
    options => {
        options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
    })
    .AddServer( options => {
        options.SetTokenEndpointUris("connect/token");
        options.SetAuthorizationEndpointUris("connect/authorize");
        options.SetIntrospectionEndpointUris("introspect");

        
        options.AllowPasswordFlow();
        options.AllowClientCredentialsFlow();

        options.AddDevelopmentEncryptionCertificate()
        .AddDevelopmentSigningCertificate()
        .DisableAccessTokenEncryption();
       

        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough();
    }
    
    )
    .AddValidation( options => {
        options.UseLocalServer();
        options.UseAspNetCore();
    }
    );

builder.Services.AddHostedService<Identity.Worker>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedEmail = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
