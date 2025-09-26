using GmailService.Classes;
using GmailService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure EmailSettings from appsettings.json
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
app.MapGet("/", (HttpContext httpContext) => httpContext.Request.Headers);
app.MapPost("/send", async (IEmailService emailService, ILogger<Program> logger, EmailRequest request, HttpContext httpContext) =>
{
    // Kiểm tra request null (Minimal APIs không có ModelState, bạn phải kiểm tra thủ công hoặc dùng FluentValidation)
    if (request == null)
    {
        logger.LogWarning("Email request is null");
        return Results.BadRequest(new { message = "Request body is required" });
    }
    if (request.Validate() is List<string> errors)
    {
        logger.LogWarning("Email request validation failed");
        return Results.BadRequest(new { message = "Invalid request", errors = errors });
    }

    var result = await emailService.SendEmailAsync(request);

    if (result)
    {
        logger.LogInformation("Email sent successfully");
        return Results.Json(new { message = "Email sent successfully" }, statusCode: 503);
    }

    logger.LogError("Failed to send email");
    return Results.BadRequest(new { message = "Failed to send email" });
});

app.MapGet("/health/{id:int}", (int id, ILogger<Program> logger) =>
{
    logger.LogWarning($"Health check requested with ID: {id}");
    return Results.Ok(new { message = $"Service is healthy. ID: {id}" });
});

app.Run();
