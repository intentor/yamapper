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
using System.Reflection;
using Castle.DynamicProxy;
using Intentor.Utilities;
using System.Data;

namespace Intentor.Yamapper.Proxy {
    /// <summary>
    /// Interceptador para preenchimento de propriedades.
    /// </summary>
    public class PropertyInterceptor : IInterceptor {
        public void Intercept(IInvocation invocation) {
            //Executa o método.
            invocation.Proceed();

            //Obtém o nome do método.
            string name = invocation.Method.Name;

            //Caso seja um método GET, intercepta-o para popular os dados.
            if (name.StartsWith("get_")) {
                object currentValue = invocation.ReturnValue;

                //Caso o valor seja NULL ou o valor padrão do objeto, captura o dado do banco de dados.
                if (currentValue.IsNullOrDbNull() || currentValue.Equals(this.GetDefaultValue(currentValue.GetType()))) {
                    //Obtém o nome da propriedade para obtenção do nome do campo no banco de dados.
                    string propertyName = name.Substring(4);

                    //Obtém a query para obtenção do valor.
                    TableMapping tb = DbMapperHelper.Mappings[invocation.TargetType.FullName];
                    DbProvider provider = DbProviderFactory.GetProvider(tb.ConnectionName);
                    string sql = DbMapperQueryBuilder.CreateSelectForProperty(tb, propertyName, provider.DriverFactory.ParameterPrefix);

                    //Valor de retorno
                    object value = null;

                    //Obtém o valor da propriedade.
                    using (IDbCommand cmd = provider.CreateCommandForText(sql)) {
                        #region População da(s) chave(s) primária(s)

                        var pks = tb.GetPrimaryKeys();
                        Check.Require(pks.Count > 0, String.Format(Messages.PrimaryKeyNotFoundEntity, tb.EntityName));
                        foreach (var pk in pks) cmd.CreateParameter(pk.PropertyName, DbMapperHelper.GetPropertyValue(invocation.InvocationTarget, invocation.TargetType, pk));

                        #endregion

                        value = provider.ExecuteScalar(cmd);

                        //Caso o valor seja DBNull, define-o como NULO.
                        if (value == DBNull.Value) value = null;
                    }

                    //Invoca o método SET para preencher o objeto.
                    MethodInfo set = invocation.TargetType.GetMethod("set_" + propertyName);
                    set.Invoke(invocation.InvocationTarget, new object[] { value });

                    //Retorna o valor obtido.
                    invocation.ReturnValue = value;
                }
            }
        }

        /// <summary>
        /// Obtém o valor padrão de um determinado tipo.
        /// </summary>
        /// <param name="t">Tipo a ter o valor padrão obtido.</param>
        /// <returns>Valor padrão do tipo.</returns>
        private object GetDefaultValue(Type t) {
            if (!t.IsValueType)
                return null;
            else
                return Activator.CreateInstance(t);
        }
    }
}
