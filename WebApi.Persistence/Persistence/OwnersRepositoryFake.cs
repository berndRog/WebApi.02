using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;

namespace WebApi.Persistence;

public class OwnersRepositoryFake(
   IDataContext dataContext
): IOwnersRepository {
   
   public IEnumerable<Owner> Select() {
      return dataContext.Owners.Values.ToList();
   }

   public Owner? FindById(Guid id) { 
      dataContext.Owners.TryGetValue(id, out Owner? owner);
      return owner;
   }

   public void Add(Owner owner) {
      dataContext.Owners.Add(owner.Id, owner);
   }
   
   public void Update(Owner owner) {
      dataContext.Owners.Remove(owner.Id);
      dataContext.Owners.Add(owner.Id, owner);
   }

   public void Remove(Owner owner) {
      dataContext.Owners.Remove(owner.Id);
   }
}