var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controllers.
builder.Services.AddControllers();

// Register StudentRepository as a singleton.
builder.Services.AddSingleton<MyWebApi.Repositories.StudentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Map controllers to handle requests.
app.MapControllers();

app.Run();
