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


app.MapGet("/product", ([FromServices] IUnitOfWork unitOfWork) =>
    {
        using (unitOfWork)
        {
            unitOfWork.GetRepository<IProductRepository>().Hello();
            unitOfWork.SaveChanges();
        }
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/category", ([FromServices] IUnitOfWork unitOfWork) =>
    {
        using (unitOfWork)
        {
            unitOfWork.GetRepository<ICustomerRepository>().Hello();
            unitOfWork.SaveChanges();
        }
    })
    .WithName("GetWeatherForecast2")
    .WithOpenApi();

app.MapGet("/allOf", ([FromServices] IUnitOfWork unitOfWork) =>
    {
        using (unitOfWork)
        {
            unitOfWork.GetRepository<IProductRepository>().Hello();
            unitOfWork.GetRepository<ICustomerRepository>().Hello();

            unitOfWork.SaveChanges();
        }
    })
    .WithName("GetWeatherForecast33")
    .WithOpenApi();


app.Run();


public class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
{
    public void SaveChanges()
    {
        Console.WriteLine("Saving changes");
    }

    public TRepository GetRepository<TRepository>() where TRepository : IRepository
    {
        return serviceProvider.GetRequiredService<TRepository>();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public interface IRepository
{
}

public interface IProductRepository : IRepository
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


public interface ICustomerRepository : IRepository
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

public interface IUnitOfWork : IDisposable
{
    public TRepository GetRepository<TRepository>() where TRepository : IRepository;
    void SaveChanges();
}