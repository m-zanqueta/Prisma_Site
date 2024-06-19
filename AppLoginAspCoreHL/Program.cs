using AppLoginAspCoreHL.Contract;
using AppLoginAspCoreHL.GerenciadorArquivos;
using AppLoginAspCoreHL.Libraries.Login;
using AppLoginAspCoreHL.Libraries.Middleware;
using AppLoginAspCoreHL.Repository;
using AppLoginAspCoreHL.Repository.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adicionar a Interface com um serviço
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IItemRepository, ItensRepository>();
builder.Services.AddScoped<IPesquisaRepository, PesquisaRepository>();



builder.Services.AddScoped<AppLoginAspCoreHL.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginCliente>();
builder.Services.AddScoped<LoginColaborador>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc().AddSessionStateTempDataProvider();


builder.Services.AddScoped<AppLoginAspCoreHL.Cookie.Cookie>();
builder.Services.AddScoped<GerenciadorArquivo>();
builder.Services.AddScoped<AppLoginAspCoreHL.CarrinhoCompra.CookieCarrinhoCompra>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseCookiePolicy();
app.UseSession();
app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();