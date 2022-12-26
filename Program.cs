using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme) //configura o serviço para adicionar a autenticação baseada em cookie. Essa autenticação é usada em cenários do navegador e para definir o desafio para OpenID Connect.
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))//adiciona a autenticação de plataforma de identidade da Microsoft ao seu aplicativo. O aplicativo é configurado para conectar usuários com base nas seguintes informações na seção AzureAD do arquivo de configuração
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)//Adicionar suporte para o aplicativo da Web adquirir tokens para chamar uma API
    .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))//permite que os controladores ou o Razor Pages beneficiem-se diretamente do GraphServiceClient (pela injeção de dependência)
    .AddInMemoryTokenCaches();//permitem que o seu aplicativo se beneficie de um cache de token.

// Add services to the container.
builder.Services.AddControllersWithViews(options =>{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddMicrosoftIdentityUI();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
