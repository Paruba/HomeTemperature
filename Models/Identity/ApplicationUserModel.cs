using System.ComponentModel.DataAnnotations;

namespace Boiler.Mobile.Models.Identity;

public class ApplicationUserModel
{
    [Key]
    public string Id { get; set; }
    public string UserName { get; set; }
    public virtual IEnumerable<ApplicationRoleModel> Roles { get; set; }
}