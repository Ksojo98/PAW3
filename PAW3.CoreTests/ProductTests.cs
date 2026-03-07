using Moq;
using PAW3.Core.BusinessLogic;
using PAW3.Core.Domain;
using PAW3.Data.Repositories;
using PAW3.Models.Entities.Productdb;
using Task = System.Threading.Tasks.Task;

namespace PAW3.CoreTests;

public class ProductTests
{
    private readonly Mock<IRepositoryProduct> _repositoryProductMock;
    private readonly IProductBusiness _business;

    public ProductTests()
    {
        _repositoryProductMock = new Mock<IRepositoryProduct>();
        _business = new ProductBusiness(_repositoryProductMock.Object);
    }

    [Fact]
    public void ApplyBusinessRules_Should_Assign_Default_Rating_When_Null()
    {
        var products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "A", Rating = null, LastModified = DateTime.Now.AddDays(-10) }
        };

        var domain = new ProductDomain();

        var result = domain.ApplyBusinessRules(products).First();

        Assert.Equal(3, result.Rating);
    }

    [Fact]
    public void ApplyBusinessRules_Should_Assign_RatingClass_C_When_Rating_Is_3()
    {
        var products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "A", Rating = 3, LastModified = DateTime.Now.AddDays(-10) }
        };

        var domain = new ProductDomain();

        var result = domain.ApplyBusinessRules(products).First();

        Assert.Equal("C", result.RatingClass);
    }

    [Fact]
    public void ApplyBusinessRules_Should_Assign_TimeClass_A_When_Time_Is_10()
    {
        var products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "A", Rating = 4, LastModified = DateTime.Now.AddDays(-10) }
        };

        var domain = new ProductDomain();

        var result = domain.ApplyBusinessRules(products).First();

        Assert.Equal("A", result.TimeClass);
    }

    [Fact]
    public async Task GetProducts_WhenIdIsNull_Should_Group_By_RatingClass_And_TimeClass()
    {
        var products = new List<Product>
    {
        new Product { ProductId = 1, ProductName = "A", Rating = null, LastModified = DateTime.Now.AddDays(-10) },
        new Product { ProductId = 2, ProductName = "B", Rating = 4.7m, LastModified = DateTime.Now.AddDays(-20) },
        new Product { ProductId = 3, ProductName = "C", Rating = 4.8m, LastModified = DateTime.Now.AddDays(-30) }
    };

        _repositoryProductMock
            .Setup(rp => rp.ReadAsync())
            .ReturnsAsync(products);

        var result = await _business.GetProducts(null);

        Assert.NotNull(result);
        Assert.Equal(3, result.Products.Count());
        Assert.NotEmpty(result.Summaries);
        Assert.Contains(result.Summaries, x => x.RatingClass == "A" && x.TimeClass == "B");
        Assert.Contains(result.Summaries, x => x.RatingClass == "C" && x.TimeClass == "A");
    }
    [Theory]
    [InlineData(1.5, "D")]
    [InlineData(2.0, "C")]
    [InlineData(3.0, "C")]
    [InlineData(4.0, "B")]
    [InlineData(4.5, "A")]
    [InlineData(5.0, "A")]
    public void ApplyBusinessRules_Should_Assign_Correct_RatingClass(decimal rating, string expectedClass)
    {
        var products = new List<Product>
    {
        new Product
        {
            ProductId = 1,
            ProductName = "Producto Test",
            Rating = rating,
            LastModified = DateTime.Now.AddDays(-10)
        }
    };

        var domain = new ProductDomain();

        var result = domain.ApplyBusinessRules(products).First();

        Assert.Equal(expectedClass, result.RatingClass);
    }
    [Theory]
    [InlineData(10, "A")]
    [InlineData(15, "A")]
    [InlineData(20, "B")]
    [InlineData(25, "B")]
    [InlineData(30, "C")]
    public void ApplyBusinessRules_Should_Assign_Correct_TimeClass(int daysAgo, string expectedClass)
    {
        var products = new List<Product>
    {
        new Product
        {
            ProductId = 1,
            ProductName = "Producto Test",
            Rating = 4,
            LastModified = DateTime.Now.AddDays(-daysAgo)
        }
    };

        var domain = new ProductDomain();

        var result = domain.ApplyBusinessRules(products).First();

        Assert.Equal(expectedClass, result.TimeClass);
    }
}