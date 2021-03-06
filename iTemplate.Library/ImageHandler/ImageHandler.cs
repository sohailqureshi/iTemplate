﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace iTemplate.Library
{
  public class ImageHandler
  {
    public static string GetContentType(string SourceFileName)
    {
      var extension = Path.GetExtension(SourceFileName).ToLower();
      switch (extension)
      {
        case ".ai": return "application/postscript";
        case ".aif": return "audio/x-aiff";
        case ".aifc": return "audio/x-aiff";
        case ".aiff": return "audio/x-aiff";
        case ".asc": return "text/plain";
        case ".au": return "audio/basic";
        case ".avi": return "video/x-msvideo";
        case ".bcpio": return "application/x-bcpio";
        case ".bin": return "application/octet-stream";
        case ".c": return "text/plain";
        case ".cc": return "text/plain";
        case ".ccad": return "application/clariscad";
        case ".cdf": return "application/x-netcdf";
        case ".class": return "application/octet-stream";
        case ".cpio": return "application/x-cpio";
        case ".cpp": return "text/plain";
        case ".cpt": return "application/mac-compactpro";
        case ".cs": return "text/plain";
        case ".csh": return "application/x-csh";
        case ".css": return "text/css";
        case ".dcr": return "application/x-director";
        case ".dir": return "application/x-director";
        case ".dms": return "application/octet-stream";
        case ".doc": return "application/msword";
        case ".drw": return "application/drafting";
        case ".dvi": return "application/x-dvi";
        case ".dwg": return "application/acad";
        case ".dxf": return "application/dxf";
        case ".dxr": return "application/x-director";
        case ".eps": return "application/postscript";
        case ".etx": return "text/x-setext";
        case ".exe": return "application/octet-stream";
        case ".ez": return "application/andrew-inset";
        case ".f": return "text/plain";
        case ".f90": return "text/plain";
        case ".fli": return "video/x-fli";
        case ".flv": return "video/x-flv";
        case ".gif": return "image/gif";
        case ".gtar": return "application/x-gtar";
        case ".gz": return "application/x-gzip";
        case ".h": return "text/plain";
        case ".hdf": return "application/x-hdf";
        case ".hh": return "text/plain";
        case ".hqx": return "application/mac-binhex40";
        case ".htm": return "text/html";
        case ".html": return "text/html";
        case ".ice": return "x-conference/x-cooltalk";
        case ".ief": return "image/ief";
        case ".iges": return "model/iges";
        case ".igs": return "model/iges";
        case ".ips": return "application/x-ipscript";
        case ".ipx": return "application/x-ipix";
        case ".jpe": return "image/jpeg";
        case ".jpeg": return "image/jpeg";
        case ".jpg": return "image/jpeg";
        case ".js": return "application/x-javascript";
        case ".kar": return "audio/midi";
        case ".latex": return "application/x-latex";
        case ".lha": return "application/octet-stream";
        case ".lsp": return "application/x-lisp";
        case ".lzh": return "application/octet-stream";
        case ".m": return "text/plain";
        case ".man": return "application/x-troff-man";
        case ".me": return "application/x-troff-me";
        case ".mesh": return "model/mesh";
        case ".mid": return "audio/midi";
        case ".midi": return "audio/midi";
        case ".mime": return "www/mime";
        case ".mov": return "video/quicktime";
        case ".movie": return "video/x-sgi-movie";
        case ".mp2": return "audio/mpeg";
        case ".mp3": return "audio/mpeg";
        case ".mpe": return "video/mpeg";
        case ".mpeg": return "video/mpeg";
        case ".mpg": return "video/mpeg";
        case ".mpga": return "audio/mpeg";
        case ".ms": return "application/x-troff-ms";
        case ".msh": return "model/mesh";
        case ".nc": return "application/x-netcdf";
        case ".oda": return "application/oda";
        case ".pbm": return "image/x-portable-bitmap";
        case ".pdb": return "chemical/x-pdb";
        case ".pdf": return "application/pdf";
        case ".pgm": return "image/x-portable-graymap";
        case ".pgn": return "application/x-chess-pgn";
        case ".png": return "image/png";
        case ".pnm": return "image/x-portable-anymap";
        case ".pot": return "application/mspowerpoint";
        case ".ppm": return "image/x-portable-pixmap";
        case ".pps": return "application/mspowerpoint";
        case ".ppt": return "application/mspowerpoint";
        case ".ppz": return "application/mspowerpoint";
        case ".pre": return "application/x-freelance";
        case ".prt": return "application/pro_eng";
        case ".ps": return "application/postscript";
        case ".qt": return "video/quicktime";
        case ".ra": return "audio/x-realaudio";
        case ".ram": return "audio/x-pn-realaudio";
        case ".ras": return "image/cmu-raster";
        case ".rgb": return "image/x-rgb";
        case ".rm": return "audio/x-pn-realaudio";
        case ".roff": return "application/x-troff";
        case ".rpm": return "audio/x-pn-realaudio-plugin";
        case ".rtf": return "text/rtf";
        case ".rtx": return "text/richtext";
        case ".scm": return "application/x-lotusscreencam";
        case ".set": return "application/set";
        case ".sgm": return "text/sgml";
        case ".sgml": return "text/sgml";
        case ".sh": return "application/x-sh";
        case ".shar": return "application/x-shar";
        case ".silo": return "model/mesh";
        case ".sit": return "application/x-stuffit";
        case ".skd": return "application/x-koan";
        case ".skm": return "application/x-koan";
        case ".skp": return "application/x-koan";
        case ".skt": return "application/x-koan";
        case ".smi": return "application/smil";
        case ".smil": return "application/smil";
        case ".snd": return "audio/basic";
        case ".sol": return "application/solids";
        case ".spl": return "application/x-futuresplash";
        case ".src": return "application/x-wais-source";
        case ".step": return "application/STEP";
        case ".stl": return "application/SLA";
        case ".stp": return "application/STEP";
        case ".sv4cpio": return "application/x-sv4cpio";
        case ".sv4crc": return "application/x-sv4crc";
        case ".swf": return "application/x-shockwave-flash";
        case ".t": return "application/x-troff";
        case ".tar": return "application/x-tar";
        case ".tcl": return "application/x-tcl";
        case ".tex": return "application/x-tex";
        case ".tif": return "image/tiff";
        case ".tiff": return "image/tiff";
        case ".tr": return "application/x-troff";
        case ".tsi": return "audio/TSP-audio";
        case ".tsp": return "application/dsptype";
        case ".tsv": return "text/tab-separated-values";
        case ".txt": return "text/plain";
        case ".unv": return "application/i-deas";
        case ".ustar": return "application/x-ustar";
        case ".vcd": return "application/x-cdlink";
        case ".vda": return "application/vda";
        case ".vrml": return "model/vrml";
        case ".wav": return "audio/x-wav";
        case ".wrl": return "model/vrml";
        case ".xbm": return "image/x-xbitmap";
        case ".xlc": return "application/vnd.ms-excel";
        case ".xll": return "application/vnd.ms-excel";
        case ".xlm": return "application/vnd.ms-excel";
        case ".xls": return "application/vnd.ms-excel";
        case ".xlw": return "application/vnd.ms-excel";
        case ".xml": return "text/xml";
        case ".xpm": return "image/x-xpixmap";
        case ".xwd": return "image/x-xwindowdump";
        case ".xyz": return "chemical/x-pdb";
        case ".zip": return "application/zip";
        default: return string.Format("application/{0}", extension);
      }
    }

    public static byte[] ImageToByte(Image img)
    {
      ImageConverter converter = new ImageConverter();
      return (byte[])converter.ConvertTo(img, typeof(byte[]));
    }

    public static Stream GetStream(Image img)
    { 
      return GetStream(img, ImageFormat.Png);
    }

    public static Stream GetStream(Image img, ImageFormat format)
    {
      var stream = new MemoryStream();
      img.Save(stream, format);
      stream.Position = 0;
      return stream;
    }

    public static Image ResizeImage(Image imgToResize, Size size)
    {
      int sourceWidth = imgToResize.Width;
      int sourceHeight = imgToResize.Height;

      float nPercent = 0;
      float nPercentW = 0;
      float nPercentH = 0;

      nPercentW = ((float)size.Width / (float)sourceWidth);
      nPercentH = ((float)size.Height / (float)sourceHeight);

      if (nPercentH < nPercentW)
        nPercent = nPercentH;
      else
        nPercent = nPercentW;

      int destWidth = (int)(sourceWidth * nPercent);
      int destHeight = (int)(sourceHeight * nPercent);

      Bitmap b = new Bitmap(destWidth, destHeight);
      Graphics g = Graphics.FromImage((Image)b);
      g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

      g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
      g.Dispose();

      return (Image)b;
    }

    public static Image ResizeImage(Image imgPhoto, int newWidth, int newHeight)
    {
      //Image imgPhoto = Image.FromFile(stPhotoPath);

      int sourceWidth = imgPhoto.Width;
      int sourceHeight = imgPhoto.Height;

      //Consider vertical pics
      if (sourceWidth < sourceHeight)
      {
        int buff = newWidth;

        newWidth = newHeight;
        newHeight = buff;
      }

      int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
      float nPercent = 0, nPercentW = 0, nPercentH = 0;

      nPercentW = ((float)newWidth / (float)sourceWidth);
      nPercentH = ((float)newHeight / (float)sourceHeight);
      if (nPercentH < nPercentW)
      {
        nPercent = nPercentH;
        destX = System.Convert.ToInt16((newWidth -
                  (sourceWidth * nPercent)) / 2);
      }
      else
      {
        nPercent = nPercentW;
        destY = System.Convert.ToInt16((newHeight -
                  (sourceHeight * nPercent)) / 2);
      }

      int destWidth = (int)(sourceWidth * nPercent);
      int destHeight = (int)(sourceHeight * nPercent);


      Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                    PixelFormat.Format24bppRgb);

      bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                   imgPhoto.VerticalResolution);

      Graphics grPhoto = Graphics.FromImage(bmPhoto);
      grPhoto.Clear(Color.Black);
      grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

      grPhoto.DrawImage(imgPhoto,
          new Rectangle(destX, destY, destWidth, destHeight),
          new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
          GraphicsUnit.Pixel);

      grPhoto.Dispose();
      imgPhoto.Dispose();
      return bmPhoto;
    }

    /// <summary>
    /// Function to download Image from website
    /// </summary>
    /// <param name="_URL">URL address to download image</param>
    /// <returns>Image</returns>
    public static Image GetImageFromUrl(string _URL)
    {
      Image _tmpImage = null;

      try
      {
        // Open a connection
        System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL);

        _HttpWebRequest.AllowWriteStreamBuffering = true;

        // You can also specify additional header values like the user agent or the referer: (Optional)
        _HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
        _HttpWebRequest.Referer = "http://www.google.com/";

        // set timeout for 20 seconds (Optional)
        _HttpWebRequest.Timeout = 20000;

        // Request response:
        System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();

        // Open data stream:
        System.IO.Stream _WebStream = _WebResponse.GetResponseStream();

        // convert webstream to image
        _tmpImage = Image.FromStream(_WebStream);

        // Cleanup
        _WebResponse.Close();
        _WebResponse.Close();
      }
      catch (Exception _Exception)
      {
        // Error
        Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
        return null;
      }

      return _tmpImage;
    }
  }
}
