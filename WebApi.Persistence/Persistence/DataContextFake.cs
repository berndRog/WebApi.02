using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Persistence;

public class DataContextFake: IDataContext {

   #region fields
   private readonly string _filePath =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
      + "WebApi02.json";      
   #endregion

   #region properties
   public Dictionary<Guid, Owner> Owners { get; } 
   #endregion

   #region ctor
   public DataContextFake() {
      try {
         if(!File.Exists(_filePath)) {
            Owners = new Dictionary<Guid, Owner>();
         } else {

            // Read the JSON file
            var json = File.ReadAllText(_filePath, Encoding.UTF8);

            // Deserialize the JSON string back into the CombinedDictionaries class
            Owners = JsonSerializer.Deserialize<Dictionary<Guid, Owner>>(
               json,
               new JsonSerializerOptions {
                  PropertyNameCaseInsensitive = true,
                  //       ReferenceHandler = ReferenceHandler.Preserve,
                  //       ReferenceHandler = ReferenceHandler.IgnoreCycles,
                  WriteIndented = true
               }
            );
         }
      }
      catch (Exception e) {
         Console.WriteLine(e.Message);
      }
   }
   #endregion

   #region methods
   public bool SaveAllChanges() {
      try {
         // Serialize to JSON
         var json = JsonSerializer.Serialize(
            Owners, 
            new JsonSerializerOptions {
               PropertyNameCaseInsensitive = true,
               //       ReferenceHandler = ReferenceHandler.Preserve,
               //       ReferenceHandler = ReferenceHandler.IgnoreCycles,
               WriteIndented = true
            }
         );
         // Write JSON string to file
         File.WriteAllText(_filePath, json, Encoding.UTF8);
         return true;
      }
      catch (Exception e) {
         Console.WriteLine(e.Message);
         return false;
      }
   }  
   #endregion

}