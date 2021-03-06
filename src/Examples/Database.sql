USE [Yamapper]
GO

/****** Object:  Table [dbo].[tbCliente]    Script Date: 01/27/2011 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbCliente](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[DataCadastro] [datetime] NULL,
	[Informativos] [bit] NOT NULL,
 CONSTRAINT [PK_tbClientes] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tbCliente] ADD  CONSTRAINT [DF_tbCliente_Informativos]  DEFAULT ((1)) FOR [Informativos]
GO



USE [Yamapper]
GO

/****** Object:  Table [dbo].[tbClienteProduto]    Script Date: 10/22/2010 13:08:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbClienteProduto](
	[IdCliente] [int] NOT NULL,
	[IdProduto] [int] NOT NULL,
 CONSTRAINT [PK_tbCliente_Produto] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC,
	[IdProduto] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbClienteProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbClienteProduto_tbClientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[tbCliente] ([IdCliente])
GO

ALTER TABLE [dbo].[tbClienteProduto] CHECK CONSTRAINT [FK_tbClienteProduto_tbClientes]
GO

ALTER TABLE [dbo].[tbClienteProduto]  WITH CHECK ADD  CONSTRAINT [FK_tbClienteProduto_tbProduto] FOREIGN KEY([IdProduto])
REFERENCES [dbo].[tbProduto] ([IdProduto])
GO

ALTER TABLE [dbo].[tbClienteProduto] CHECK CONSTRAINT [FK_tbClienteProduto_tbProduto]
GO

USE [Yamapper]
GO

/****** Object:  Table [dbo].[tbProduto]    Script Date: 10/22/2010 13:08:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbProduto](
	[IdProduto] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
 CONSTRAINT [PK_tbProduto] PRIMARY KEY CLUSTERED 
(
	[IdProduto] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO