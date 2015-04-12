using System.Web;

namespace KladrTask.WebUI
{
    public static class ResponseExtensions
    {
        public static void SaveFile(this HttpResponseBase response, string fileName, string text)
        {
            response.Clear();
            response.Buffer = true;
            response.AddHeader("content-disposition", "attachment;filename="+fileName);
            response.Charset = "";
            response.ContentType = "application/text";
            response.Output.Write(text);
            response.Flush();
            response.End();
        }
    }
}