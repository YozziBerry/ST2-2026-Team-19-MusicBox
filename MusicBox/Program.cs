using Microsoft.EntityFrameworkCore;
using MusicBox.Data;
using MusicBox.Services.Repositories;
using MusicBox.Services.Recommendations;
using MusicBox.Services;
namespace MusicBox

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient<IAIService, GeminiAIService>(
    client =>
    {
        client.Timeout = TimeSpan.FromSeconds(60);
    }
);
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ISongRepository, SongRepository>();
            builder.Services.AddScoped<GenreRecommendationStrategy>();
            builder.Services.AddScoped<ArtistRecommendationStrategy>();
            builder.Services.AddDbContext<MusicBoxDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MusicBoxConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
