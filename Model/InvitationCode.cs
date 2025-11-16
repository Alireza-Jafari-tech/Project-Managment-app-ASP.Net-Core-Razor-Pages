using System.ComponentModel.DataAnnotations;

namespace AuthTest.Model
{
  public class InvitationCode
  {
    [Key]
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public int Code { get; set; }
  }
}