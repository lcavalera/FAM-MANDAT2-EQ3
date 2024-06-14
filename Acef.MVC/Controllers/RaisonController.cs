using Acef.MVC.Interfaces;
using Acef.MVC.Models.DTO;
using Acef.MVC.Models.Log;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Index()
        {
            return View(await _raisonService.ObtenirTout());
        }

        // GET: RaisonController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            RaisonDTO raison = await _raisonService.ObtenirSelonId(id);

            if (raison == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une raison de consultation. ID = {raison.ID}");
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
        public async Task<IActionResult> Create([Bind("ID,NomRaison")] RaisonDTO raison)
        {
            if (ModelState.IsValid)
            {
                await _raisonService.Ajouter(raison);
                _logger.LogInformation(RaisonLog.Creation, $"Creation d'une raison de consultation. Nom = {raison.NomRaison}");

                _logger.LogCritical($"L'application a rencontré un problème critique lors de la création d'une raison de consultation. Nom = {raison.NomRaison}");

                return RedirectToAction(nameof(Index));
            }
            _logger.LogError($"Une erreur c'est produite lors de la création d'une raison de consultation. Nom = {raison.NomRaison}");
            return View(raison);
        }

        // GET: RaisonController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            RaisonDTO raison = await _raisonService.ObtenirSelonId(id);

            if (raison == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une raison de consultation. ID = {raison.ID}");
                return NotFound();
            }
            return View(raison);
        }

        // POST: RaisonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NomRaison")] RaisonDTO raison)
        {
            if (raison == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la modification d'une raison de consultation. ID = {raison.ID}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _raisonService.Modifier(raison);
                _logger.LogInformation(RaisonLog.Modication, $"Modification d'une offre. ID = {raison.ID}");

                _logger.LogCritical($"L'application a rencontré un problème critique lors de la modification d'une raison de consultation. ID = {raison.ID}");

                return RedirectToAction(nameof(Index));
            }

            return View(raison);
        }

        // GET: RaisonController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            RaisonDTO raison = await _raisonService.ObtenirSelonId(id);

            if (raison == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une raison de consultation. ID = {raison.ID}");
                return NotFound();
            }
            return View(raison);
        }

        // POST: RaisonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, RaisonDTO raison)
        {
            raison = await _raisonService.ObtenirSelonId(id);
            if (raison == null)
            {
                _logger.LogError($"Une erreur c'est produite lors de la récupération d'une raison de consultation. ID = {raison.ID}");
                return NotFound();
            }
            await _raisonService.Supprimer(raison);
            _logger.LogInformation(RaisonLog.Suppression, $"Suppression d'une raison de consultation. ID = {raison.ID}");
            _logger.LogCritical($"L'application a rencontré un problème critique lors de la suppression d'une raison de consultation. ID = {raison.ID}");

            return RedirectToAction(nameof(Index));
        }
    }
}
