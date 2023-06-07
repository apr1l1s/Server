using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    public class Reply
    {
        public Reply(ErrorHandler.StatusType status, string content)
        {
            this.status = status;
            headers.Add("Date: ", GetDate());
            headers.Add("Server: ", "apr1l1s");
            if (content != "")
            {
                headers.Add("Content-Type: ", ContentTypeToString(ContentType.html));
                headers.Add("Content-Length: ", content.Length.ToString());
                this.content = content;
            }
            headers.Add("Access-Control-Allow-Methods: ", "*");
            headers.Add("Access-Control-Allow-Headers: ", "*");
            headers.Add("Access-Control-Allow-Origin: ", "*");
        }
        enum ContentType {
            json,html
        }
        ErrorHandler.StatusType status;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string content;
        string GetDate()
        {
            return DateTime.Now.ToString();
        }
        string ContentTypeToString(ContentType type)
        {
            switch (type)
            {
                case ContentType.html:
                    return "text/html";
                case ContentType.json:
                    return "application/json";
                default:
                    return "text/html";
            }
        }
        string StatusToString(ErrorHandler.StatusType status)
        {
            switch (status)
            {
                case ErrorHandler.StatusType.ok:
                    return "HTTP/1.0 200 OK\n";
                case ErrorHandler.StatusType.accepted:
                    return "HTTP/1.0 202 Accepted\n";
                case ErrorHandler.StatusType.no_content:
                    return "HTTP/1.0 204 No Content\n";
                case ErrorHandler.StatusType.bad_request:
                    return "HTTP/1.0 400 Bad Request\n";
                case ErrorHandler.StatusType.unauthorized:
                    return "HTTP/1.0 401 Unauthorized\n";
                case ErrorHandler.StatusType.not_found:
                    return "HTTP/1.0 404 Not Found\n";
                case ErrorHandler.StatusType.internal_server_error:
                    return "HTTP/1.0 500 Internal Server Error\n";
                default:
                    return "HTTP/1.0 500 Internal Server Error\n";
            }
        }
        public override string ToString()
        {
            string crlf = "\n";
            string message="";
            message += StatusToString(status);
            var hs = headers.Select(headers => headers.Key + headers.Value+crlf).ToList();
            hs.ForEach(h => message += h);
            message += crlf + content;
            return message;
        }
    }
}
