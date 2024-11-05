using CarBuilderAPI.Models;
using CarBuilderAPI.Models.DTO;
using Microsoft.AspNetCore.Authentication;

List<PaintColor> paints = new List<PaintColor>()
{
    new PaintColor { Id = 1, Price = 15.99m, Color = "Silver" },
    new PaintColor { Id = 2, Price = 18.99m, Color = "Midnight Blue" },
    new PaintColor { Id = 3, Price = 19.99m, Color = "Firebrick Red" },
    new PaintColor { Id = 4, Price = 16.99m, Color = "Spring Green" }
};

List<Interior> interiors = new List<Interior>()
{
    new Interior { Id = 1, Price = 1200.00m, Material = "Leather" },
    new Interior { Id = 2, Price = 800.00m, Material = "Fabric" },
    new Interior { Id = 3, Price = 1500.00m, Material = "Alcantara" },
    new Interior { Id = 4, Price = 1000.00m, Material = "Synthetic Leather" }
};

List<Technology> technologies = new List<Technology>()
{
    new Technology { Id = 1, Price = 2500.00m, Package = "Premium Audio" },
    new Technology { Id = 2, Price = 3000.00m, Package = "Advanced Safety" },
    new Technology { Id = 3, Price = 1800.00m, Package = "Navigation System" },
    new Technology { Id = 4, Price = 2200.00m, Package = "Driver Assistance" }
};

List<Wheel> wheels = new List<Wheel>()
{
    new Wheel { Id = 1, Price = 600.00m, Style = "18-inch Alloy" },
    new Wheel { Id = 2, Price = 800.00m, Style = "20-inch Chrome" },
    new Wheel { Id = 3, Price = 1000.00m, Style = "22-inch Matte Black" },
    new Wheel { Id = 4, Price = 750.00m, Style = "19-inch Sport" }
};

List<Order> orders = new List<Order>();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/orders", () =>
{
    return orders.Select(order => new OrderDTO
    {
        Id = order.Id,
        Timestamp = order.Timestamp,
        WheelId = order.WheelId,
        Wheel = order.Wheel,
        TechnologyId = order.TechnologyId,
        Technology = order.Technology,
        PaintId = order.PaintId,
        PaintColor = order.PaintColor,
        InteriorId = order.InteriorId,
        Interior = order.Interior
    });
});

app.MapGet("/wheels", () =>
{
    return wheels.Select(w => new WheelDTO
    {
        Id = w.Id,
        Price = w.Price,
        Style = w.Style
    });
});

app.MapGet("/technologies", () =>
{
    return technologies.Select(t => new TechnologyDTO
    {
        Id = t.Id,
        Price = t.Price,
        Package = t.Package
    });
});

app.MapGet("/paints", () =>
{
    return paints.Select(p => new PaintColorDTO
    {
        Id = p.Id,
        Price = p.Price,
        Color = p.Color
    });
});

app.MapGet("/interiors", () =>
{
    return interiors.Select(i => new InteriorDTO
    {
        Id = i.Id,
        Price = i.Price,
        Material = i.Material
    });
});

app.MapPost("/orders", (Order order) =>
{
    order.Id = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
    orders.Add(order);

    order.Wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    order.Technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    order.PaintColor = paints.FirstOrDefault(p => p.Id == order.PaintId);
    order.Interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
    order.Timestamp = DateTime.Now;

    return Results.Created($"/orders/{order.Id}", new OrderDTO
    {
        Id = order.Id,
        Timestamp = order.Timestamp,
        WheelId = order.WheelId,
        Wheel = order.Wheel,
        TechnologyId = order.TechnologyId,
        Technology = order.Technology,
        PaintId = order.PaintId,
        PaintColor = order.PaintColor,
        InteriorId = order.InteriorId,
        Interior = order.Interior
    });
});



app.Run();
