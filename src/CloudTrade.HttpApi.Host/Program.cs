using Autofac;
using Autofac.Extensions.DependencyInjection;
using CloudTrade.SqlSugarCore;
using SqlSugar;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args).Inject();

// Add services to the container.

builder.Services.AddControllers().AddInject();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//autofac “¿¿µ◊¢»Î≥Ã–ÚºØ

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(x =>
{
    x.RegisterModule(new AutofacModuleRegister());
});
builder.Services.AddTransient<IBaseService, BaseService>();
builder.Services.AddSingleton<ISqlSugarClient>(x => {
    SqlSugarScope client = new SqlSugarScope(new ConnectionConfig()
    {
        DbType = SqlSugar.DbType.SqlServer,
        ConnectionString = "Server=154.201.64.195;Database=CloudTrade;uid=sa;pwd=wuyule123.;TrustServerCertificate=True",
        IsAutoCloseConnection = true,
        InitKeyType = InitKeyType.Attribute,
    }, db => {
        db.Aop.OnLogExecuting = (sql, pars) =>
        {

        };
    });
    return client;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseInject();
app.MapControllers();

app.Run();


public class AutofacModuleRegister : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        Assembly interfaceAssembly = Assembly.Load("CloudTrade.Application");
        Assembly serviceAssembly = Assembly.Load("CloudTrade.Application.Contracts");

        builder.RegisterAssemblyTypes(interfaceAssembly, serviceAssembly).AsImplementedInterfaces();
    }
}
