using CustomerManagement.Core.Interfaces;
using CustomerManagement.Infrastructure.Persistence.Data;
using CustomerManagement.Infrastructure.Persistence.Repositories;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Application.Services;
using Microsoft.EntityFrameworkCore;
using CustomerManagement.Core.Entities;
using CustomerManagement.Application.Mapping;
using System.Collections.Generic;
using CustomerManagement.Common.DTOs;

var builder = WebApplication.CreateBuilder(args);

//Add DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAppraisalRepository, AppraisalRepository>();
builder.Services.AddScoped<IAppraisalService, AppraisalService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IGenericService<EmployeeDto>, GenericService<Employee, EmployeeDto>>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllersWithViews();
// For .NET 6 or later - Program.cs
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Appraisals}/{action=AppDetails}/{uId?}");


app.Run();
