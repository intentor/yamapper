# Changelog for Yamapper

Changelog details in Brazilian Portuguese.

## 12.6.18.1905 (18/06/2012)
- Exclus�o de chamada a m�todos Dispose() de objetos IDbCommand em m�todos do DbProvider que o recebem como par�metro.

## 12.6.11.2046 (11/06/2012)
- Acr�scimo de driver do ODP.Net;
- Acr�scimo de verifica��o de possibilidade de escrita em propriedades nos m�todos BindTo;
- Ajustes no script do MyGeneration para gera��o de entidades custom.
	
## 12.2.22.1314 (22/02/2012)
- BindTo retorna nulo caso algum valor seja vazio.
	
## 12.1.19.1149 (19/01/2012)
- Acr�scimo de provider para o PostgreSQL.

## 12.1.13.1515 (13/01/2012)
- Transforma��o da classe abstrata BaseExpression na interface IBaseExpression;
- Atualiza��o de vers�o do Intentor.Utilities para 12.1.12.1357.
	
## 12.1.2.1043 (02/01/2012)
- Acr�scimo do m�todo GetConnectionByName em YamapperConfigurationSection;
- Ajustes no script do MyGeneration para apenas salvar custom objects caso estes n�o existam no local de destino.

## 11.11.2.2006 (02/11/2011)
- Acr�scimo de Extension Method BindTo para c�pia de propriedades entre uma classe e outra.

