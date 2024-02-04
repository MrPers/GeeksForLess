using Microsoft.AspNetCore.Mvc;
using GeeksForLessMVC.Interfaces;
using GeeksForLessMVC.Models;

namespace GeeksForLessMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITreeService _configService;
        private const int idFirstElement = 1;

        public HomeController(ITreeService configService) =>
            _configService = configService;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Display()
        {
            try
            {
                TreeElement result = await _configService.ListTreeElementAsync(idFirstElement);

                return View(result);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file != null || file.Length != 0)
                {
                    var result = await _configService.UploadAsync(file);

                    if (result)
                    {
                        return RedirectToAction("Display");
                    }
                }

                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
