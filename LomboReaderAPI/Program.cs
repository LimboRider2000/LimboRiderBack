using DevOpseTest.Services.Hash;
using DevOpseTest.Services.KDF;

using LimboReaderAPI.Data;

using LomboReaderAPI.Services.Mail;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.FileProviders;

using MySqlConnector;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Настройка подключения к Базе Данных
String? connectionString = builder.Configuration.GetConnectionString("PlanetScale");
MySqlConnection connection = new(connectionString);

builder.Services.AddDbContext<DataContext>(options =>
 options.UseMySql(
    connection,
    ServerVersion.AutoDetect(connection),
    serverOptions => serverOptions
    .MigrationsHistoryTable(
        tableName: HistoryRepository.DefaultTableName,
        schema: "LimboReaderDB"
        ).SchemaBehavior(
            MySqlSchemaBehavior.Translate,
            (schema, table) => $"{schema}_{table}"
        )));

builder.Services.AddSingleton<IHashService, Sha1HeshService>();
builder.Services.AddSingleton<IKDFService, HashBasedKdfService>();
builder.Services.AddSingleton<IMailService, MailService>();



builder.Services.AddCors();
builder.Services.AddSwaggerGen();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("http://localhost:4200/")
                                          .AllowAnyMethod()
                                          .AllowAnyHeader()
                                          .AllowAnyOrigin());


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});
app.UseAuthorization();

app.MapControllers();

app.Run();
