using GeeksForLessMVC.Models;

namespace GeeksForLessMVC.Contracts
{
    public interface ITreeData
    {
        public Task<TreeElement> TreeElementAsync(int id);
        public Task<bool> DeleteAsync();
        public Task<bool> AddAsync(TreeElement treeElement);
    }
}
