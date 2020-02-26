using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using iTemplate.Web.Models;
using iTemplate.Web.Models.Data;
using iTemplate.Web.Data.Site;
using System.Net;
using Microsoft.AspNet.Identity;

namespace iTemplate.Web.Areas.Admin.Controllers
{
  public class SiteSettingController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

     //
    // GET: /Site/Edit/5
    public ActionResult SiteEdit(int id = 1)
    {
      var sitesetting = db.SiteConfigurations.Find(id);
      if (sitesetting == null) { return HttpNotFound(); }

      var currentUser = db.Users.Find(User.Identity.GetUserId());
      if (!sitesetting.CreatedBy.Equals(currentUser.Id))
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      return View(sitesetting);
    }

    //
    // POST: /Site/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult SiteEdit(SiteConfiguration sitesetting)
    {
      if (ModelState.IsValid)
      {
        db.Entry(sitesetting).State = EntityState.Modified;
        db.SaveChanges();

        Settings.Reset();
      }

      return View(sitesetting);
    }

    //
    // GET: /Import
    public ActionResult FileUpload()
    {
      return View();
    }

    [HttpPost]
    public ActionResult FileUpload(HttpPostedFileBase file)
    {
      if (file != null && file.ContentLength > 0)
      {
        string fileExtension = System.IO.Path.GetExtension(file.FileName);
        if (fileExtension != ".txt") { return View(); }

        var fileName = file.FileName;
        var fileLocation = System.IO.Path.Combine(Server.MapPath("~/App_Data/"), fileName);
        if (System.IO.File.Exists(fileLocation))
        {
          System.IO.File.Delete(fileLocation);
        }

        file.SaveAs(fileLocation);
      }

      return RedirectToAction("SiteIndex");
    }

    //
    // GET: /Import
    public ActionResult FileImport()
    {
      //DirectoryInfo importFiles = null;
      //FileInfo[] files = null;

      //string importPath = Server.MapPath("~/App_Data/");
      //importFiles = new DirectoryInfo(importPath);
      //files = importFiles.GetFiles();

      //var filesFound = files.Where(f => f.Extension == ".csv" || f.Extension == ".txt")
      //  .OrderBy(f => f.Name)
      //  .Select(f => f.Name).ToList();

      return View();
    }

    [HttpPost]
    public ActionResult FileImport(string fileName)
    {
      string[] strArray;
      string line = string.Empty;
 
      DataRow dr;
      DataTable dtImport = new DataTable();

      var fileLocation = System.IO.Path.Combine(Server.MapPath("~/App_Data/"), fileName);
      System.IO.StreamReader sr = new System.IO.StreamReader(fileLocation);

      line = sr.ReadLine();
      strArray = line.Split('|');
      Array.ForEach(strArray, s => dtImport.Columns.Add(new DataColumn()));

      int iRow = 0;
      while ((line = sr.ReadLine()) != null)
      {
        dr = dtImport.NewRow();
        dr.ItemArray = line.Split('|');
        dtImport.Rows.Add(dr);
        iRow++;

        //int userID = WebSecurity.GetUserId(dr[12].ToString());
        //if (userID < 1) { userID = UserProfile.Create(dr[12].ToString(), "!£$%^^*@:PB?", dr[2].ToString(), dr[3].ToString(), true).Id; }

        switch (fileName)
        {
          case "CountryList.txt":
            ImportCountryList(dr);
            break;
        }
        //iTemplate.Web.SignalR.MessageHandler.Instance.UpdateStatus(iRow.ToString());
      }

      sr.Dispose();
      return RedirectToAction("SiteIndex");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="dr"></param>
    private void ImportCountryList(DataRow dr)
    {
      int iCol = 0;
      Country country = new Country();
      //country.LanguageCode = Convert.ToString(dr[iCol++]);
      //country.CountryName = Convert.ToString(dr[iCol++]);
      //country.CountryCode = Convert.ToString(dr[iCol++]);

      try
      {
        var p = db.Countries.Find(country.Id);
        if (p == null)
        {
          db.Countries.Add(country);
          db.SaveChanges();
        }
      }
      catch (Exception e) { var message = e.Message; }
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private int parseInt(object obj)
    {
      string parse = (obj.ToString().Length > 0) ? obj.ToString() : "0";
      return Convert.ToInt32(parse);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private decimal parseDecimal(object obj)
    {
      string parse = (obj.ToString().Length > 0) ? obj.ToString() : "0";
      return Convert.ToDecimal(parse);
    }

    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}