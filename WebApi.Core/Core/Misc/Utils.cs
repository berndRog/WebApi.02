using System;
namespace WebApi.Utilities; 

public static class Utils {
   public static string As8(this Guid guid) => guid.ToString()[..7];
}