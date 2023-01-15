using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt => {
    opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

string conn = builder.Configuration.GetConnectionString("devDB") ?? "NaN";
builder.Services.AddDbContext<NotesAPI.Database.DatabaseContext>(opt => { opt.UseSqlServer(conn);  });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
