using EFAereoNuvem;
using EFAereoNuvem.Data;
using EFAereoNuvem.Repository;
using EFAereoNuvem.Repository.Interface;
using EFAereoNuvem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar autenticação por COOKIE
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Login/Index";               
        options.LogoutPath = "/Login/Logout";             
        options.AccessDeniedPath = "/Home/AccessDenied";  
        options.ExpireTimeSpan = TimeSpan.FromHours(8);   
        options.SlidingExpiration = true;                 
        options.Cookie.HttpOnly = true;                   
        options.Cookie.IsEssential = true;                
        options.Cookie.Name = "AereoNuvem.Auth";          
    });

// ==========================================================
// Sessão e HttpContext
// ==========================================================
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ==========================================================
// MVC e Banco de Dados
// ==========================================================
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================================
// Repositórios e Serviços
// ==========================================================
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IAirplaneRepository, AirplaneRepository>();
builder.Services.AddScoped<IScaleRepository, ScaleRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();

builder.Services.AddHttpClient<ApiClient>();

// Swagger apenas em dev
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================================
// Pipeline
// ==========================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sessão e autenticação
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ==========================================================
// Rotas padrão
// ==========================================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
