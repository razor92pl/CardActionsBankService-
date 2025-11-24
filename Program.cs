using CardActionsService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // dodajemy obsługę kontrolerów
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// service DI injection (singleton for Task requirment, in the future, with database for example propably should be scoped)
builder.Services.AddSingleton<ICardService, CardService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); 

app.Run();
