using System;
using System.Collections.Generic;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Core;

public interface IOwnersRepository {
   IEnumerable<Owner> Select();
   Owner? FindById(Guid id);
   void Add(Owner owner);
   void Update(Owner owner);
   void Remove(Owner owner);
}