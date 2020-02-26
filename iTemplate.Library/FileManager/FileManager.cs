using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using WebMatrix.WebData;

namespace iTemplate.Library
{
  /// <summary>
  /// Summary description for FileManager
  /// </summary>
  public class FileManager
  {
    public class UploadEventStatus : EventStatus
    {
      public string FileUploaded { get; set; }

      public UploadEventStatus(Exception exception)
      {
        this.Exception = exception;
      }

      public UploadEventStatus(string fileName)
      {
        this.Exception = null;
        this.FileUploaded = fileName;
      }
    }

    const int FILEMASKLOW =  1000000;
    const int FILEMASKHIGH = 1999999;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromPath"></param>
    /// <param name="toPath"></param>
    /// <returns></returns>
    public static EventStatus CopyFile(string fromPath, string toPath)
    {
      if (FileExists(fromPath))
      {
        try
        {
          File.Copy(fromPath, toPath, true);
          return EventStatus.Empty();
        }
        catch (IOException ex)
        {
          return new EventStatus(ex);// We couldn't copy the file.
        }
      }      
      return new EventStatus(new FileNotFoundException("File not found!")); // file doesn't exist
    }

		public static bool CreateDirectory(string dirName)
		{
      if (Directory.Exists(dirName)) { return false; }

			try
			{
				System.IO.Directory.CreateDirectory(dirName);//If directory does not exist then create it
        return true;
      }
			catch (Exception e)
			{
				//LiteralMessage.Text = "Error: Permission denied";
				return false;
			}
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FileName"></param>
    /// <returns></returns>
    public static EventStatus DeleteFile(string FileName)
    {
      if (FileExists(FileName))
      {
        try
        {
          File.Delete(FileName);
          return EventStatus.Empty();
        }
        catch (IOException ex)
        {
          return new EventStatus(ex);// We couldn't delete the file.
        }
      }
      return new EventStatus(new FileNotFoundException("File not found!")); // file doesn't exist
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirName"></param>
    /// <returns></returns>
    public static bool DirectoryExists(string dirName)
    {
      string dirPath = GetDirectoryPath(dirName);
      return Directory.Exists(dirPath);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="FileName"></param>
    /// <returns></returns>
    public static bool FileExists(string FileName)
		{
			return File.Exists(FileName);
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirName"></param>
    /// <returns></returns>
    public static List<string> GetDirectoryFileNames(string dirName)
    {
      List<FileInfo> source = new List<FileInfo>();
      List<string> fNames = new List<string>();

      source = GetDirectoryFiles(dirName);
      foreach (FileInfo fi in source)
      {
        fNames.Add(fi.Name);
      }

      return fNames;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirName"></param>
    /// <returns></returns>
		public static List<FileInfo> GetDirectoryFiles(string dirName)
		{
			List<FileInfo> source = new List<FileInfo>();
			if (DirectoryExists(dirName))
			{
        string dirPath = GetDirectoryPath(dirName);
				DirectoryInfo dirinfo = new DirectoryInfo(dirPath);
				FileInfo[] fis = dirinfo.GetFiles();

				foreach (FileInfo fi in fis)
				{
					if (!fi.Attributes.ToString().Contains("Hidden") && !fi.Attributes.ToString().Contains("System"))
					{
						source.Add(fi);
					}
				}
			}
			return source;
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirName"></param>
    /// <returns></returns>
		public static string GetDirectoryPath(string dirName)
		{
      return HostingEnvironment.MapPath(dirName);
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="SubDirectory"></param>
    /// <returns></returns>
    public static int GetFileCount(string SubDirectory)
    {
      if (!DirectoryExists(SubDirectory)) { return 0; }

      int fileCount = 0;
      string dirname = GetDirectoryPath(SubDirectory);

      //add the number of files in the current directory to the total count
      fileCount += System.IO.Directory.GetFiles(dirname).Length;

      //get all the directories and files inside a directory
      string[] all_subdirs = System.IO.Directory.GetDirectories(dirname);

      ////loop through all dirs and recursively count each dir’s files
      foreach (string dir in all_subdirs)
        fileCount += GetFileCount(dir); //recursive call

      return fileCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFilePath(string fileName)
    {
      try
      {
        string dirName = GetVirtualDirectory(fileName);
        return GetDirectoryPath(dirName);
      }
      catch
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirName"></param>
    /// <returns></returns>
		public static string GetLatestFile(string dirName)
		{
			if (DirectoryExists(dirName))
			{
				if (GetFileCount(dirName) > 0)
				{
          string dirPath = GetDirectoryPath(dirName);
					FileSystemInfo fileInfo = new DirectoryInfo(dirPath).GetFileSystemInfos().OrderByDescending(fi => fi.CreationTime).First();
					return fileInfo.CreationTime.ToUniversalTime().ToString();
				}
				return string.Empty;
			}
			return string.Empty;
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public static string GetVirtualDirectory(string FilePath)
    {
      return VirtualPathUtility.GetDirectory(FilePath).ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public static string GetVirtualFileExtension(string FilePath)
    {
      return VirtualPathUtility.GetExtension(FilePath).ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public static string GetVirtualFileName(string FilePath)
    {
      return VirtualPathUtility.GetFileName(FilePath).ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromPath"></param>
    /// <param name="toPath"></param>
    /// <returns></returns>
    public static EventStatus MoveFile(string fromPath, string toPath)
		{
      if (FileExists(fromPath))
      {
        try
        {
          CreateDirectory(Path.GetDirectoryName(toPath));
          DeleteFile(toPath);
          CopyFile(fromPath, toPath);
          DeleteFile(fromPath);
        }
        catch (IOException ex)
        {
          return new EventStatus(ex);// We couldn't move the file.
        }
      }
      return new EventStatus(new FileNotFoundException("File not found!")); // file doesn't exist
    }

    /// <summary>
    /// Returns a site relative HTTP path from a partial path starting out with a ~.
    /// Same syntax that ASP.Net internally supports but this method can be used
    /// outside of the Page framework.
    /// 
    /// Works like Control.ResolveUrl including support for ~ syntax
    /// but returns an absolute URL.
    /// </summary>
    /// <param name="originalUrl">Any Url including those starting with ~</param>
    /// <returns>relative url</returns>
    public static string ResolveUrl(string originalUrl)
    {
      if (string.IsNullOrEmpty(originalUrl))
        return originalUrl;

      // *** Absolute path - just return
      if (originalUrl.IndexOf("://") != -1)
        return originalUrl;

      // *** Fix up path for ~ root app dir directory
      if (originalUrl.StartsWith("~"))
      {
        // VirtualPathUtility blows up if there is a query string, so we
        // have to account for this.
        int queryStringStartIndex = originalUrl.IndexOf('?');
        if (queryStringStartIndex != -1)
        {
          string queryString = originalUrl.Substring(queryStringStartIndex);
          string baseUrl = originalUrl.Substring(0, queryStringStartIndex);

          return VirtualPathUtility.ToAbsolute(baseUrl) + queryString;
        }
        else
        {
          return VirtualPathUtility.ToAbsolute(originalUrl);
        }
      }

      return originalUrl;
    }

    /// <summary>
    /// This method returns a fully qualified absolute server Url which includes
    /// the protocol, server, port in addition to the server relative Url.
    /// 
    /// Works like Control.ResolveUrl including support for ~ syntax
    /// but returns an absolute URL.
    /// </summary>
    /// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
    /// <param name="forceHttps">if true forces the url to use https</param>
    /// <returns></returns>
    public static string ResolveServerUrl(string serverUrl, bool forceHttps)
    {
      if (string.IsNullOrEmpty(serverUrl))
        return serverUrl;

      // *** Is it already an absolute Url?
      if (serverUrl.IndexOf("://") > -1)
        return serverUrl;

      string newServerUrl = ResolveUrl(serverUrl);
      Uri result = new Uri(HttpContext.Current.Request.Url, newServerUrl);

      if (forceHttps)
      {
        UriBuilder builder = new UriBuilder(result);
        builder.Scheme = Uri.UriSchemeHttps;
        builder.Port = 443;

        result = builder.Uri;
      }

      return result.ToString();
    }

    /// <summary>
    /// This method returns a fully qualified absolute server Url which includes
    /// the protocol, server, port in addition to the server relative Url.
    /// 
    /// It work like Page.ResolveUrl, but adds these to the beginning.
    /// This method is useful for generating Urls for AJAX methods
    /// </summary>
    /// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
    /// <returns></returns>
    public static string ResolveServerUrl(string serverUrl)
    {
      return ResolveServerUrl(serverUrl, false);
    }

    public static UploadEventStatus UploadFile(System.Web.UI.WebControls.FileUpload FileUploader, string folderPath = "~/App_Data", int MaxSize = 5120000)
    {
      string extension = Path.GetExtension(FileUploader.PostedFile.FileName);

      if (extension.Length<1)
      { return new UploadEventStatus(new Exception("Error!:  Invalid file extension")); }

      if (FileUploader.HasFile == false)
      { return new UploadEventStatus(new Exception("Error!:  No file has been selected")); }

      if (FileUploader.PostedFile == null)
      { return new UploadEventStatus(new Exception("Error!:  Invalid file name")); }

      if (FileUploader.PostedFile.ContentLength < 1)
      { return new UploadEventStatus(new Exception("Error!: Invalid file size")); }

      if (FileUploader.FileBytes.Length > (MaxSize))
      { return new UploadEventStatus(new Exception(string.Format("Error: File is too large - Max size is {0}Mb", MaxSize / (1024 * 1024)))); }

      string webRootPath = HttpContext.Current.Server.MapPath(folderPath);
      string docPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(webRootPath, WebSecurity.CurrentUserId.ToString()));
      docPath = docPath + string.Format("\\{0}", extension.Substring(1));

      //If directory does not exist then create it - however it should always exist prior to upload!
      // Format of path is:   App_Data/Name/txt/
      if (!DirectoryExists(docPath))
      {
        try
        {
          CreateDirectory(docPath);
        }
        catch
        {
          return new UploadEventStatus(new Exception("Error: Permission denied"));
        }
      }

      //Get the next sequential filename in directory
      //int newFile = FILEMASKLOW;
      //string latestFile = GetLatestFile(docPath);
      //if (!String.IsNullOrEmpty(latestFile))
      //{
      //  newFile = Convert.ToInt32(latestFile.Substring(0, latestFile.IndexOf(".") - 1));
      //}

      //newFile++;
      //if (newFile > FILEMASKHIGH) { return new UploadEventStatus(new Exception("Error: File upload limit reached")); }

      var newFile = Guid.NewGuid();


      /**********************************************************************************************************************
       * Save the file....
       *********************************************************************************************************************/
      try
      {
        string newFileName = newFile.ToString() + extension;
        string docFileName = string.Format("{0}\\{1}", docPath, newFileName);
        FileUploader.PostedFile.SaveAs(docFileName);
        return new UploadEventStatus(newFileName);
      }
      catch
      {
        return new UploadEventStatus(new Exception("Error: Permission denied"));
      }
    }
  }
}