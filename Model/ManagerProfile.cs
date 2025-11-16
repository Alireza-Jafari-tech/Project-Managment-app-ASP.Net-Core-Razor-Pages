using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthTest.Model
{
  public class ManagerProfile
  {
    [Key]
    public int Id { get; set; }
    public string Department { get; set; }
  }
}