using WheelsRental.Data;
using WheelsRental.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Pasta onde os arquivos CSV são gravados (persistência do projeto).
var pastaDados = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "csv");

// Fonte de dados (CSV) e serviços de negócio registrados na injeção de dependência.
builder.Services.AddSingleton(new WheelsContext(pastaDados));
builder.Services.AddScoped<ServicoAluguel>();
builder.Services.AddScoped<ServicoReserva>();
builder.Services.AddScoped<ServicoManutencao>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
