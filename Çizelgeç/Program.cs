// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Çizelgeç
{
    static class Program
    {
        static string pak = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Trim('\\') + "\\"; //programanaklasör

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            S.BaşlangıçParametreleri = args;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            Directory.CreateDirectory(pak + "Kutuphane");

            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.Microsoft_Bcl_AsyncInterfaces, "Microsoft.Bcl.AsyncInterfaces.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.ScottPlot, "ScottPlot.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.ScottPlot_WinForms, "ScottPlot.WinForms.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Buffers, "System.Buffers.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Drawing_Common, "System.Drawing.Common.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Memory, "System.Memory.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Numerics_Vectors, "System.Numerics.Vectors.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Runtime_CompilerServices_Unsafe, "System.Runtime.CompilerServices.Unsafe.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Text_Encodings_Web, "System.Text.Encodings.Web.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Text_Json, "System.Text.Json.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Threading_Tasks_Extensions, "System.Threading.Tasks.Extensions.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_ValueTuple, "System.ValueTuple.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.Microsoft_CodeAnalysis_Scripting, "Microsoft.CodeAnalysis.Scripting.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.Microsoft_CodeAnalysis, "Microsoft.CodeAnalysis.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.Microsoft_CodeAnalysis_CSharp_Scripting, "Microsoft.CodeAnalysis.CSharp.Scripting.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.Microsoft_CodeAnalysis_CSharp, "Microsoft.CodeAnalysis.CSharp.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Collections_Immutable, "System.Collections.Immutable.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Reflection_Metadata, "System.Reflection.Metadata.dll");
            AnaEkran_GerekliDosyaKontrolü(Properties.Resources.System_Text_Encoding_CodePages, "System.Text.Encoding.CodePages.dll");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AnaEkran());
        }
        private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string strTempAssmbPath = pak + "Kutuphane\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
            if (!File.Exists(strTempAssmbPath)) return null;
            return Assembly.LoadFrom(strTempAssmbPath);
        }
        static int AnaEkran_GerekliDosyaKontrolü(byte[] Kaynak, string AsılDosyaAdı)
        {
            try
            {
                if (!File.Exists(pak + "Kutuphane\\" + AsılDosyaAdı)) { File.WriteAllBytes(pak + "Kutuphane\\" + AsılDosyaAdı, Kaynak); return 1; }

                File.WriteAllBytes("GerekliDosya.Gecici", Kaynak);
                if (System.Diagnostics.FileVersionInfo.GetVersionInfo("GerekliDosya.Gecici").FileVersion != System.Diagnostics.FileVersionInfo.GetVersionInfo(pak + "Kutuphane\\" + AsılDosyaAdı).FileVersion ||
                    new FileInfo("GerekliDosya.Gecici").Length != new FileInfo(pak + "Kutuphane\\" + AsılDosyaAdı).Length ||
                    new FileInfo("GerekliDosya.Gecici").LastWriteTime != new FileInfo(pak + "Kutuphane\\" + AsılDosyaAdı).LastWriteTime)
                {
                    File.Delete(pak + "Kutuphane\\" + AsılDosyaAdı);
                    File.Move("GerekliDosya.Gecici", pak + "Kutuphane\\" + AsılDosyaAdı);
                    return 2;
                }

                File.Delete("GerekliDosya.Gecici");
            }
            catch (Exception) { }

            return 0;
        }
    }
}
