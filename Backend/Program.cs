
using Backend.Services;
using JsonHelperLibrary;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<JsonHelper>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var dataFolder = config["DataFolder"] ?? "Data";
                return new JsonHelper(dataFolder);
            });

            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<ScheduleService>();

            builder.Services.AddSingleton<GeminiService>();
            builder.Services.AddSingleton<TranscriptService>();
    
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
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
