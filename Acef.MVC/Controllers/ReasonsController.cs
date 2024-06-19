using Acef.MVC.Interfaces;
using Acef.MVC.Models;
using Acef.MVC.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Acef.MVC.Controllers
{
    public class ReasonsController : Controller
    {
        private readonly IReasonService _reasonService;
        private readonly ILogger<ReasonsController> _logger;

        public ReasonsController(IReasonService reasonService, ILogger<ReasonsController> logger)
        {
            _reasonService = reasonService;
            _logger = logger;
        }

        // GET: ReasonController
        public async Task<IActionResult> Index(string? filter, string sortOrder)
        {
            var consultationReasons = await _reasonService.Get();

            // Filtering by name or description
            if (!string.IsNullOrEmpty(filter))
            {
                consultationReasons = consultationReasons.Where(
                    cr => cr.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ||
                    (cr.Description != null && cr.Description.Contains(filter, 
                        StringComparison.InvariantCultureIgnoreCase))
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

        // GET: ReasonController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ReasonDTO reason = await _reasonService.GetById(id);

            if (reason == null)
            {
                _logger.LogError($"An error occurred when retrieving" +
                    $" a consultation reason. ID = {reason.ID}");
                return NotFound();

            }
            return View(reason);
        }

        // GET: ReasonController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: ReasonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] ReasonDTO reason)
        {
            if (ModelState.IsValid)
            {
                await _reasonService.Add(reason);

                _logger.LogInformation(ReasonLog.Creation, $"Creating a consultation reason. Nom = {reason.Name}");

                _logger.LogCritical($"The application encountered a critical " +
                    $"problem when creating a consultation reason. Nom = {reason.Name}");

                return RedirectToAction(nameof(Index));
            }
            _logger.LogError($"An error occurred when creating a reason for consultation. Nom = {reason.Name}");
            return View(reason);
        }

        // GET: ReasonController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ReasonDTO reason = await _reasonService.GetById(id);

            if (reason == null)
            {
                _logger.LogError($"An error occurred when retrieving a consultation reason. ID = {reason.ID}");
                return NotFound();
            }

            return View(reason);
        }

        // POST: ReasonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] ReasonDTO reason)
        {
            if (reason == null)
            {
                _logger.LogError($"An error occurred when modifying a consultation reason. ID = {reason.ID}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _reasonService.Edit(reason);
                _logger.LogInformation(ReasonLog.Modification, $"Editing an offer. ID = {reason.ID}");

                _logger.LogCritical($"The application encountered a critical problem when modifying a consultation reason. ID = {reason.ID}");

                return RedirectToAction(nameof(Index));
            }

            return View(reason);
        }

        // GET: ReasonController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            ReasonDTO reason = await _reasonService.GetById(id);

            if (reason == null)
            {
                _logger.LogError($"An error occurred when retrieving a consultation reason. ID = {reason.ID}");
                return NotFound();
            }
            return View(reason);
        }

        // POST: ReasonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, ReasonDTO reason)
        {
            reason = await _reasonService.GetById(id);
            if (reason == null)
            {
                _logger.LogError($"An error occurred when retrieving a consultation reason. ID = {reason.ID}");
                return NotFound();
            }
            await _reasonService.Delete(reason);
            _logger.LogInformation(ReasonLog.Deletion, $"Deleting a consultation reason. ID = {reason.ID}");
            _logger.LogCritical($"The application encountered a critical problem when deleting a consultation reason. ID = {reason.ID}");

            return RedirectToAction(nameof(Index));
        }
    }
}
