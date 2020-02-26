using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web;
using System.Xml;
using iTemplate.Web.Models;
using iTemplate.Web.Models.Data;
using iTemplate.Web.Data.Site;
using iTemplate.Web.Constants;
using System.IO;

namespace iTemplate.Web.Configuration
{
  public class InitiatilizeSite
  {
    const string importDirectory = "~/App_Data/Install/";
    const string staticListDirectory = "~/App_Data/StaticList/";

    public static void Setup(ApplicationDbContext db)
    {
      ImportAddressType(db);
      ImportContactType(db);
      ImportCountry(db);
      ImportThemes(db);
      ImportUserRoles(db);
      ImportUsers(db);
      ImportSite(db);

      ImportCategories(db);
      ImportNavigation(db);
    }

    private static XmlNodeList ReadXmlDocument(string fileName, string firstNode)
    {
      XmlDocument xmldoc = new XmlDocument();
      xmldoc.Load(System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), fileName));
      return xmldoc.GetElementsByTagName(firstNode);
    }

    /// <summary>
    /// 
    /// </summary>
    private static void ImportAddressType(ApplicationDbContext db)
    {
      XmlNodeList nodes = ReadXmlDocument("AddressType\\AddressType.xml", "defaults");

      var sortIndex = 0;
      foreach (XmlNode node in nodes)
      {
        AddressType addressType = new AddressType()
        {
          Name = node["Text"].InnerText,
          Description = node["Description"].InnerText,
          SortOrder = sortIndex++
        };
        //Add the object to the DB
        db.AddressTypes.Add(addressType);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportNavigation(ApplicationDbContext db)
    {
      XmlNodeList nodes = ReadXmlDocument("SiteNavigation.xml", "defaults");
      ImportSubMenus(db, nodes, null);
    }

    private static void ImportSubMenus(ApplicationDbContext db, XmlNodeList nodes, int? parentId)
    {
      var SortOrder = 0;
      ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));

      foreach (XmlNode node in nodes)
      {
        var sn = new SiteNavigationItem() {
          SortOrder = SortOrder++,
          AreaName = node["AreaName"].InnerText,
          ControllerName = node["ControllerName"].InnerText,
          ActionName = node["ActionName"].InnerText,
          ParentId = parentId
        };

        sn.IsDivider = true;
        if (node["IsDivider"] == null)
        {
          sn.Text = node["Text"].InnerText;
          sn.Description = node["Description"].InnerText;
          sn.Icon = node["Icon"].InnerText;
          sn.IsDivider = false;
        }

        if (node["InDashboard"] != null)
        {
          sn.InNavBar = false;
          sn.InDashboard = Convert.ToBoolean(node["InDashboard"].InnerText);
        }

        db.SiteNavigationItems.Add(sn);
        db.SaveChanges();

        /**************************************************************************************
          Add role to menu Item
        **************************************************************************************/
        foreach (XmlNode roles in node.SelectNodes("Roles"))
        {
          var roleName = roles["RoleName"].InnerText;
          var role = roleManager.FindByName(roleName);
          if (role != null)
          {
            var snr = new SiteNavigationRole()
            {
              MenuId = sn.Id,
              RoleId = role.Id
            };
            db.SiteNavigationRoles.Add(snr);
            db.SaveChanges();
          }
        }

        /**************************************************************************************
          Add any sub-menus - if any exist
        **************************************************************************************/
        if (node["SubMenu"] !=null) {
          ImportSubMenus(db, node.SelectNodes("SubMenu"), sn.Id);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportUserRoles(ApplicationDbContext db)
    {
      XmlNodeList nodes = ReadXmlDocument("UserRoles.xml", "Site.UserRoles");
      ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));

      foreach (XmlNode node in nodes)
      {
        var roleName = node["RoleName"].InnerText;
        string description = node["Description"].InnerText;
        bool isSystem = Convert.ToBoolean(node["IsSystem"].InnerText);

        var role = roleManager.FindByName(roleName);
        if (role == null)
        {
          role = new ApplicationRole(roleName, description, isSystem);
          var roleresult = roleManager.Create(role);
        }
      }
    }

    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportUsers(ApplicationDbContext db)
    {
      XmlDocument xmldoc = new XmlDocument();
      xmldoc.Load(System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), "Users.xml"));
      XmlNodeList nodes = xmldoc.GetElementsByTagName("Site.Users");

      ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
      ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
      foreach (XmlNode node in nodes)
      {
        var user = new ApplicationUser
        {
          UserName = node["Email"].InnerText,
          Email = node["Email"].InnerText,
          EmailConfirmed = true,
          FirstName = node["FirstName"].InnerText,
          LastName = node["LastName"].InnerText,
          DateOfBirth = DateTime.Now.AddYears(-25),
        };

        userManager.Create(user, node["Password"].InnerText);
        userManager.SetLockoutEnabled(user.Id, false);
        db.SaveChanges();

        //Add role to user if it doesn't already exist
        var roleName = node["RoleName"].InnerText;
        var rolesForUser = userManager.GetRoles(user.Id);
        if (!rolesForUser.Contains(roleName))
        {
          userManager.AddToRole(user.Id, roleName);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dr"></param>
    private static void ImportCountry(ApplicationDbContext db)
    {
      ImportContinent(db);
      ImportCurrency(db);
      ImportLanguage(db);

      var fileLocation = System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), "StaticData\\Country.tsv");
      System.IO.StreamReader sr = new System.IO.StreamReader(fileLocation);
      string line = sr.ReadLine(); //remove header information

      int iRow = 0;
      while ((line = sr.ReadLine()) != null)
      {
        iRow++;
        int iCol = 0;
        string[] strArray = line.Split(Convert.ToChar(9));

        string countryName = strArray[iCol++];
        string Iso2Code = strArray[iCol++];
        string Iso3Code = strArray[iCol++];
        string IsoNumeric = strArray[iCol++];
        string continentCode = strArray[iCol++];
        string currencyCode = strArray[iCol++];
        string languageCodes = strArray[iCol++];

        Country country = new Country()
        {
          Name = countryName,
          Iso2Code = Iso2Code,
          Iso3Code = Iso3Code,
          IsoNumeric =Convert.ToInt32(IsoNumeric),
          ContinentId= db.Continents.FirstOrDefault(c=>c.IsoCode.Equals(continentCode)).Id,
          CurrencyId = db.Currencies.FirstOrDefault(c => c.IsoCode.Equals(currencyCode)).Id,
          LanguageCodes = languageCodes
        };

        db.Countries.Add(country);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportContactType(ApplicationDbContext db)
    {
      var dirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(string.Concat(importDirectory, "\\ContactType")));
      var fileNames = dirInfo.GetFiles();
      foreach (var fileInfo in fileNames)
      {
        string filePath = string.Concat(dirInfo.Name, "/", fileInfo.Name);
        string key = string.Concat(dirInfo.Name.Replace(" ", "_"), ".", Path.GetFileNameWithoutExtension(fileInfo.Name));

        var sortIndex = 0;
        ContactType parentContactType = new ContactType()
        {
          Key = Path.GetFileNameWithoutExtension(fileInfo.Name),
          Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
          SortOrder = sortIndex++
        };
        db.ContactTypes.Add(parentContactType);
        db.SaveChanges();

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(System.IO.Path.Combine(HttpContext.Current.Server.MapPath(string.Concat(importDirectory, filePath))));
        XmlNodeList nodes = xmldoc.GetElementsByTagName("defaults");

        var soIndex = 0;
        foreach (XmlNode node in nodes)
        {
          ContactType contactType = new ContactType()
          {
            Key = node["Text"].InnerText,
            ParentId = parentContactType.Id,
            Name = node["Text"].InnerText,
            Description = node["Description"].InnerText,
            SortOrder = soIndex++
          };

          //Add the object to the DB
          db.ContactTypes.Add(contactType);
          db.SaveChanges();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportContinent(ApplicationDbContext db)
    {
      var fileLocation = System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), "StaticData\\Continent.tsv");
      System.IO.StreamReader sr = new System.IO.StreamReader(fileLocation);
      string line = sr.ReadLine(); //remove header information

      int iRow = 0;
      while ((line = sr.ReadLine()) != null)
      {
        iRow++;
        int iCol = 0;
        string[] strArray = line.Split(Convert.ToChar(9));
        Continent continent = new Continent()
        {
          IsoCode = strArray[iCol++],
          Name = strArray[iCol++]
        };

        db.Continents.Add(continent);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportCurrency(ApplicationDbContext db)
    {
      var fileLocation = System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), "StaticData\\Currency.tsv");
      System.IO.StreamReader sr = new System.IO.StreamReader(fileLocation);
      string line = sr.ReadLine(); //remove header information

      int iRow = 0;
      while ((line = sr.ReadLine()) != null)
      {
        iRow++;
        int iCol = 0;
        string[] strArray = line.Split(Convert.ToChar(9));
        Currency currency = new Currency()
        {
          IsoCode = strArray[iCol++],
          Name = strArray[iCol++],
          IsoNumeric= Convert.ToInt32(strArray[iCol++]),
          MinorEntity= Convert.ToInt32(strArray[iCol++])
        };

        db.Currencies.Add(currency);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportLanguage(ApplicationDbContext db)
    {
      var fileLocation = System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), "StaticData\\Language.tsv");
      System.IO.StreamReader sr = new System.IO.StreamReader(fileLocation);
      string line = sr.ReadLine(); //remove header information

      int iRow = 0;
      while ((line = sr.ReadLine()) != null)
      {
        iRow++;
        int iCol = 0;
        string[] strArray = line.Split(Convert.ToChar(9));
        Language language = new Language()
        {
          IsoCode = strArray[iCol++],
          Name = strArray[iCol++]
        };

        db.Languages.Add(language);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportSite(ApplicationDbContext db)
    {
      ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
      ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));

      XmlDocument xmldoc = new XmlDocument();
      xmldoc.Load(System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), "Site.xml"));
      XmlNodeList defaults = xmldoc.GetElementsByTagName("defaults");

      foreach (XmlNode node in defaults)
      {
        var roleName = node["AdminRole"].InnerText;
        var user = userManager.FindByEmail(node["AdminEmail"].InnerText);
        var roleID = db.Roles.FirstOrDefault(x => x.Name.Equals(roleName)).Id;

        SiteConfiguration ss = new SiteConfiguration()
        {
          Name = node["Name"].InnerText,
          LogoUrl = node["LogoUrl"].InnerText,
          Disclaimer = node["Disclaimer"].InnerText,
          RegistrationMethod = (Enums.RegistrationMethods)Convert.ToInt32(node["RegistrationMethod"].InnerText),
          ThemeId = Convert.ToInt32(node["ThemeId"].InnerText),
          AdministratorRoleID = roleID,
          CreatedBy = user.Id
        };

        foreach (XmlNode addressField in node.SelectNodes("Address"))
        {
          ss.Address.Line1 = addressField["Line1"].InnerText;
          ss.Address.Line2 = addressField["Line2"].InnerText;
          ss.Address.Line3 = addressField["Line3"].InnerText;
          ss.Address.Line4 = addressField["Line4"].InnerText;
          ss.Address.Line5 = addressField["Line5"].InnerText;
          ss.Address.PostalCode = addressField["PostalCode"].InnerText;
          ss.Address.Telephone = addressField["Telephone"].InnerText;
          ss.Address.Email = addressField["Email"].InnerText;

          var cc = addressField["CountryCode"].InnerText;
          Country country = db.Countries.FirstOrDefault(x => x.Iso2Code.Equals(cc));
          ss.Address.CountryId = country.Id;
        }

        //AddressBook ab = new AddressBook(Enums.AddressTypes.Correspondence)
        //{
        //  Line1 = ss.Address.Line1,
        //  Line2 = ss.Address.Line2,
        //  Line3 = ss.Address.Line3,
        //  Line4 = ss.Address.Line4,
        //  Line5 = ss.Address.Line5,
        //  PostalCode = ss.Address.PostalCode,
        //  CountryId= ss.Address.CountryId,
        //  CreatedBy = ss.CreatedBy
        //};

        //db.Addresses.Add(ab);
        db.SiteConfigurations.Add(ss);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    private static void ImportThemes(ApplicationDbContext db)
    {
      XmlDocument xmldoc = new XmlDocument();
      xmldoc.Load(System.IO.Path.Combine(HttpContext.Current.Server.MapPath(importDirectory), "SiteThemes.xml"));
      XmlNodeList defaults = xmldoc.GetElementsByTagName("defaults");

      foreach (XmlNode node in defaults)
      {
        SiteTheme t = new SiteTheme()
        {
          Name = node["Key"].InnerText,
          Url = node["Url"].InnerText
      };
        db.SiteThemes.Add(t);
        db.SaveChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private static void ImportCategories(ApplicationDbContext db)
    {
      var sortIndex = 0;
      var dirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(staticListDirectory));
      foreach (var dir in dirInfo.GetDirectories())
      {
        StaticCategory staticCategory = new StaticCategory()
        {
          Name = dir.Name,
          SortOrder = sortIndex++
        };
        db.StaticCategories.Add(staticCategory);
        db.SaveChanges();

        var soIndex = 0;
        var fileNames = dir.GetFiles();
        foreach (var fileInfo in fileNames)
        {
          string filePath = string.Concat(dir.Name, "/", fileInfo.Name);
          string key = string.Concat(dir.Name.Replace(" ", "_"), ".", Path.GetFileNameWithoutExtension(fileInfo.Name));

          StaticCategoryList staticCategoryList = new StaticCategoryList()
          {
            Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
            StaticCategory = staticCategory,
            SortOrder = soIndex++
          };
          db.StaticCategoryLists.Add(staticCategoryList);
          db.SaveChanges();

          ImportCategoryList(db, staticCategoryList, key, filePath);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private static void ImportCategoryList(ApplicationDbContext db, StaticCategoryList staticCategoryList, string key, string fileName)
    {
      XmlDocument xmldoc = new XmlDocument();
      xmldoc.Load(System.IO.Path.Combine(HttpContext.Current.Server.MapPath(staticListDirectory), fileName));
      XmlNodeList nodes = xmldoc.GetElementsByTagName("defaults");

      var sortIndex = 0;
      foreach (XmlNode node in nodes)
      {
        StaticCategoryListItem staticListItem = new StaticCategoryListItem();
        staticListItem.StaticCategoryList = staticCategoryList;
        staticListItem.Name = node["Text"].InnerText;
        staticListItem.Description = node["Description"].InnerText;
        staticListItem.SortOrder = sortIndex++;

        //Add the object to the DB
        db.StaticCategoryListItems.Add(staticListItem);
        db.SaveChanges();
      }
    }


    /// <summary>
    /// /
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static int parseInt(object obj)
    {
      string parse = (obj.ToString().Length > 0) ? obj.ToString() : "0";
      return Convert.ToInt32(parse);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static decimal parseDecimal(object obj)
    {
      string parse = (obj.ToString().Length > 0) ? obj.ToString() : "0";
      return Convert.ToDecimal(parse);
    }   
  }
}
 