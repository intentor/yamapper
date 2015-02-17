SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[yamapper_Membership](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](10) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[PasswordFormat] [smallint] NOT NULL,
	[PasswordQuestion] [varchar](200) NULL,
	[PasswordAnswer] [varchar](200) NULL,
	[FailedPasswordAttemptCount] [smallint] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAnswerAttemptCount] [smallint] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
	[LastLockOutDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[Comments] [varchar](500) NULL,
 CONSTRAINT [PK_yamapper_Membership] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID do usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'UserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome do usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'Username'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'E-mail do usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'Email'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Senha do usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'Password'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Formato da senha do usuário. 0 - Clear; 1 - Hashed; 2 - Encrypted.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'PasswordFormat'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pergunta para recuperação de senha.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'PasswordQuestion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Resposta da pergunta para recuperação de senha.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'PasswordAnswer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contador de tentativas de falha de validação de senha.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'FailedPasswordAttemptCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última falha de validação de senha.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'FailedPasswordAttemptWindowStart'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contador de tentativas de falha de validação de resposta de pergunta.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'FailedPasswordAnswerAttemptCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última falha de validação de resposta de pergunta.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'FailedPasswordAnswerAttemptWindowStart'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última troca de senha.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'LastPasswordChangedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se o usuário está aprovado.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'IsApproved'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se o usuário está bloqueado para acesso.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'IsLockedOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora de criação do usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'CreationDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última atividade realizada pelo usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'LastActivityDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora do último travamento do usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'LastLockOutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora do último login do usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'LastLoginDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentários sobre o usuário.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership', @level2type=N'COLUMN',@level2name=N'Comments'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Armazena dados de usuários do Membership Provider do Yamapper.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'yamapper_Membership'
GO

ALTER TABLE [dbo].[yamapper_Membership] ADD  CONSTRAINT [DF_yamapper_Membership_PasswordFormat]  DEFAULT ((0)) FOR [PasswordFormat]
GO

ALTER TABLE [dbo].[yamapper_Membership] ADD  CONSTRAINT [DF_yamapper_Membership_FailedPasswordAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAttemptCount]
GO

ALTER TABLE [dbo].[yamapper_Membership] ADD  CONSTRAINT [DF_yamapper_Membership_FailedPasswordAnswerAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAnswerAttemptCount]
GO

ALTER TABLE [dbo].[yamapper_Membership] ADD  CONSTRAINT [DF_yamapper_Membership_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO

ALTER TABLE [dbo].[yamapper_Membership] ADD  CONSTRAINT [DF_yamapper_Membership_IsLockedOut]  DEFAULT ((0)) FOR [IsLockedOut]
GO

ALTER TABLE [dbo].[yamapper_Membership] ADD  CONSTRAINT [DF_yamapper_Membership_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [dbo].[yamapper_Membership] ADD  CONSTRAINT [DF_yamapper_Membership_LastActivityDate]  DEFAULT (getdate()) FOR [LastActivityDate]
GO

