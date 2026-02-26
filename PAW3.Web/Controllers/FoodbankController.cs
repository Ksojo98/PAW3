using Microsoft.AspNetCore.Mvc;
using PAW3.Architecture;
using PAW3.Architecture.Providers;
using PAW3.Core.Services;
using PAW3.Data.Foodbankdb.Models;
using PAW3.Models.DTO;
using PAW3.Web.Filters;
using PAW3.Web.Models.ViewModels;

namespace PAW3.Web.Controllers;

[RequireLogin]
public class FoodbankController : Controller
{
    private readonly IRestProvider _restProvider;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public FoodbankController(IRestProvider restProvider, IConfiguration configuration)
    {
        _restProvider = restProvider;
        _configuration = configuration;
        _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7180/api";
    }

    public async Task<IActionResult> Index([FromServices] IEntityOperationService entityOperationService)
    {
        try
        {
            var endpoint = $"{_apiBaseUrl}/FoodItemApi";
            var response = await _restProvider.GetAsync(endpoint, null);
            var foodItems = JsonProvider.DeserializeSimple<List<FoodItem>>(response);
            var vm = new FoodBankViewModel
            {
                foodItems =  foodItems;
            };
            //entityOperationService.SumEach([.. foodItems.Products.Select(x => x.Rating ?? 0.0M)], 0.5M);
            return View(vm);
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading products: {ex.Message}";
            return View(new ProductDtoViewModel());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading the food items: {ex.Message}";
            return NotFound();
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FoodBankViewModel product)
    {
        try
        {
            if (ModelState.IsValid)
            {
               
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error creating the food items: {ex.Message}");
        }
        return View(product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading the food items: {ex.Message}";
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FoodBankViewModel product)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error updating product: {ex.Message}");
        }
        return View(product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading the food items: {ex.Message}";
            return NotFound();
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error deleting the food items: {ex.Message}";
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}

