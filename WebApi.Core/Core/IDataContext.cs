using System;
using System.Collections.Generic;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Core;

public interface IDataContext {
   Dictionary<Guid, Owner> Owners { get;  } 
   bool SaveAllChanges();
}