## 11.10.28.1325 (28/10/2011)
- Corre��o de problema que causava retorno nulo no m�todo Update;
- Acr�scimo de checagem de exist�ncia de barra ("\") no caminho de salvamento de arquivos no script do MyGeneration.

## 11.10.17.1454 (17/10/2011)
	 - Acr�scimo de AlternateText nos ImageButton do Grid gerado com os WebForms para facilitar entendimento das a��es;
	 - Acr�scimo de sobrecarga para o m�todo BindTo permitindo que o objeto possa ser copiado para uma nova inst�ncia de mesmo tipo, sem necessidade de um Data Transfer Object;
	 - Exclus�o da obriga��o de heran�a de MappingEntity para entidades;
	 - Ajustes no script do MyGeneration para n�o utiliza��o de heran�a de MappingEntity nas entidades;
	 - Ajustes no exemplo de uso de GridView com pagina��o de Intentor.Examples.Web;
	 - Organiza��o do projeto Intentor.Examples.Model para refletir as �ltimas mudan�as na arquitetura.

## 11.10.14.1619 (14/10/2011)
- Atualiza��o de vers�o do Intentor.Utilities para 11.10.11.1412;
- Organiza��o do projeto de exemplo para agrupamento de todas as camadas de dados em um �nico projeto;
- Retirada dos campos de valida��o utilizados em Mapping Entity;
- No script do MyGeneration:
	- Ajustes na nomenclatura de interface;
	- Uso de Data Annotations para valida��o das entidades;
	- Acr�scimo de gera��o de WebForms;
	- Acr�scimo de possibilidade de indica��o dos diret�rios para gera��o dos mapeamentos, classes do modelo e WebForms;
	- Acr�scimo de arquivos de configura��o externo para os par�metros do gerador

## 11.9.2.1004 (02/09/2011)
- Ajustes na obten��o de dados de sequence (para uso com bancos de dados Oracle);
- Cria��o do m�todo SetPropertyValue em DbMapperHelper.

## 11.8.20.2208 (20/08/2011)
- Acr�scimo de sobrecarga ao m�todo ChangePassword de YamapperMembershipProvider para permitir a inser��o direta de uma nova senha sem necessidade de digita��o de senha anterior;
- Corre��o de problema com coloca��o de dois pontos ao final do nomes dos custom objects de Dao no gerador de c�digo do MyGeneration;
- Altera��o do script de gera��o de objetos para permitir cria��o de prefixo para nomes de objetos;
- Atualiza��o da biblioteca Intentor.Utilities para a vers�o 11.8.20.2122.

## 11.8.1.1344 (01/08/2011)
- Acr�scimo de operador NOT LIKE;
- Acr�scimo de avalia��o se os mapeamentos est�o criados quando da obten��o de provider;
- Atualiza��o de vers�o da biblioteca Intentor.Utilities para 11.8.1.1330.

## 11.5.20.1406 (20/05/2011)
- Corre��o de problema que impedia a decripta��o de strings de conex�o criptografadas;
- Adapta��es na gera��o de mapeamento para permitir leitura de arquivos .cmf como resources de um assembly (para uso em projetos Windows Form);
- Acr�scimo de projeto exemplo para Windows Forms;
- Acr�scimo de MembershipProvider do Yamapper, YamapperMembershipProvider (nem todos os m�todos est�o implementados);
- Altera��o do script de gera��o de objetos para gera��o dos diret�rios de objetos Generated (para objetos que contenham procedimentos gerados automaticamente) e Custom (para objetos que podem conter customiza��o do usu�rio);
- Altera��o do script de gera��o de objetos para retorno de GetById em objetos Dao ser NULL caso nenhum dado seja encontrado;
- Altera��o do script de gera��o de objetos para permitir identifica��o opcional de conex�o com o banco de dados para objetos.

## 11.2.10.1111 (10/02/2011)
- Compila��o de vers�o para o .NET 4.0 Full Profile;
- Substitui��o do Castle.DynamicProxy pela vers�o mais recente presente em Castle.Core;
- Corre��o de problema com preenchimento de propriedades via lazy loading mesmo ap�s a defini��o de um valor;
- Gerador: corre��o de problema no com Design by Contract de Strings de valores nulos;
- Melhorias nos coment�rios dos schemas dos arquivos de configura��o e mapeamento.

## 11.1.27.1703 (27/01/2011)
- Corre��o de operador "menor ou igual" na express�o SimpleExpression;
- Cria��o de campo para armazenamento dos campos analisados durante lazy loading para evitar problemas com an�lise de valye types vazios.

## 11.1.12.1042 (12/01/2011)
- Corre��o de problema envolvendo convers�o de valores bin�rios quando da atualiza��o de objetos.

## 11.1.3.1121 (03/01/2011)
- Corre��o de problema envolvendo convers�o de valores bin�rios quando da inser��o de objetos.

## 10.12.19.1516 (19/12/2010)
- Acr�scimo de mensagem indicando se um mapeamento solicitado foi efetivamente localizado;
- Corre��o de problema com avalia��o de valores nulos de banco de dados quando da obten��o de dados via lazy loading.

## 10.11.16.1345 (16/11/2010)
- Corre��o de problema com driver do MySQL envolvendo pagina��o;
- Cria��o do par�metro "defaultLimit" para indica��o do valor padr�o de Limit utilizado em pagina��es no driver do ## MySQL;
- Cria��o de valida��o de conex�es obtidas em DbProviderFactory;
- Atualiza��o de vers�o padr�o do assembly do provider do MySQL para 6.3.5.

## 10.11.3.1604 (03/11/2010)
- Corre��o de problema com nomes de par�metros repetidos na express�o BETWEEN.

## 10.10.25.1138 (25/10/2010)
- Atualiza��es para suporte ao Intentor.Utilities 10.10.25.1130.

## 10.10.22.1412 (22/10/2010)
- Acr�scimo de gera��o arquivos customiz�veis de DataInterfaces, Dao e Biz (agora h� separa��o entre objetos do modelo gerados automaticamente e customizados pelo usu�rio);
- Acr�scimo de express�es IN e NOT IN em objetos Criteria;
- Acr�scimo de agrupamento de express�es por AND e OR em objetos Criteria;
- Acr�scimo de express�o de instru��es SQL em objetos Criteria, com uso do curinga {param} para indica��o do local aonde estar� o par�metro;
- Acr�scimo de possibilidade de limita��o/pagina��o de registros atrav�s dos m�todos Offset e Limit inclu�dos na classe Criteria;
- Acr�scimo de m�todo ExecuteScalar n�o gen�rico;
- Altera��es internas no mapeador para permitir chaves compostas, lazy loading e limita��o de registros;
- Altera��o do m�todo Create de ICommonDataBaseActions para n�o retornar dados (chaves prim�rias s�o preenchidas diretamente no objeto enviado);
- Altera��o do gerador de c�digo para refletir as mudan�as de m�todos;
- Altera��es no schema de mapeamento para inclus�o de indica��o de chave de autonumera��o, lazy loading (para uso em vers�o futura) e coloca��o de sequence sobre campo, n�o mais tabela;
- Retirada do m�todo Fill no provedor;
- Retirada dos m�todos Get com sobrecargas de instru��o SQL e objetos IDbCommand;
- Retirada do m�todo GetById no provedor. Tal m�todo ser� gerado automaticamente dentro de cada Dao de acordo com a(s) chave(s) prim�ria(s);
- Retirada do m�todo Delete por ID no provedor. Tal m�todo ser� gerado automaticamente em cada Dao de acordo com a(s) chave(s) prim�ria(s);
- Retirada dos m�todos GetOf e GetListOf. As a��es de tais m�todos s�o executadas a partir de sobrecargas do m�todo Get;
- Retirada dos m�todos GetById e Delete com ID de ICommonDataBaseActions;
- Retirada da necessidade de informa��o do tipo do campo PK de ICommonDataBaseActions;
- Retirada do driver de acesso ao banco de dados do Access por baixa utiliza��o;
- Durante inclus�o, somente s�o ignorados campos que sejam de autonumera��o ou marcados para tal;
- O m�todo Insert n�o mais retorna chave prim�ria gerada; ao inv�s disso, preenche seus valores diretamente na entidade.

## 10.8.22.1511 (22/08/2010)
- Os m�todos de Insert n�o necessitam mais de passagem de par�metros por refer�ncia;
- Cria��o de m�todo para busca por crit�rios;
- Cria��o de m�todo para dele��o com crit�rios;
- Cria��o de m�todo Clone em Criteria a partir da implementa��o da interface ICloneable;
- Cria��o da interface IViewDataBaseActions para representa��o do contrato de uma View;
- A interface ICommonDataBaseActions passa a implementar a inerface IViewDataBaseActions;
- Melhorias na valida��o de arquivos de mapeamento e configura��o;
- Adapta��es e corre��es diversas no gerador de c�digo do MyGeneration.

## 10.8.12.1529 (12/08/2010)
- Corre��o do m�todo MapToSingle para realiza��o de leitura do DataReader antes de seu preenchimento;
- Acr�scimo dos m�todos Exists (para verifica��o de exist�ncia de registros) e Count (para contagem de registros) a partir de crit�rios.

## 10.4.1.1651 (01/04/2010)
- Gerador: corre��o de problemas com falta de aspas na gera��o de mapeamento externo;
- Gerador: acr�scimo da possibilidade de sele��o de inclus�o dos namespaces dos TransferObjects e Views;
- Cria��o dos m�todos ASC e DESC na classe Order para simplificar a cria��o de crit�rios de ordena��o;
- Acr�scimo de possibilidade de envio de par�metros ao provedor de acesso ao banco de dados a partir do atributo "params" da tag "connection", com cada driver podendo possuir seu pr�prio conjunto de par�metros;
- Atualiza��o de projeto exemplo enviado junto do c�digo-fonte para exemplificar o uso de ordena��o e obten��o de dados a partir de consultas SQL customizadas.