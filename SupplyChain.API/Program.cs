using Microsoft.AspNetCore.RateLimiting;
using SupplyChain.Application.Services;
using SupplyChain.Contracts.Services;
using SupplyChain.Infrastructure.Persistence;
using SupplyChain.Infrastructure.Services;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();


builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>(); 
builder.Services.AddHttpClient<IFakeStoreApiClient, FakeStoreApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["FakeStoreApi:BaseUrl"]
                                   ?? throw new InvalidOperationException("FakeStoreApi:BaseUrl ayarla"));
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rate Limit
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10; 
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2; 
    });

    // tümüne kuralý uygula
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});


builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRateLimiter();

app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers().RequireRateLimiting("fixed");

app.Run();
