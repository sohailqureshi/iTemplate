using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using iTemplate.Web.Configuration;
using iTemplate.Web.Models.Data;
using System.Linq;
using System;
using System.Data.Entity.ModelConfiguration;

namespace iTemplate.Web.Models
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext() : base("DefaultConnection"){}

    //Core Structure
    public virtual DbSet<AddressBook> Addresses { get; set; }
    public virtual DbSet<AddressType> AddressTypes { get; set; }
    public virtual DbSet<Connection> Connections { get; set; }
    public virtual DbSet<ContactDetail> ContactDetails { get; set; }
    public virtual DbSet<ContactType> ContactTypes { get; set; }
    public virtual DbSet<Continent> Continents { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<SiteNavigationItem> SiteNavigationItems { get; set; }
    public virtual DbSet<SiteNavigationRole> SiteNavigationRoles { get; set; }
    public virtual DbSet<SiteConfiguration> SiteConfigurations { get; set; }
    public virtual DbSet<SiteLookUp> SiteLookUps { get; set; }
    public virtual DbSet<StaticCategory> StaticCategories { get; set; }
    public virtual DbSet<StaticCategoryList> StaticCategoryLists { get; set; }
    public virtual DbSet<StaticCategoryListItem> StaticCategoryListItems { get; set; }
    public virtual DbSet<SiteTheme> SiteThemes { get; set; }
    public virtual DbSet<ApplicationPermission> Permissions { get; set; }

    static ApplicationDbContext()
    {
      // Set the database intializer which is run once during application start
      // This seeds the database with admin user credentials and admin role
      Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
    }

    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      if (modelBuilder == null)
      {
        throw new ArgumentNullException("ModelBuilder is NULL");
      }

      base.OnModelCreating(modelBuilder);

      //// Add this - so that IdentityUser can share a table with ApplicationUser
      //modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");
      //EntityTypeConfiguration<ApplicationUser> tableUsers = modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

      //// Add this - so that IdentityRole can share a table with ApplicationRole
      //modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
      //EntityTypeConfiguration<ApplicationRole> tableRoles = modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

      //modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

      modelBuilder.Entity<ApplicationUser>()
        .HasMany(t => t.UserRoles)
        .WithMany(t => t.RoleUsers)
        .Map(m =>
        {
          m.ToTable("AspNetRoleUsers");
          m.MapLeftKey("UserId");
          m.MapRightKey("RoleId");
        });

      modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
      modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
      modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles");
      modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");
      modelBuilder.Entity<IdentityUserLogin>().ToTable("AspNetUserLogins");

      modelBuilder.Entity<ApplicationPermission>()
      .HasMany(e => e.Roles)
      .WithMany(e => e.Permissions)
      .Map(m => m.ToTable("AspNetRolePermissions")
      .MapLeftKey("PermissionId").MapRightKey("RoleId"));
        }
  }


  // This is useful if you do not want to tear down the database each time you run the application.
  // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
  // This example shows you how to create a new database if the Model changes
  public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
  {
    protected override void Seed(ApplicationDbContext context)
    {
      InitiatilizeSite.Setup(context);
      base.Seed(context);
    }
  }
}