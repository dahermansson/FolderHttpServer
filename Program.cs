using System.Net;

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
    HttpListenerRequest request = context.Request ?? throw new NullReferenceException(nameof(context.Request));
    Uri requestUrl = request.Url ?? throw new NullReferenceException(nameof(context.Request.Url));

    Console.WriteLine(requestUrl.AbsolutePath);

    HttpListenerResponse response = context.Response;

    var fileName = requestUrl.AbsolutePath.TrimStart('/');
    string filePath = Path.Combine(path, fileName);
    byte[] buffer;
    string filetype = string.Empty;
    if ( File.Exists(filePath))
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
    output.Close();
}

listener.Stop();