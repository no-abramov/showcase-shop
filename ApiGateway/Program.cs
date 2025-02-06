using Microsoft.AspNetCore.Diagnostics;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку Ocelot
builder.Services.AddOcelot();
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Загружаем конфигурацию Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Глобальный обработчик ошибок
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error != null)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(exceptionHandlerPathFeature.Error, "Ошибка в Ocelot API Gateway");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Произошла внутренняя ошибка сервера");
        }
    });
});

// Логирование (statusCode 400+) запросов
app.Use(async (context, next) =>
{
    await next();

    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var statusCode = context.Response.StatusCode;

    switch (statusCode)
    {
        case 400:
            logger.LogWarning($"Запрос 400 (Bad Request): {context.Request.Method} {context.Request.Path}");
            break;
        case 401:
            logger.LogWarning($"Запрос 401 (Unauthorized): {context.Request.Method} {context.Request.Path}");
            break;
        case 403:
            logger.LogWarning($"Запрос 403 (Forbidden): {context.Request.Method} {context.Request.Path}");
            break;
        case 404:
            logger.LogWarning($"Запрос 404 (Not Found): {context.Request.Method} {context.Request.Path}");
            break;
        case 500:
            logger.LogError($"Запрос 500 (Internal Server Error): {context.Request.Method} {context.Request.Path}");
            break;
        default:
            if (statusCode >= 400)
            {
                logger.LogWarning($"Ошибка {statusCode}: {context.Request.Method} {context.Request.Path}");
            }
            break;
    }

    // Обработка 502 Bad Gateway
    if (statusCode == 502)
    {
        logger.LogError($"Ошибка 502 (Bad Gateway): Сервис недоступен {context.Request.Method} {context.Request.Path}");

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"Сервис временно недоступен. Повторите попытку позже.\"}");
    }
});

// Подключаем Ocelot
await app.UseOcelot();

app.Run();