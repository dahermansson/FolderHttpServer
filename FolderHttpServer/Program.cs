using System;
using System.IO;
using System.Net;

namespace FolderHttpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = string.Empty;
            int port = 1234;

            if (args.Length > 0)
                path = args[0];
            if (args.Length > 1)
                int.TryParse(args[1], out port);
            string hostName = string.Format("http://localhost:{0}/", port);
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(hostName);
            listener.Start();
            Console.WriteLine("Listening... ");
            Console.WriteLine("Open {0} in a browser", hostName);
            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                Console.WriteLine(request.Url.AbsolutePath);
                HttpListenerResponse response = context.Response;
                string filePath = Path.Combine(path, request.Url.AbsolutePath.TrimStart('/'));
                byte[] buffer;
                string filetype = string.Empty;
                if (File.Exists(filePath))
                {
                    buffer = File.ReadAllBytes(filePath);
                    filetype = Path.GetExtension(filePath);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = Mimetypes.GetMimetype(filetype);
                    response.StatusCode = 200;
                }
                else
                {
                    buffer = System.Text.Encoding.UTF8.GetBytes(Page404.Page404Html);
                    response.ContentType = Mimetypes.GetMimetype(".html");
                    response.StatusCode = 404;
                }

                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                buffer = null;
                output.Close();
            }
            listener.Stop();
        }
    }
}
