using CurrencyAppUI.Repo;
using CurrencyAppUI.Repo.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICurrencyTransactionRepo, CurrencyTransactionRepo>();
builder.Services.AddScoped<IGetProfileAccountsRepo, GetProfileAccountsRepo>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient("CurrencyAppUIClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:35842");
});
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromHours(1);
    options.Cookie.Name = "UserJwt";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserLogin}/{action=Login}");

app.Run();