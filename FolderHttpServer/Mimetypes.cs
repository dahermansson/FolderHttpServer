using System;
using System.Collections.Generic;

namespace FolderHttpServer
{
  public static class Mimetypes
  {
    private static Dictionary<string, string> _mimetypes = new Dictionary<string, string>(){
        {".html", "text/html"},
        {".css", "text/css"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".png", "image/png"},
        {".gif", "image/gif"},
        {".js", "application/javascript"},
        {".eot", "application/vnd.ms-fontobject"},
        {".ico", "image/x-icon"}
      };
    
    public static string GetMimetype(string fileExtension)
    {
      if (_mimetypes.ContainsKey(fileExtension))
        return _mimetypes[fileExtension];
      else
        return "application/octet-stream";
    }

  }
}
