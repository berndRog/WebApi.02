using System;
using System.Collections.Generic;
namespace WebApi.Core.DomainModel.Entities; 

public record Owner: IEntity {
   
   #region properties
   public Guid     Id       { get; init; } = Guid.Empty;
   public string   Name     { get; set; } = string.Empty;
   public DateTime Birthdate{ get; set; } = DateTime.UtcNow;
   public string   Email    { get; set; } = string.Empty;
   #endregion


   #region ctor
   public Owner() { }
   
   public void Update(string name, string email) {
      Name = name;
      Email = email;
   }
   #endregion
}
