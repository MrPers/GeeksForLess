using GeeksForLessMVC.Contracts;
using GeeksForLessMVC.Interfaces;
using GeeksForLessMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GeeksForLessMVC.Data
{
    public class TreeData : ITreeData
    {
        private readonly MyDbContext _context;
        
        public TreeData(MyDbContext context)
        {
            _context = context;
        }

        public async Task<TreeElement> TreeElementAsync(int id)
        {
            var result = await _context.TreeElements
                .Where(x => x.Id == id)
                .Include(u => u.Childrens)
                .FirstAsync();

            return result;
        }

        public async Task<bool> DeleteAsync()
        {
            var result = await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [TreeElements]") != 0;

            return result;
        }

        public async Task<bool> AddAsync(TreeElement treeElement)
        {
            await _context.TreeElements.AddAsync(treeElement);
            var result = await _context.SaveChangesAsync() != 0;

            return result;
        }
    }
}
