using ApiCutAndGoApp.Data;
using ApiCutAndGoApp.Repositores;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("SqlHairdressersAzure");

    builder.Services.AddTransient<RepositoryHairdresser>();
    builder.Services.AddDbContext<HairdressersContext>(
        options => options.UseSqlServer(connectionString)
    );

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options => {
        options.SwaggerDoc("v1", new OpenApiInfo {
            Title = "API Cut&Go - Hairdressers Web App",
            Description = "API oficial de la aplicación web 'Cut&Go'. ©Todos los derechos reservados",
            Version = "v1",
            Contact = new OpenApiContact() {
                Name = "Giovanny Andrés Cortés Hernández",
                Email = "giovannyandresch@gmail.com"
            }
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "API CUT&GO V1");
        options.RoutePrefix = "";
    });

// if (app.Environment.IsDevelopment()) {}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
