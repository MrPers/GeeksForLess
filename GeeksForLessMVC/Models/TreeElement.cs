using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeeksForLessMVC.Models
{
    public class TreeElement
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public TreeElement Parent { get; set; }
        public ICollection<TreeElement> Childrens { get; set; } = new List<TreeElement>();
    }
}
