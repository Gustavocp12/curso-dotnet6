using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);


var app = builder.Build();

app.MapGet("/", () => "Hello World! 3");
app.MapGet("/user", () => "Gustavo Costa");
app.MapPost("/", () => new{Name= "Gustavo", Age="19"});

//Enviar pelo body
app.MapPost("/products", (ProductRequest request, ApplicationDbContext context) => {
    var category = context.Categories.Where(c => c.Id == request.categoriaId).First();
    var product = new Product{
        Code = request.Code,
        Name = request.Name,
        Descricao = request.Descricao,
        categoria = category

    };
    if(request.tags != null){

        product.tags = new List<Tags>();
        foreach (var item in request.tags)
        {
            product.tags.Add(new Tags {Name = item});
        }

    }
    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created($"/products/{product.Id}", product.Id);
});

//Enviar pela query
app.MapGet("/products", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {

    return dateStart + "-" + dateEnd;

});

//Enviar pela rota
app.MapGet("/products/{Id}", ([FromRoute] int id, ApplicationDbContext context) => {
    var product = context.Products
    .Include(p => p.categoria)
    .Include(p => p.tags)
    .Where(p => p.Id == id).First();
    if(product != null){
        return Results.Ok(product);
    }else{
        return Results.NotFound();
    }
});

//Editar
app.MapPut("/products/{Id}", ([FromRoute] int id, ProductRequest request, ApplicationDbContext context) => {
     var product = context.Products
    .Include(p => p.tags)
    .Where(p => p.Id == id).First();
    var category = context.Categories.Where(c => c.Id == request.categoriaId).First();

    product.Code = request.Code;
    product.Descricao = request.Descricao;
    product.Name = request.Name;
    product.categoria = category;
    if(request.tags != null){

        product.tags = new List<Tags>();
        foreach (var item in request.tags)
        {
            product.tags.Add(new Tags {Name = item});
        }

    }
    context.SaveChanges();
    return Results.Ok();
});

app.MapDelete("/products/{code}", ([FromRoute] int id, ApplicationDbContext context) => {
    var product = context.Products.Where(p => p.Id == id).First();
    context.Products.Remove(product);
    context.SaveChanges();
    return Results.Ok();
});

//Enviar pelo request
app.MapGet("/products", (HttpRequest request) => {

    return request.Headers["Codigo"].ToString();

});

app.MapGet("/configuration/database", (IConfiguration configuration) => {
    return Results.Ok($"{configuration ["database:connection"]}/{configuration ["database:port"]}");
});

app.Run();
