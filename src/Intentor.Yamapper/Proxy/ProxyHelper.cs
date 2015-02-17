/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Intentor.Yamapper.Proxy {
    /// <summary>
    /// Classe de apoio na utilização de objetos proxy.
    /// </summary>
    public static class ProxyHelper {
        /// <summary>
        /// Cria uma entidade proxy a partir de um tipo.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade a ser gerado.</typeparam>
        /// <returns>Entidade proxy gerada.</returns>
        public static T CreateProxyForEntity<T>()
            where T : class {
            ProxyGenerator generator = new ProxyGenerator();
            T obj = generator.CreateClassProxy<T>(new PropertyInterceptor());
            return obj;
        }
    }
}
