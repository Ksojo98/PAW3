using Microsoft.AspNetCore.Mvc;
using PAW3.Architecture;
using PAW3.Architecture.Providers;
using PAW3.Web.Filters;
using PAW3.Web.Models.ViewModels;
using System.Text.Json;

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

    [HttpGet]
    public async Task<IActionResult> Index(
     string? name,
     string? category,
     string? brand,
     string? description,
     decimal? price,
     string? unit,
     int? quantityInStock,
     DateOnly? expirationDate,
     bool? isPerishable,
     int? caloriesPerServing,
     string? ingredients,
     string? barcode,
     string? supplier,
     DateTime? dateAdded,
     bool? isActive
 )
    {
        try
        {
            var endpoint = $"{_apiBaseUrl}/FoodItemApi";
            var response = await _restProvider.GetAsync(endpoint, null);

            var items = JsonSerializer.Deserialize<List<FoodBankViewModel>>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<FoodBankViewModel>();

            var query = items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(x => x.Category.Contains(category, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(x => x.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(x => x.Description.Contains(description, StringComparison.OrdinalIgnoreCase));

            if (price.HasValue)
                query = query.Where(x => x.Price == price.Value);

            if (!string.IsNullOrWhiteSpace(unit))
                query = query.Where(x => x.Unit.Contains(unit, StringComparison.OrdinalIgnoreCase));

            if (quantityInStock.HasValue)
                query = query.Where(x => x.QuantityInStock == quantityInStock.Value);

            if (expirationDate.HasValue)
                query = query.Where(x => x.ExpirationDate.HasValue && x.ExpirationDate.Value == expirationDate.Value);

            if (isPerishable.HasValue)
                query = query.Where(x => x.IsPerishable == isPerishable.Value);

            if (caloriesPerServing.HasValue)
                query = query.Where(x => x.CaloriesPerServing == caloriesPerServing.Value);

            if (!string.IsNullOrWhiteSpace(ingredients))
                query = query.Where(x => x.Ingredients.Contains(ingredients, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(barcode))
                query = query.Where(x => x.Barcode.Contains(barcode, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(supplier))
                query = query.Where(x => x.Supplier.Contains(supplier, StringComparison.OrdinalIgnoreCase));

            if (dateAdded.HasValue)
                query = query.Where(x => x.DateAdded.Date == dateAdded.Value.Date);

            if (isActive.HasValue)
                query = query.Where(x => x.IsActive == isActive.Value);


            return View(query.ToList());
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading the food items: {ex.Message}";
            return View(new List<FoodBankViewModel>());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var endpoint = $"{_apiBaseUrl}/FoodItemApi/{id}";
            var response = await _restProvider.GetAsync(endpoint, id.ToString());

            var item = JsonSerializer.Deserialize<FoodBankViewModel>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (item == null)
                return NotFound();

            return View(item);
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading the food item: {ex.Message}";
            return NotFound();
        }
    }

    public IActionResult Create()
    {
        return View(new FoodBankViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FoodBankViewModel item)
    {
        if (!ModelState.IsValid)
            return View(item);
        try
        {
            //aca mandamos un payload para ayudar al FE con la carga de datos
            var endpoint = $"{_apiBaseUrl}/FoodItemApi";
            var apiPayload = new
            {
                name = item.Name,
                category = item.Category,
                brand = item.Brand,
                description = item.Description,
                price = item.Price,
                unit = item.Unit,
                quantityInStock = item.QuantityInStock,
                expirationDate = item.ExpirationDate?.ToString("yyyy-MM-dd"),
                isPerishable = item.IsPerishable,
                caloriesPerServing = item.CaloriesPerServing,
                ingredients = item.Ingredients,
                barcode = item.Barcode,
                supplier = item.Supplier,
                dateAdded = DateTime.UtcNow,
                isActive = item.IsActive,
                roleId = item.RoleId == 0 ? 1 : item.RoleId
            };

            var json = JsonSerializer.Serialize(apiPayload);

            await _restProvider.PostAsync(endpoint, json);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error creating the food item: {ex.Message}");
            return View(item);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var endpoint = $"{_apiBaseUrl}/FoodItemApi/{id}";
            var response = await _restProvider.GetAsync(endpoint, null);

            var item = JsonSerializer.Deserialize<FoodBankViewModel>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (item == null)
                return NotFound();

            return View(item);
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading the food item: {ex.Message}";
            return NotFound();
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(FoodBankViewModel item)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(item);

            var endpoint = $"{_apiBaseUrl}/FoodItemApi/{item.FoodItemId}";

            var json = JsonSerializer.Serialize(item, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });

            await _restProvider.PutAsync(endpoint, null, json);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error updating food item: {ex.Message}");
            return View(item);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var endpoint = $"{_apiBaseUrl}/FoodItemApi/{id}";
            var response = await _restProvider.GetAsync(endpoint, id.ToString());

            var item = JsonSerializer.Deserialize<FoodBankViewModel>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (item == null)
                return NotFound();

            return View(item);
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error loading the food item: {ex.Message}";
            return NotFound();
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var endpoint = $"{_apiBaseUrl}/FoodItemApi";
            await _restProvider.DeleteAsync(endpoint, id.ToString());
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error deleting the food item: {ex.Message}";
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}