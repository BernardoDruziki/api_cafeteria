using cafeteria.Controllers;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = "_corsPolicy";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name:corsPolicy,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5142");                                            
                      });
});  

var app = builder.Build();
app.UseRouting();

//Clients
app.MapPost("/registerClient", clientController.registerClient);
app.MapDelete("/deleteClient/{Id}", clientController.deleteClient);
app.MapPut("/editClient/{Id}", clientController.editClient);
app.MapGet("/getClient/{clientId}", clientController.getClient);

app.UseSwagger();
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1.0.0");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
