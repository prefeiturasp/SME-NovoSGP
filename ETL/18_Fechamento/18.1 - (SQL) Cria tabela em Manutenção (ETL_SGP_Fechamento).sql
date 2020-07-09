USE [Manutencao]
GO

IF OBJECT_ID('[dbo].[ETL_SGP_Fechamento]', 'U') IS NOT NULL
   DROP TABLE [dbo].[ETL_SGP_Fechamento]
GO

IF OBJECT_ID('[dbo].[ETL_SGP_Fechamento_508]', 'U') IS NOT NULL
   DROP TABLE [dbo].[ETL_SGP_Fechamento_508]
GO


CREATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento]
(
	[ID]                     INT IDENTITY(1,1) NOT NULL,
	[TURMA_ID]               INT               NOT NULL,
	[PERIODO_ESCOLAR_ID]     INT                   NULL,
	[CRIADO_EM]              DATETIME          NOT NULL,
	[ALTERADO_EM]            DATETIME              NULL,
	[DISCIPLINA_ID]          INT               NOT NULL,
	[ALUNO_CODIGO]           VARCHAR (15)      NOT NULL,
	[NOTA]                   VARCHAR (15)          NULL,
	[SINTESE_ID]             INT                   NULL,
	[CONCEITO_ID]            INT                   NULL
 CONSTRAINT [PK_ETL_SGP_Fechamento] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_508]
(
	[ID]                     INT IDENTITY(1,1) NOT NULL,
	[TURMA_ID]               INT               NOT NULL,
	[PERIODO_ESCOLAR_ID]     INT                   NULL,
	[CRIADO_EM]              DATETIME          NOT NULL,
	[ALTERADO_EM]            DATETIME              NULL,
	[DISCIPLINA_ID]          INT               NOT NULL,
	[ALUNO_CODIGO]           VARCHAR (15)      NOT NULL,
	[NOTA]                   VARCHAR (15)          NULL,
	[SINTESE_ID]             INT                   NULL,
	[CONCEITO_ID]            INT                   NULL
 CONSTRAINT [PK_ETL_SGP_Fechamento_508] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
