using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test_v01.Repository;
using test_v01.Repository.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicione o serviço do contexto do banco de dados
builder.Services.AddDbContext<SITEtccDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar o Identit

// Adicione serviços MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Documentos}/{action=Index}/{id?}");


app.Run();
