using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Intentor.Yamapper;

namespace Intentor.Examples.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Constrói os mapeamentos na inicialização da aplicação.
            DbProviderFactory.BuildMappings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
