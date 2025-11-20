using MicroserviceMembership.Domain.Ports;
using MicroserviceMembership.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
// Llama al m�todo de extensi�n para registrar las dependencias del m�dulo
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
var app = builder.Build();

// 2. PIPELINE HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();