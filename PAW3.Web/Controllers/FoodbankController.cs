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

    public async Task<IActionResult> Index()
    {
        try
        {
            var endpoint = $"{_apiBaseUrl}/FoodItemApi";
            var response = await _restProvider.GetAsync(endpoint, null);

            ViewBag.RawJson = response;

            var items = JsonSerializer.Deserialize<List<FoodBankViewModel>>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<FoodBankViewModel>();

            return View(items);
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
        try
        {
            if (ModelState.IsValid)
            {
                var endpoint = $"{_apiBaseUrl}/FoodItemApi";
                var json = JsonSerializer.Serialize(item);
                await _restProvider.PostAsync(endpoint, json);
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error creating the food item: {ex.Message}");
        }

        return View(item);
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