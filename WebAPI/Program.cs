using Application.Contract;
using Application.Helper;
using Infrastructure.Data;
using Infrastructure.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<ITransaction, TransactionRepository>();
builder.Services.AddScoped<HandleExceptionHelper>();

builder.Services.AddDbContext<FinanceAppDbContext>(options => options.UseSqlServer("Server=localhost; Database=financebd;user=DESKTOP-B741IOH\\Sheila; Trusted_Connection=true; Trust Server Certificate = true"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policy =>
    {
        policy.WithOrigins("https://localhost:7074")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithHeaders(HeaderNames.ContentType);
    }); 
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
