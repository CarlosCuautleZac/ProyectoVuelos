using AerolineaApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<sistem21_aerolineaContext>(optionsBuilder=>

    optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_aerolinea;user=sistem21_aerolinea;password=sistemas19_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb")),
    ServiceLifetime.Transient
    );
var app = builder.Build();

app.UseRouting();
app.UseFileServer();
app.UseEndpoints(x=>x.MapControllers());
app.Run();
