USE [Manutencao]
GO

IF OBJECT_ID('[dbo].[ETL_SGP_Compensacao_Ausencia]', 'U') IS NOT NULL
   DROP TABLE [dbo].[ETL_SGP_Compensacao_Ausencia]
GO

IF OBJECT_ID('[dbo].[ETL_SGP_Compensacao_Ausencia_508]', 'U') IS NOT NULL
   DROP TABLE [dbo].[ETL_SGP_Compensacao_Ausencia_508]
GO


CREATE TABLE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
(
	[ID]                     INT IDENTITY(1,1) NOT NULL,
	[BIMESTRE]               INT               NOT NULL,
	[DISCIPLINA_ID]          VARCHAR (15)      NOT NULL,
	[TURMA_ID]               INT               NOT NULL,
	[NOME]                   VARCHAR (MAX)         NULL,
	[DESCRICAO]              VARCHAR (MAX)     NOT NULL,
	[CRIADO_EM]              DATETIME          NOT NULL,
	[ALTERADO_EM]            DATETIME              NULL,
	[ANO_LETIVO]             INT               NOT NULL,
	[CODIGO_ALUNO]           VARCHAR (100)     NOT NULL,
	[QTD_FALTAS_COMPENSADAS] INT               NOT NULL,
 CONSTRAINT [PK_ETL_SGP_Compensacao_Ausencia] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia_508]
(
	[ID]                     INT IDENTITY(1,1) NOT NULL,
	[BIMESTRE]               INT               NOT NULL,
	[DISCIPLINA_ID]          VARCHAR (15)      NOT NULL,
	[TURMA_ID]               INT               NOT NULL,
	[NOME]                   VARCHAR (MAX)         NULL,
	[DESCRICAO]              VARCHAR (MAX)     NOT NULL,
	[CRIADO_EM]              DATETIME          NOT NULL,
	[ALTERADO_EM]            DATETIME              NULL,
	[ANO_LETIVO]             INT               NOT NULL,
	[CODIGO_ALUNO]           VARCHAR (100)     NOT NULL,
	[QTD_FALTAS_COMPENSADAS] INT               NOT NULL,
 CONSTRAINT [PK_ETL_SGP_Compensacao_Ausencia_508] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
