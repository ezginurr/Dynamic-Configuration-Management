using ConfigurationLibrary.Models;
using ConfigurationWebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationWebUI.Controllers
{
    public class ConfigurationController: Controller
    {
        private readonly MongoConfigService _service;

        public ConfigurationController(MongoConfigService service)
        {
            _service = service;
        }

        public async Task<IActionResult> List()
        {
            var configs = await _service.GetAllAsync();
            return View(configs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConfigurationItem item)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(item);
                TempData["SuccessMessage"] = "Configuration saved successfully!";
                return RedirectToAction(nameof(List));
            }

            return View(item);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var item = await _service.GetByIdAsync(id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConfigurationItem item)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(item);
                TempData["SuccessMessage"] = "Configuration edited successfully!";
                return RedirectToAction(nameof(List));
            }

            return View(item);
        }

    }
}
