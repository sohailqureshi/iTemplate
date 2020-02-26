using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iTemplate.Web.Models
{
  [Table("AspNetPermissions")]
  public class ApplicationPermission
  {
    [Key]
    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Description { get; set; }

    public virtual ICollection<ApplicationRole> Roles { get; set; }

    public ApplicationPermission()
    {
      Roles = new HashSet<ApplicationRole>();
    }
  }
}
