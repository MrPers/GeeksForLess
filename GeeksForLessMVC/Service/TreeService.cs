using GeeksForLessMVC.Contracts;
using GeeksForLessMVC.Interfaces;
using GeeksForLessMVC.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GeeksForLessMVC.Service
{
    public class TreeService : ITreeService
    {
        private readonly ITreeData _treeData;
        public TreeService(ITreeData treeData)
        {
            _treeData = treeData;
        }
        public async Task<TreeElement> ListTreeElementAsync(int id)
        {
            TreeElement treeElement = await _treeData.TreeElementAsync(id);

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
                    treeElement.Childrens.Add(await ListTreeElementAsync(treeElementId));
                }
            }

            return treeElement;
        }

        public async Task<bool> UploadAsync(IFormFile file)
        {
            var treeElement = new TreeElement();
            var contentFile = new StringBuilder();

            if (!await _treeData.DeleteAsync())
            {
                return false;
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    contentFile.AppendLine(await reader.ReadLineAsync());
            }

            switch (file.FileName.Split('.')[1])
            {
                case "json":
                {
                    JToken configRoot = JToken.Parse(contentFile.ToString());
                    treeElement = Read(configRoot);
                    break;
                }
                case "txt":
                {

                    break;
                }
                default:
                    throw new Exception();
            }
            return await _treeData.AddAsync(treeElement);
        }

        private static TreeElement Read(JToken content)
        {
            TreeElement treeElement = new()
            {
                Name = content.Path.Split('.').Last(),
            };

            switch (content)
            {
                case JObject nonTerminal:
                    foreach (var child in nonTerminal.Children<JProperty>())
                    {
                        treeElement.Childrens.Add(Read(child.Value));
                    }
                    break;
                case JValue terminal:
                    {
                        treeElement.Value = terminal.Value?.ToString();
                    }
                    break;
                default:
                    throw new ArgumentException(nameof(content));
            }

            return treeElement;
        }
    }
}
