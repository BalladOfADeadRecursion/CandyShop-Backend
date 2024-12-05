// Подключение необходимых пространств имен для работы с Entity Framework и веб-приложением
using Microsoft.EntityFrameworkCore; // Для работы с Entity Framework Core
using WebApi; // Для доступа к классу ApplicationDbContext (контекст базы данных)

var builder = WebApplication.CreateBuilder(args); // Создание билдер объекта для веб-приложения

// Добавление сервисов в контейнер зависимостей

// Конфигурирование контекста базы данных, используя Npgsql для подключения к PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Используется строка подключения с именем "DefaultConnection" из конфигурации приложения

// Конфигурирование CORS для обеспечения возможности общения между сервером и клиентом (например, с React-приложением)
builder.Services.AddCors(options =>
{
    // Добавление политики CORS с именем "AllowReactApp"
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            // Разрешение запросов с указанного источника (например, React-приложения, работающее на localhost:3000)
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader() // Разрешение любых заголовков
                   .AllowAnyMethod(); // Разрешение любых методов (GET, POST, PUT, DELETE и т.д.)
        });
});

// Добавление службы контроллеров для обработки HTTP-запросов
builder.Services.AddControllers();

// Настройка Swagger для документирования API
// Дополнительную информацию о Swagger можно найти по ссылке: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // Добавление службы для поиска и описания API конечных точек
builder.Services.AddSwaggerGen(); // Добавление Swagger для генерации спецификации API

// Создание приложения
var app = builder.Build();

// Настройка конвейера обработки HTTP-запросов

// Включение Swagger и Swagger UI только в среде разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Включение Swagger для отображения документации API
    app.UseSwaggerUI(); // Включение UI для визуализации Swagger-документации
}

// Разрешение CORS для политики "AllowReactApp", чтобы клиентское приложение могло отправлять запросы на сервер
app.UseCors("AllowReactApp");

// Включение механизма авторизации
app.UseAuthorization();

// Маршрутизация запросов к контроллерам API
app.MapControllers();

// Запуск приложения
app.Run(); // Старт веб-приложения, обработка запросов начинается с этого момента
