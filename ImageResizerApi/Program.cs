
using ImageResizerApi.Data;
using ImageResizerApi.Service;
using Microsoft.EntityFrameworkCore;

namespace ImageResizerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Context>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {

                        //you can configure your custom policy
                        builder.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                try
                {
                    var ctx = app.Services.CreateScope().ServiceProvider.GetService<Context>();
                    ctx.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });

            app.MapControllers();

            app.Run();
        }
    }
}