using Microsoft.EntityFrameworkCore;
using GerenciadorTarefas.MVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Tenta usar MySQL; se falhar (ex.: servidor offline), registra um DB InMemory para permitir execução local sem conexão.
var usingInMemory = false;
try
{
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, serverVersion));
}
catch
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("DevDb"));
    usingInMemory = true;
}

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Conta/Login";
        options.AccessDeniedPath = "/Conta/AcessoNegado";
    });
builder.Services.AddScoped<ITarefaRepositorio, TarefaRepositorio>();

var app = builder.Build();

// Se estivermos usando InMemory, semear alguns dados de exemplo para permitir navegação nas views
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var context = provider.GetService<ApplicationDbContext>();
    if (context != null)
    {
        // Se não houver tarefas, adiciona algumas para teste
        if (!context.Tarefas.Any())
        {
            context.Tarefas.AddRange(new[] {
                new GerenciadorTarefas.MVC.Models.Tarefa { Titulo = "Tarefa de exemplo 1", Descricao = "Descrição 1", Data = System.DateTime.Today, Concluida = false },
                new GerenciadorTarefas.MVC.Models.Tarefa { Titulo = "Tarefa de exemplo 2", Descricao = "Descrição 2", Data = System.DateTime.Today.AddDays(1), Concluida = true }
            });
            context.SaveChanges();
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();