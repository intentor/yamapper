# Changelog for Yamapper

Changelog details in Brazilian Portuguese.

## 12.6.18.1905 (18/06/2012)
- Exclusão de chamada a métodos Dispose() de objetos IDbCommand em métodos do DbProvider que o recebem como parâmetro.

## 12.6.11.2046 (11/06/2012)
- Acréscimo de driver do ODP.Net;
- Acréscimo de verificação de possibilidade de escrita em propriedades nos métodos BindTo;
- Ajustes no script do MyGeneration para geração de entidades custom.
	
## 12.2.22.1314 (22/02/2012)
- BindTo retorna nulo caso algum valor seja vazio.
	
## 12.1.19.1149 (19/01/2012)
- Acréscimo de provider para o PostgreSQL.

## 12.1.13.1515 (13/01/2012)
- Transformação da classe abstrata BaseExpression na interface IBaseExpression;
- Atualização de versão do Intentor.Utilities para 12.1.12.1357.
	
## 12.1.2.1043 (02/01/2012)
- Acréscimo do método GetConnectionByName em YamapperConfigurationSection;
- Ajustes no script do MyGeneration para apenas salvar custom objects caso estes não existam no local de destino.

## 11.11.2.2006 (02/11/2011)
- Acréscimo de Extension Method BindTo para cópia de propriedades entre uma classe e outra.

