# Roadmap for Yamapper

Roadmap details in Brazilian Portuguese.

## New features

- Criar PreCondition do Criteria
	- Para qualquer criteria, chama-o antes
	- booleano indicando se vale para todo request ou só para aqueles que possuem Criteria
- Cache de queries
- Cache de classes inteiras
	- Meios de invalidar
	- Setar pelo arquivo de mapemaneto
		- Application
		- Cache 
		- Session
	- Tempo
	- Cachear por propriedade
- Carga dos filhos (via FK)
	- Many-to-one
	- Many-to-many (usando tabela de controle)
- gerar os arquivos de mapeamento com o nome do schema.nometabela