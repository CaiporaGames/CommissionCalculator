
namespace FCamara.CommissionCalculator
{
    using FCamara.CommissionCalculator.Options;
    using FCamara.CommissionCalculator.Services;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // bind CommissionOptions from config and provide defaults
            builder.Services.Configure<CommissionOptions>(builder.Configuration.GetSection("Commission"));
            builder.Services.AddScoped<ICommissionCalculator, CommissionCalculator>();

            // CORS for React dev server
            const string FrontendCors = "FrontendCors";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(FrontendCors, p =>
                    p.WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseRouting();
            app.UseCors(FrontendCors);

            app.MapControllers();

            app.Run();
        }
    }
}
