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
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Gera expressões de acesso ao banco de dados.
    /// </summary>
    public static class Expression {
        /// <summary>
        /// Conjunto de expressões de comparação AND.
        /// </summary>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression And(params IExpression[] expressions) {
            return new AndExpression(expressions);
        }

        /// <summary>
        /// Conjunto de expressões de comparação OR.
        /// </summary>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression Or(params IExpression[] expressions) {
            return new OrExpression(expressions);
        }

        /// <summary>
        /// Expressão de igualdade.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression Equal(string propertyName, object value) {
            return new SimpleExpression(propertyName
                , CommonOperator.Equal
                , value);
        }

        /// <summary>
        /// Expressão de desigualdade.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression NotEqual(string propertyName, object value) {
            return new SimpleExpression(propertyName
                , CommonOperator.NotEqual
                , value);
        }

        /// <summary>
        /// Expressão de maior.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression GreaterThan(string propertyName, object value) {
            return new SimpleExpression(propertyName
                , CommonOperator.GreaterThan
                , value);
        }

        /// <summary>
        /// Expressão de maior ou igual.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression GreaterOrEqual(string propertyName, object value) {
            return new SimpleExpression(propertyName
                , CommonOperator.GreaterOrEqual
                , value);
        }

        /// <summary>
        /// Expressão de menor.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression LesserThan(string propertyName, object value) {
            return new SimpleExpression(propertyName
                , CommonOperator.LesserThan
                , value);
        }

        /// <summary>
        /// Expressão de menor ou igual.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression LesserOrEqual(string propertyName, object value) {
            return new SimpleExpression(propertyName
                , CommonOperator.LesserOrEqual
                , value);
        }

        /// <summary>
        /// Expressão de verificação de um campo nulo.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression IsNull(string propertyName) {
            return new SimpleExpression(propertyName
                , CommonOperator.IsNull);
        }

        /// <summary>
        /// Expressão de verificação de um campo não nulo.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression IsNotNull(string propertyName) {
            return new SimpleExpression(propertyName
                , CommonOperator.IsNotNull);
        }

        /// <summary>
        /// Expressão de comparação por parte de string.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression Like(string propertyName, string value) {
            return new SimpleExpression(propertyName
                , CommonOperator.Like
                , value);
        }

        /// <summary>
        /// Expressão de comparação por parte de string de forma negativa.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value">Valor a ser comparado.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression NotLike(string propertyName, string value) {
            return new SimpleExpression(propertyName
                , CommonOperator.NotLike
                , value);
        }

        /// <summary>
        /// Expressão de comparação entre dois valores.  
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="value1">Valor à esquerda da expressão.</param>
        /// <param name="value2">Valor à direita da expressão.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression Between(string propertyName, object value1, object value2) {
            return new SimpleExpression(propertyName
                , CommonOperator.Between
                , value1
                , value2);
        }

        /// <summary>
        /// Expressão para comparação de um conjunto de parâmetros.
        /// </summary>
        /// <typeparam name="T">Tipo dos dados a serem avaliados.</typeparam>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="values">Valores a serem comparados.</param>
        /// <remarks>Os valores devem ser tipos primitivos.</remarks>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression In<T>(string propertyName, T[] values) {
            return new SimpleExpression(propertyName
                , CommonOperator.In
                , values);
        }

        /// <summary>
        /// Expressão para comparação de um conjunto de parâmetros de forma negativa.
        /// </summary>
        /// <typeparam name="T">Tipo dos dados a serem avaliados.</typeparam>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="values">Valores a serem comparados.</param>
        /// <remarks>Os valores devem ser tipos primitivos.</remarks>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression NotIn<T>(string propertyName, T[] values) {
            return new SimpleExpression(propertyName
                , CommonOperator.NotIn
                , values);
        }

        /// <summary>
        /// Expressão SQL customizada.
        /// </summary>
        /// <param name="sql">Instrução SQL.</param>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression Sql(string sql) {
            return new SqlExpression(sql);
        }

        /// <summary>
        /// Expressão SQL customizada.
        /// </summary>
        /// <param name="sql">Instrução SQL.</param>
        /// <param name="value">Valor utilizado na instrução.</param>
        /// <remarks>Utilizar o curinga {param} para indicação do ponto de substituição do nome do parâmetro.</remarks>
        /// <returns>Objeto <see cref="IExpression"/>.</returns>
        public static IExpression Sql(string sql, object value) {
            return new SqlExpression(sql, value);
        }
    }
}
