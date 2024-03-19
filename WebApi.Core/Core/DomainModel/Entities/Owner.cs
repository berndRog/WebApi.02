using System;
using System.Collections.Generic;
namespace WebApi.Core.DomainModel.Entities; 

public record Owner: IEntity {
   #region properties
   public Guid     Id       { get; set; } = Guid.Empty;
   public string   Name     { get; set; } = string.Empty;
   public DateTime Birthdate{ get; set; } = DateTime.UtcNow;
   public string   Email    { get; set; } = string.Empty;
   #endregion


   #region ctor
   public Owner() { }
   
   public Owner (Owner source) {
      Id = source.Id;
      Name = source.Name;
      Birthdate = source.Birthdate;
      Email = source.Email;
   }
   #endregion
}
