using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
using WebApi.Utilities;

namespace WebApi.Controllers; 
[Route("banking/owners")]
[ApiController]
public class OwnersController(
   // Dependency injection
   IOwnersRepository ownersRepository,
   IDataContext dataContext,
   ILogger<OwnersController> logger
) : ControllerBase {
   
   // Get all owners
   // http://localhost:5100/banking/owners
   [HttpGet("")]
   public ActionResult<IEnumerable<Owner>> GetOwners() {
      logger.LogDebug("GetOwners()");
      return Ok(ownersRepository.Select());  
   }

   // Get owner by Id
   // http://localhost:5100/banking/owners/{id}
   [HttpGet("{id}")]
   public ActionResult<Owner> GetOwnerById(
      [FromRoute] Guid id
   ) {
      logger.LogDebug("GetOwnerById() id={id}", id.As8());
      switch (ownersRepository.FindById(id)) {
         case Owner owner: return Ok(owner);
         case null:        return NotFound($"Owner with given Id not found");
      }
   }
   
   // Get owners by name
   // http://localhost:5100/banking/owners/name
   [HttpGet("name")]
   public ActionResult<IEnumerable<Owner>> GetOwnersByName(
      [FromQuery] string name
   ) {
      logger.LogDebug("GetOwnersByName() name={name}", name);
      switch (ownersRepository.SelectByName(name)) {
         case IEnumerable<Owner> owners: 
            return Ok(owners);
         case null: 
            return NotFound($"Owners with given name not found");
      }
   }

   // Get owner by email
   // http://localhost:5100/banking/owners/email
   [HttpGet("email")]
   public ActionResult<Owner?> GetOwnerByEmail(
      [FromQuery] string email
   ) {
      logger.LogDebug("GetOwnerByEmail() email={email}", email);
      switch (ownersRepository.FindByEmail(email)) {
         case Owner owner: return Ok(owner);
         case null:        return NotFound($"Owner with given email not found");
      }
   }

   // Get owners by birthdate 
   // http://localhost:5100/banking/owners/birthdate/?from=yyyy-MM-dd&to=yyyy-MM-dd
   [HttpGet("birthdate")]
   public ActionResult<IEnumerable<Owner>> GetOwnerByBirthdate(
      [FromQuery] string from,   // Date must be in the format yyyy-MM-dd
                                 // MM = 01 for January through 12 for December
      [FromQuery] string to      
   ) {
      logger.LogDebug("GetOwnerByBirthdate() from={from} to={to}", from, to);
      
      // Convert string to DateTime
      var (errorFrom, dateFrom) = ConvertToDateTime(from);
      if(errorFrom) 
         return BadRequest($"GetOwnerByBirthdate: Invalid date 'from': {from}");
      var (errorTo, dateTo) = ConvertToDateTime(to);
      if(errorTo) 
         return BadRequest($"GetOwnerByBirthdate: Invalid date 'to': {to}");

      // Get owners by birthdate
      var owners = ownersRepository.SelectByBirthDate(dateFrom, dateTo);   
      
      // return Ok with owners
      return Ok(owners);
   }
   
   // Convert string in German format dd.MM.yyyy to DateTime
   private (bool, DateTime) ConvertToDateTime(string date) {
      try {
         var dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
         return (false, dateTime );
      } catch (Exception e) {
         return (true, DateTime.MinValue);
      }
   }
   
   // Create a new owner
   // http://localhost:5100/banking/owners
   [HttpPost("")]
   public ActionResult<Owner> CreateOwner(
      [FromBody] Owner owner
   ) {
      logger.LogDebug("CreateOwner() owner={owner}", owner.Name);
      
      // check if owner.Id is set, else generate new Id
      if(owner.Id == Guid.Empty) owner.Id = Guid.NewGuid();
      // check if owner with given Id already exists   
      if(ownersRepository.FindById(owner.Id) != null) 
         return Conflict($"CreateOwner: Owner with the given id already exists");
      
      // add owner to repository
      ownersRepository.Add(owner); 
      // save to datastore
      dataContext.SaveAllChanges();
      
      // return created owner      
      var uri = new Uri($"{Request.Path}/{owner.Id}", UriKind.Relative);
      return Created(uri: uri, value: owner);     
   }
   
   // Update owner
   // http://localhost:5100/banking/owners/{id}
   [HttpPut("{id:Guid}")] 
   public ActionResult<Owner> UpdateOwner(
      [FromRoute] Guid id,
      [FromBody]  Owner updOwner
   ) {
      
      logger.LogDebug("UpdateOwner() id={id} updOwner={updOwner}",id.As8(), updOwner.Name);
      
      if(id != updOwner.Id) 
         return BadRequest($"UpdateOwner: Id in the route and body do not match.");
      
//    Owner updOwner = mapper.Map<Owner>(updPersonDto);
      
      Owner? owner = ownersRepository.FindById(id);
      if (owner == null)
         return NotFound($"UpdateOwner: Owner with given id not found.");

      // Update person
      owner = new Owner(updOwner);
      
      // save to repository and write to database 
      ownersRepository.Update(owner);
      dataContext.SaveAllChanges();

      // return ok with updated owner
      return Ok(owner); 
   }
   
   // Delete owner
   // http://localhost:5100/banking/owners/{id}
   [HttpDelete("{id:Guid}")]
   public IActionResult DeleteOwner(
      [FromRoute] Guid id
   ) {
      logger.LogDebug("DeleteOwner {id}", id.As8());
      
      Owner? owner = ownersRepository.FindById(id);
      if(owner == null)
         return NotFound($"DeleteOwner: Owner with given id not found.");

      // delete in repository and write to database 
      ownersRepository.Remove(owner);
      dataContext.SaveAllChanges();

      // return no content
      return NoContent();
   }

}