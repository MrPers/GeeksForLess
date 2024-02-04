using GeeksForLessMVC.Controllers;
using GeeksForLessMVC.Models;
using Newtonsoft.Json.Linq;

namespace GeeksForLessMVC.Interfaces;

public interface ITreeService
{
    public Task<TreeElement> ListTreeElementAsync(int id);
    public Task<bool> UploadAsync(IFormFile file);
}