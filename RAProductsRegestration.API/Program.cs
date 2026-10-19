
using RAProductsRegestration.API.Constants;
using RAProductsRegestration.Application;
using RAProductsRegestration.Infrastructure;
using RAProductsRegestration.Infrastructure.Identity;
using Scalar.AspNetCore;

namespace RAProductsRegestration.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddApi(builder.Configuration);
            //builder.Services.AddApi();
            var app = builder.Build();

            // Apply pending migrations
            await app.Services.MigrateDatabaseAsync();
            await app.Services.SeedIdentityAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseExceptionHandler();

            app.UseStatusCodePages();
            app.UseHttpsRedirection();

            app.UseCors(
    Frontend.FrontendCorsPolicy);

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