## 11.10.28.1325 (28/10/2011)
- Correção de problema que causava retorno nulo no método Update;
- Acréscimo de checagem de existência de barra ("\") no caminho de salvamento de arquivos no script do MyGeneration.

## 11.10.17.1454 (17/10/2011)
	 - Acréscimo de AlternateText nos ImageButton do Grid gerado com os WebForms para facilitar entendimento das ações;
	 - Acréscimo de sobrecarga para o método BindTo permitindo que o objeto possa ser copiado para uma nova instância de mesmo tipo, sem necessidade de um Data Transfer Object;
	 - Exclusão da obrigação de herança de MappingEntity para entidades;
	 - Ajustes no script do MyGeneration para não utilização de herança de MappingEntity nas entidades;
	 - Ajustes no exemplo de uso de GridView com paginação de Intentor.Examples.Web;
	 - Organização do projeto Intentor.Examples.Model para refletir as últimas mudanças na arquitetura.

## 11.10.14.1619 (14/10/2011)
- Atualização de versão do Intentor.Utilities para 11.10.11.1412;
- Organização do projeto de exemplo para agrupamento de todas as camadas de dados em um único projeto;
- Retirada dos campos de validação utilizados em Mapping Entity;
- No script do MyGeneration:
	- Ajustes na nomenclatura de interface;
	- Uso de Data Annotations para validação das entidades;
	- Acréscimo de geração de WebForms;
	- Acréscimo de possibilidade de indicação dos diretórios para geração dos mapeamentos, classes do modelo e WebForms;
	- Acréscimo de arquivos de configuração externo para os parâmetros do gerador

## 11.9.2.1004 (02/09/2011)
- Ajustes na obtenção de dados de sequence (para uso com bancos de dados Oracle);
- Criação do método SetPropertyValue em DbMapperHelper.

## 11.8.20.2208 (20/08/2011)
- Acréscimo de sobrecarga ao método ChangePassword de YamapperMembershipProvider para permitir a inserção direta de uma nova senha sem necessidade de digitação de senha anterior;
- Correção de problema com colocação de dois pontos ao final do nomes dos custom objects de Dao no gerador de código do MyGeneration;
- Alteração do script de geração de objetos para permitir criação de prefixo para nomes de objetos;
- Atualização da biblioteca Intentor.Utilities para a versão 11.8.20.2122.

## 11.8.1.1344 (01/08/2011)
- Acréscimo de operador NOT LIKE;
- Acréscimo de avaliação se os mapeamentos estão criados quando da obtenção de provider;
- Atualização de versão da biblioteca Intentor.Utilities para 11.8.1.1330.

## 11.5.20.1406 (20/05/2011)
- Correção de problema que impedia a decriptação de strings de conexão criptografadas;
- Adaptações na geração de mapeamento para permitir leitura de arquivos .cmf como resources de um assembly (para uso em projetos Windows Form);
- Acréscimo de projeto exemplo para Windows Forms;
- Acréscimo de MembershipProvider do Yamapper, YamapperMembershipProvider (nem todos os métodos estão implementados);
- Alteração do script de geração de objetos para geração dos diretórios de objetos Generated (para objetos que contenham procedimentos gerados automaticamente) e Custom (para objetos que podem conter customização do usuário);
- Alteração do script de geração de objetos para retorno de GetById em objetos Dao ser NULL caso nenhum dado seja encontrado;
- Alteração do script de geração de objetos para permitir identificação opcional de conexão com o banco de dados para objetos.

## 11.2.10.1111 (10/02/2011)
- Compilação de versão para o .NET 4.0 Full Profile;
- Substituição do Castle.DynamicProxy pela versão mais recente presente em Castle.Core;
- Correção de problema com preenchimento de propriedades via lazy loading mesmo após a definição de um valor;
- Gerador: correção de problema no com Design by Contract de Strings de valores nulos;
- Melhorias nos comentários dos schemas dos arquivos de configuração e mapeamento.

## 11.1.27.1703 (27/01/2011)
- Correção de operador "menor ou igual" na expressão SimpleExpression;
- Criação de campo para armazenamento dos campos analisados durante lazy loading para evitar problemas com análise de valye types vazios.

## 11.1.12.1042 (12/01/2011)
- Correção de problema envolvendo conversão de valores binários quando da atualização de objetos.

## 11.1.3.1121 (03/01/2011)
- Correção de problema envolvendo conversão de valores binários quando da inserção de objetos.

## 10.12.19.1516 (19/12/2010)
- Acréscimo de mensagem indicando se um mapeamento solicitado foi efetivamente localizado;
- Correção de problema com avaliação de valores nulos de banco de dados quando da obtenção de dados via lazy loading.

## 10.11.16.1345 (16/11/2010)
- Correção de problema com driver do MySQL envolvendo paginação;
- Criação do parâmetro "defaultLimit" para indicação do valor padrão de Limit utilizado em paginações no driver do ## MySQL;
- Criação de validação de conexões obtidas em DbProviderFactory;
- Atualização de versão padrão do assembly do provider do MySQL para 6.3.5.

## 10.11.3.1604 (03/11/2010)
- Correção de problema com nomes de parâmetros repetidos na expressão BETWEEN.

## 10.10.25.1138 (25/10/2010)
- Atualizações para suporte ao Intentor.Utilities 10.10.25.1130.

## 10.10.22.1412 (22/10/2010)
- Acréscimo de geração arquivos customizáveis de DataInterfaces, Dao e Biz (agora há separação entre objetos do modelo gerados automaticamente e customizados pelo usuário);
- Acréscimo de expressões IN e NOT IN em objetos Criteria;
- Acréscimo de agrupamento de expressões por AND e OR em objetos Criteria;
- Acréscimo de expressão de instruções SQL em objetos Criteria, com uso do curinga {param} para indicação do local aonde estará o parâmetro;
- Acréscimo de possibilidade de limitação/paginação de registros através dos métodos Offset e Limit incluídos na classe Criteria;
- Acréscimo de método ExecuteScalar não genérico;
- Alterações internas no mapeador para permitir chaves compostas, lazy loading e limitação de registros;
- Alteração do método Create de ICommonDataBaseActions para não retornar dados (chaves primárias são preenchidas diretamente no objeto enviado);
- Alteração do gerador de código para refletir as mudanças de métodos;
- Alterações no schema de mapeamento para inclusão de indicação de chave de autonumeração, lazy loading (para uso em versão futura) e colocação de sequence sobre campo, não mais tabela;
- Retirada do método Fill no provedor;
- Retirada dos métodos Get com sobrecargas de instrução SQL e objetos IDbCommand;
- Retirada do método GetById no provedor. Tal método será gerado automaticamente dentro de cada Dao de acordo com a(s) chave(s) primária(s);
- Retirada do método Delete por ID no provedor. Tal método será gerado automaticamente em cada Dao de acordo com a(s) chave(s) primária(s);
- Retirada dos métodos GetOf e GetListOf. As ações de tais métodos são executadas a partir de sobrecargas do método Get;
- Retirada dos métodos GetById e Delete com ID de ICommonDataBaseActions;
- Retirada da necessidade de informação do tipo do campo PK de ICommonDataBaseActions;
- Retirada do driver de acesso ao banco de dados do Access por baixa utilização;
- Durante inclusão, somente são ignorados campos que sejam de autonumeração ou marcados para tal;
- O método Insert não mais retorna chave primária gerada; ao invés disso, preenche seus valores diretamente na entidade.

## 10.8.22.1511 (22/08/2010)
- Os métodos de Insert não necessitam mais de passagem de parâmetros por referência;
- Criação de método para busca por critérios;
- Criação de método para deleção com critérios;
- Criação de método Clone em Criteria a partir da implementação da interface ICloneable;
- Criação da interface IViewDataBaseActions para representação do contrato de uma View;
- A interface ICommonDataBaseActions passa a implementar a inerface IViewDataBaseActions;
- Melhorias na validação de arquivos de mapeamento e configuração;
- Adaptações e correções diversas no gerador de código do MyGeneration.

## 10.8.12.1529 (12/08/2010)
- Correção do método MapToSingle para realização de leitura do DataReader antes de seu preenchimento;
- Acréscimo dos métodos Exists (para verificação de existência de registros) e Count (para contagem de registros) a partir de critérios.

## 10.4.1.1651 (01/04/2010)
- Gerador: correção de problemas com falta de aspas na geração de mapeamento externo;
- Gerador: acréscimo da possibilidade de seleção de inclusão dos namespaces dos TransferObjects e Views;
- Criação dos métodos ASC e DESC na classe Order para simplificar a criação de critérios de ordenação;
- Acréscimo de possibilidade de envio de parâmetros ao provedor de acesso ao banco de dados a partir do atributo "params" da tag "connection", com cada driver podendo possuir seu próprio conjunto de parâmetros;
- Atualização de projeto exemplo enviado junto do código-fonte para exemplificar o uso de ordenação e obtenção de dados a partir de consultas SQL customizadas.