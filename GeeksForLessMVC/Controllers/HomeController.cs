using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace GeeksForLessMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Display()
        {
            var result =  await ListTreeElement();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file != null || file.Length != 0)
                {
                    TreeElement treeElement = new();
                    var configFile = new StringBuilder();

                    await _context.TreeElements.ExecuteDeleteAsync();
                    await _context.SaveChangesAsync();

                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            configFile.AppendLine(await reader.ReadLineAsync());
                    }

                    switch (file.FileName.Split('.')[1])
                    {
                        case "json":
                            treeElement = Parse(configFile.ToString());
                            break;
                        case "txt":


                            break;
                        default:
                            return RedirectToAction("Index");
                    }

                    await _context.TreeElements.AddAsync(treeElement);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Display");
                }

                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public TreeElement Parse(string configFile)
        {
            JToken configRoot = JToken.Parse(configFile);
            TreeElement result = NewMethod(configRoot);

            return result;
        }

        async Task<TreeElement> ListTreeElement(int id = 1)
        {
            var treeElement = await _context.TreeElements
                .Where(x => x.Id == id)
                .Include(u => u.Childrens)
                .FirstAsync();

            List<int> treeElements = new List<int>();

            foreach (var dot in treeElement.Childrens)
            {
                treeElements.Add(dot.Id);
            }

            treeElement.Childrens = new List<TreeElement>();

            if (treeElements.Any())
            {
                foreach (var treeElementId in treeElements)
                {
                    treeElement.Childrens.Add(await ListTreeElement(treeElementId));
                }
            }

            return treeElement;
        }

        static TreeElement NewMethod(JToken configRoot)
        {
            TreeElement treeElement = new()
            {
                Name = configRoot.Path.Split('.').Last(),
            };

            switch (configRoot)
            {
                case JObject nonTerminal:
                    foreach (var child in nonTerminal.Children<JProperty>())
                        treeElement.Childrens.Add(NewMethod(child.Value));
                    break;

                case JValue terminal:
                    treeElement.Value = terminal.Value?.ToString();
                    break;

                default:
                    throw new ArgumentException(nameof(configRoot));
            }

            return treeElement;
        }
    }

    public class TreeElement
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;
        public string? Value { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public TreeElement? Parent { get; set; }
        public IList<TreeElement> Childrens { get; set; } = new List<TreeElement>();
    }
}
