using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CallerIdIntegration
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_DispatcherUnhandledException_1(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogAndDisplayException(e.Exception);
            e.Handled = true;
        }

        private static void LogAndDisplayException(Exception exception)
        {
            FileStream fs = new FileStream(@"C:\ErrorLogCallerID.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sm = new StreamWriter(fs);
            sm.BaseStream.Seek(0, SeekOrigin.End);
            StringBuilder objBulider = new StringBuilder();
            objBulider.Append("/*****************************" + DateTime.UtcNow + "******************************/" + Environment.NewLine + exception + Environment.NewLine);
            sm.WriteLine(objBulider);
            sm.Flush();
            sm.Close();
        }


    }
}
