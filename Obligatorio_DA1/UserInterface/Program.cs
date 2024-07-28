using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Services;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Radzen;
using Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Promotion>, PromotionRepository>();
builder.Services.AddScoped<IRepository<Deposit>, DepositRepository>();
builder.Services.AddScoped<IRepository<Log>, LogRepository>();
builder.Services.AddScoped<IRepository<Review>, ReviewRepository>();
builder.Services.AddScoped<IRepository<Booking>, BookingRepository>();
builder.Services.AddScoped<IRepository<Payment>, PaymentRepository>();

builder.Services.AddScoped<BookingController>();
builder.Services.AddScoped<ReviewController>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<PromotionController>();
builder.Services.AddScoped<DepositController>();
builder.Services.AddScoped<PaymentController>();

builder.Services.AddScoped<AvailabilityService>();
builder.Services.AddScoped<SessionLogic>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<StatsService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ReservationReportExporter>();

builder.Services.AddTransient<PricingService>();

builder.Services.AddDbContextFactory<SqlContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure()
    )
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();