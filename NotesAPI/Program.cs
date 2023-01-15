using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt => {
    opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

string conn = builder.Configuration.GetConnectionString("devDB") ?? "NaN";
builder.Services.AddDbContext<NotesAPI.Database.DatabaseContext>(opt => { opt.UseSqlServer(conn);  });

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
