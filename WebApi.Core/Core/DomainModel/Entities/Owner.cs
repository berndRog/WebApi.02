using System;
namespace WebApi.Core.DomainModel.Entities; 

public class Owner: AEntity {
   
   #region properties
   public override Guid Id  { get; init; } = Guid.NewGuid();
   public string   Name     { get; set; } = string.Empty;
   public DateTime Birthdate{ get; init; } = DateTime.UtcNow;
   public string   Email    { get; set; } = string.Empty;
   #endregion
   
   #region methods
   public void Update(string name, string email) {
         Name = name;
         Email = email;
   }
   #endregion
}









// public record Owner: IEntity {
//    
//    #region properties
//    public Guid     Id       { get; init; } = Guid.Empty;
//    public string   Name     { get; set; } = string.Empty;
//    public DateTime Birthdate{ get; set; } = DateTime.UtcNow;
//    public string   Email    { get; set; } = string.Empty;
//    #endregion
//
//
//    #region ctor
//    public Owner() { }
//    
//    public void Update(string name, string email) {
//       Name = name;
//       Email = email;
//    }
//    #endregion
// }


