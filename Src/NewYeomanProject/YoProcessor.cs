using System.Diagnostics;

namespace CommonYo
{
    public class YoProcessor
    {
        public void Generate()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                FileName = "Yo",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal, 
            };

            ////////////////////////////// No need for a try/catch here - any exceptions are caught by the calling UserForm and displayed in the dialog to user
            using (var process = Process.Start(processStartInfo)) { }
        }
    }
}