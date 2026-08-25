using AvenaCore.Repositories;
using AvenaPersistentSqlServer.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<IUserRepository>(_ =>
    new MSSqlUserRepository(connectionString));

builder.Services.AddScoped<IGenreRepository>(_ =>
    new MSSqlGenreRepository(connectionString));

builder.Services.AddScoped<INewsRepository>(_ =>
    new MSSqlNewsRepository(connectionString));

builder.Services.AddScoped<ICommentRepository>(_ =>
    new MSSqlCommentRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

app.Run();
