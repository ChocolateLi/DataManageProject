using api.Data;
using api.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("/test", () =>
{
    return "ok";
});

app.MapGet("/codefirst", () =>
{
    return SqlSugarHelper.InitDateBase();
});
app.MapPost("/list", (Model req) =>
{
    return SqlSugarHelper.GetUsers(req);
});
app.MapPost("/add", (AddReq req) =>
{
    return SqlSugarHelper.Add(req);
});
app.MapPost("/edit", (User req) =>
{
    return SqlSugarHelper.Edit(req);
});
app.MapGet("/del", (string ids) =>
{
    return SqlSugarHelper.Del(ids);
});


app.Run();
 