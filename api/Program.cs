
namespace FCamara.CommissionCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS for React dev server
            const string FrontendCors = "FrontendCors";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(FrontendCors, policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "https://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(FrontendCors);
            app.MapControllers();

            app.Run();
        }
    }
}
