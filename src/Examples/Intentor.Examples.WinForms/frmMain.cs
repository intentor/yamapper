using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Intentor.Examples.Facade;
using System.Reflection;

namespace Intentor.Examples.WinForms
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            /* NOTAS
             * - Todos os arquivos de mapeamento devem estar como Embedded Resource;
             * - Recomenda-se a prévia criação dos mapeamentos durante a inicialização
             *   da aplicação diretamente no método Main a partir da chamada ao método
             *   DbProviderFactory.BuildMappings();
             * - O arquivo App.config deve conter todas as configurações necessárias
             *   para utilização do Yamapper a partir da section YamapperConfigurationSection;
             * - O atributo mappingAssembly da section deve ser configurado com o 
             *   full qualified name do assembly aonde os mapeamentos estão como arquivos
             *   de recurso.
             */
            var clientes = ProdutosFacade.GetClientes();
        }
    }
}
