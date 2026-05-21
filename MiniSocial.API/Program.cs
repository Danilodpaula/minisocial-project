// using Microsoft.EntityFrameworkCore;
// using MiniSocialApi.Application.Services;
// using MiniSocialApi.Infrastructure.Data;
// using MiniSocialApi.Infrastructure.Repositories;
//
// var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//                           ?? throw new InvalidOperationException("Connection string not found.");
//
// builder.Services.AddDbContext<MiniSocialDbContext>(options =>
// {
//     options.UseNpgsql(connectionString);
// });
//
// builder.Services.AddScoped<ISocialRepository, SocialRepository>();
// builder.Services.AddScoped<SocialService>();
// builder.Services.AddScoped<FeedAdoRepository>();
//
// var app = builder.Build();
//
// using (IServiceScope scope = app.Services.CreateScope())
// {
//     MiniSocialDbContext context = scope.ServiceProvider.GetRequiredService<MiniSocialDbContext>();
//     context.Database.EnsureCreated();
// }
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseAuthorization();
//
// app.MapControllers();
//
// app.Run();