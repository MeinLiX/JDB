using JDBWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DatabaseLogicService>(new DatabaseLogicService());
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WebAPI", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
