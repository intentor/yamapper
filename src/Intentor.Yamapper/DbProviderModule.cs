/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Web;

namespace Intentor.Yamapper {
    /// <summary>
    ///     Módulo para gerenciamento de conexões.
    /// </summary>
    public class DbProviderModule :
        IHttpModule {
        /// <summary>
        ///     Inicializa o módulo.
        /// </summary>
        public void Init(HttpApplication context) {
            context.BeginRequest += new EventHandler(BeginRequest);
            context.EndRequest += new EventHandler(EndRequest);
        }

        /// <summary>
        ///     Handler para início de requisição HTTP.
        /// </summary>
        private void BeginRequest(object sender, EventArgs e) {
            //Cria todos os provedores.
            DbProviderFactory.CreateProviders();
        }

        /// <summary>
        ///     Handler para término de requisição HTTP.
        /// </summary>
        private void EndRequest(object sender, EventArgs e) {
            //Descarta todos os provedores.
            DbProviderFactory.DisposeProviders();
        }

        /// <summary>
        ///     Descarte do módulo.
        /// </summary>
        public void Dispose() { }
    }
}
