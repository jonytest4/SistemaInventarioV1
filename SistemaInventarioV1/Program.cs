using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioV1.AccesoDatos.Data;
using SistemaInventarioV1.AccesoDatos.Repositorio;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.Utilidades;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//AddDefaultIdentity no permite trabajar con roles y se lo modifica por AddIdentity y se agrega ,IdentityRole
//options.SignIn.RequireConfirmedAccount = true se cambia true por false si aún no se configura la confirmación de email
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    //añadimos la toma de mensajes de error para la contraseña es decir la clase de utilidades
    .AddErrorDescriber<ErrorDescriber>()
    //añadimos el tokenProvider  que permite trabajar con el email seder
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//habilitación de servicio cookie para redireccionar si el usuario no se encuentra autorizado para acceder por su rol
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

//cambio de las reglas de la contraseña en utilidades se encuentra el manejo de mensajes
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 1;
});

//agregado de addRazorRuntimeCompilation para que el aplicativo pueda hacer visible los ajustes en tiempo real
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Servicio del repositorio gen�rico UnidadTrabajo accesible en cualquier momento o controlador
//AddScoped permite que la instancia de servicio se cree una vez y pueda seguirse usando las veces que sea necesario
builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

//Una vez configurado el servicio de Identity, se muetra un error para ejecutar páginas razor, agregamos este servicio para evitar el error
builder.Services.AddRazorPages();

//Agregamos el servicio de EmailSender para evitar el error de Email el archivo se encuentra en Utilidades
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Líneas de autorización y autenticación de las páginas
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //agregamos la referencia del area que queremos que acceda al ejecutar el programa
    pattern: "{area=Inventario}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
