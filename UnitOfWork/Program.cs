using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/weatherforecast", ([FromServices] IUnitOfWork unitOfWork) =>
    {
        var b = unitOfWork.ProductRepository;
        unitOfWork.SaveChanges();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/weatherforecast2", ([FromServices] IUnitOfWork unitOfWork) =>
    {
        var b = unitOfWork.CustomerRepository;
        unitOfWork.SaveChanges();
    })
    .WithName("GetWeatherForecast2")
    .WithOpenApi();

app.MapGet("/weatherforecast3", ([FromServices] IUnitOfWork unitOfWork) =>
    {
        var b = unitOfWork.ProductRepository;
        var c = unitOfWork.CustomerRepository;
        unitOfWork.SaveChanges();
    })
    .WithName("GetWeatherForecast33")
    .WithOpenApi();


app.Run();


public class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
{
    private IProductRepository? _productRepository;
    private ICustomerRepository? _customerRepository;

    public IProductRepository ProductRepository
    {
        get
        {
            if (_productRepository == null)
            {
                _productRepository = serviceProvider.GetRequiredService<IProductRepository>();
            }

            return _productRepository;
        }
    }

    public ICustomerRepository CustomerRepository
    {
        get
        {
            if (_customerRepository == null)
            {
                _customerRepository = serviceProvider.GetRequiredService<ICustomerRepository>();
            }

            return _customerRepository;
        }
    }

    public void SaveChanges()
    {
        Console.WriteLine("Saving changes");
    }
}

public interface IProductRepository
{
    void Hello();
}

public class ProductRepository : IProductRepository
{
    public ProductRepository()
    {
        Console.WriteLine("Product repository created");
    }

    public void Hello()
    {
        Console.WriteLine("Hello from product repository");
    }
}

public interface ICustomerRepository
{
    void Hello();
}

public class CustomerRepository : ICustomerRepository
{
    public CustomerRepository()
    {
        Console.WriteLine("Customer repository created");
    }

    public void Hello()
    {
        Console.WriteLine("Hello from customer repository");
    }
}

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    void SaveChanges();
}