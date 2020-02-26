using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace iTemplate.Web.Models
{
  public class ImageInfo
  {
    [Display(Name = "Profile Photo")]
    public byte[] Photo { get; set; }

    public int FileLength { get; set; }

    public string ContentType { get; set; }

    public ImageInfo() { }

    public ImageInfo(HttpPostedFileBase fileUpload)
    {
      if (fileUpload != null)
      {
        Stream fileStream = fileUpload.InputStream;
        int fileLength = fileUpload.ContentLength;

        this.FileLength = fileLength;
        this.ContentType = fileUpload.ContentType;
        this.Photo = new byte[fileLength];

        fileStream.Read(this.Photo, 0, fileLength);
      }
    }
  }
}
