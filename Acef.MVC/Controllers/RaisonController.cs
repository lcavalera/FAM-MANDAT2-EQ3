using Acef.MVC.Interfaces;
using Acef.MVC.Models.DTO;
using Acef.MVC.Models.Log;
using Microsoft.AspNetCore.Mvc;

namespace Acef.MVC.Controllers
{
    public class RaisonController : Controller
    {
        private readonly IRaisonService _raisonService;
        private readonly ILogger<RaisonController> _logger;

        public RaisonController(IRaisonService raisonService, ILogger<RaisonController> logger)
        {
            _raisonService = raisonService;
            _logger = logger;
        }

        // GET: RaisonController
        public async Task<IActionResult> Index(string? filter, string sortOrder)
        {

            var consultationReasons = await _raisonService.Get();

            // Filtering by name or description
            if (!string.IsNullOrEmpty(filter))
            {
                consultationReasons = consultationReasons.Where(
                    cr => cr.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ||
                    (cr.Description != null && cr.Description.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
                );
            }

            // Sort by name
            ViewData["SortByName"] = "";
            if (string.IsNullOrEmpty(sortOrder))
            {
                ViewData["SortByName"] = "name_desc";
            }

            consultationReasons = sortOrder == "name_desc" ?
                consultationReasons.OrderByDescending(cr => cr.Name) :
                consultationReasons.OrderBy(cr => cr.Name);

            return View(consultationReasons);
        }

        // GET: RaisonController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            RaisonDTO raison = await _raisonService.GetById(id);

            if (raison == null)
            {
                _logger.LogError($"An error occurred when retrieving a consultation reason. ID = {raison.ID}");
                return NotFound();

            }
            return View(raison);
        }

        // GET: RaisonController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: RaisonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] RaisonDTO raison)
        {
            if (ModelState.IsValid)
            {
                await _raisonService.Add(raison);
                _logger.LogInformation(RaisonLog.Creation, $"Creating a consultation reason. Nom = {raison.Name}");

                _logger.LogCritical($"The application encountered a critical problem when creating a consultation reason. Nom = {raison.Name}");

                return RedirectToAction(nameof(Index));
            }
            _logger.LogError($"An error occurred when creating a reason for consultation. Nom = {raison.Name}");
            return View(raison);
        }

        // GET: RaisonController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            RaisonDTO raison = await _raisonService.GetById(id);

            if (raison == null)
            {
                _logger.LogError($"An error occurred when retrieving a consultation reason. ID = {raison.ID}");
                return NotFound();
            }
            return View(raison);
        }

        // POST: RaisonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] RaisonDTO raison)
        {
            if (raison == null)
            {
                _logger.LogError($"An error occurred when modifying a consultation reason. ID = {raison.ID}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _raisonService.Edit(raison);
                _logger.LogInformation(RaisonLog.Modification, $"Editing an offer. ID = {raison.ID}");

                _logger.LogCritical($"The application encountered a critical problem when modifying a consultation reason. ID = {raison.ID}");

                return RedirectToAction(nameof(Index));
            }

            return View(raison);
        }

        // GET: RaisonController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            RaisonDTO raison = await _raisonService.GetById(id);

            if (raison == null)
            {
                _logger.LogError($"An error occurred when retrieving a consultation reason. ID = {raison.ID}");
                return NotFound();
            }
            return View(raison);
        }

        // POST: RaisonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, RaisonDTO raison)
        {
            raison = await _raisonService.GetById(id);
            if (raison == null)
            {
                _logger.LogError($"An error occurred when retrieving a consultation reason. ID = {raison.ID}");
                return NotFound();
            }
            await _raisonService.Delete(raison);
            _logger.LogInformation(RaisonLog.Suppression, $"Deleting a consultation reason. ID = {raison.ID}");
            _logger.LogCritical($"The application encountered a critical problem when deleting a consultation reason. ID = {raison.ID}");

            return RedirectToAction(nameof(Index));
        }
    }
}
