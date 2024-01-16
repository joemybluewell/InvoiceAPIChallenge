using InvoiceAPIChallenge.Controllers;
using InvoiceAPIChallenge.Services;

namespace InvoiceAPIChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind the External section of appsettings.json to ExternalSettings class
            builder.Services.Configure<ExternalSettings>(builder.Configuration.GetSection("External"));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register HttpClient for dependency injection
            builder.Services.AddHttpClient();

            // Register ExchangeRateService
            builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
