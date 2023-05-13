using ApiCutAndGoApp.Data;
using ApiCutAndGoApp.Helpers;
using ApiCutAndGoApp.Repositores;
using CutAndGo.Interfaces;
using Microsoft.EntityFrameworkCore;
using NSwag.Generation.Processors.Security;
using NSwag;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSingleton<HelperOAuthToken>();
    HelperOAuthToken helperOAuthToken = new HelperOAuthToken(builder.Configuration);

    builder.Services.AddHttpContextAccessor();

    // Añadimos las opciones de autentificación
    builder.Services.AddAuthentication(helperOAuthToken.GetAuthenticationOptions()).AddJwtBearer(helperOAuthToken.GetJwtOptions());

string connectionString = builder.Configuration.GetConnectionString("SqlHairdressersAzure");
    
    builder.Services.AddTransient<IRepositoryHairdresser, RepositoryHairdresser>();
    builder.Services.AddTransient<RepositoryHairdresser>();
    builder.Services.AddDbContext<HairdressersContext>(
            options => options.UseSqlServer(connectionString)
        );

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddOpenApiDocument(document => {
        document.Title = "API Cut&Go - Hairdressers Web App";
        document.Description = "API oficial de la aplicación web 'Cut&Go'. ©Todos los derechos reservados";
        document.Version = "v1";

        // CONFIGURAMOS LA SEGURIDAD JWT PARA SWAGGER, PERMITE AÑADIR EL TOKEN JWT A LA CABECERA.
        document.AddSecurity("JWT", Enumerable.Empty<string>(),
            new NSwag.OpenApiSecurityScheme {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Inserta tu TOKEN en el campo 'Value:' de la siguiente manera: Bearer {Token JWT}."
            }
        );
        document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
    });

    //builder.Services.AddSwaggerGen(options => {
    //    options.SwaggerDoc("v1", new OpenApiInfo {
    //        Title = "API Cut&Go - Hairdressers Web App",
    //        Description = "API oficial de la aplicación web 'Cut&Go'. ©Todos los derechos reservados",
    //        Version = "v1",
    //        Contact = new OpenApiContact() {
    //            Name = "Giovanny Andrés Cortés Hernández",
    //            Email = "giovannyandresch@gmail.com"
    //        }
    //    });
    //});

var app = builder.Build();
    app.UseOpenApi();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "API CUT&GO V1");
        options.RoutePrefix = "";
        options.DocExpansion(DocExpansion.None);
    });

    //app.UseSwagger();
    //app.UseSwaggerUI(options => {
    //    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "API CUT&GO V1");
    //    options.RoutePrefix = "";
    //});

// if (app.Environment.IsDevelopment()) {}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
