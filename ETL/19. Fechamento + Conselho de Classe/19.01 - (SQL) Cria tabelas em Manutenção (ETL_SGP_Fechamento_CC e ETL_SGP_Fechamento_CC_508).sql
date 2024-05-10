USE [Manutencao]
GO

IF OBJECT_ID('[dbo].[ETL_SGP_Fechamento_CC]', 'U') IS NOT NULL
   DROP TABLE [dbo].[ETL_SGP_Fechamento_CC]
GO

IF OBJECT_ID('[dbo].[ETL_SGP_Fechamento_CC_508]', 'U') IS NOT NULL
   DROP TABLE [dbo].[ETL_SGP_Fechamento_CC_508]
GO


CREATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_CC]
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
	[CONCEITO_ID]            INT                   NULL,
	[CRIADO_EM_CC]           DATETIME              NULL,
	[ALTERADO_EM_CC]         DATETIME              NULL,
	[RECOMENDACOES_ALUNO]    VARCHAR (MAX)         NULL,
	[RECOMENDACOES_FAMILIA]  VARCHAR (MAX)         NULL,
	[ANOTACOES_PEDAGOGICAS]  VARCHAR (MAX)         NULL,
	[NOTACC]                 VARCHAR (15)          NULL,
	[JUSTIFICATIVA]          VARCHAR (MAX)         NULL,
	[PARECER]                INT                   NULL,
	[SERIE]                  INT                   NULL,
	[CONCEITO_ID_CC]         INT                   NULL
 CONSTRAINT [PK_ETL_SGP_Fechamento_CC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_CC_508]
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
	[CONCEITO_ID]            INT                   NULL,
	[CRIADO_EM_CC]           DATETIME              NULL,
	[ALTERADO_EM_CC]         DATETIME              NULL,
	[RECOMENDACOES_ALUNO]    VARCHAR (MAX)         NULL,
	[RECOMENDACOES_FAMILIA]  VARCHAR (MAX)         NULL,
	[ANOTACOES_PEDAGOGICAS]  VARCHAR (MAX)         NULL,
	[NOTACC]                 VARCHAR (15)          NULL,
	[JUSTIFICATIVA]          VARCHAR (MAX)         NULL,
	[PARECER]                INT                   NULL,
	[SERIE]                  INT                   NULL,
	[CONCEITO_ID_CC]         INT                   NULL
 CONSTRAINT [PK_ETL_SGP_Fechamento_CC_508] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
