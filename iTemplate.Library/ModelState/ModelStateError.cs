using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcEmpty.Library.ModelState
{
  public class Error
  {
    public static void Show(ICollection<System.Web.Mvc.ModelState> modelStateErrors) 
    {
      foreach (System.Web.Mvc.ModelState modelState in modelStateErrors)
      {
        foreach (ModelError error in modelState.Errors)
        {
          var x = error;
        }
      }
    }
  }
}
