USE Manutencao
GO


-- Apagando as tabelas do ETL na base de Manutencao

IF OBJECT_ID('[Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS]', 'U') IS NOT NULL
   DROP TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS]
GO

IF OBJECT_ID('[Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508]', 'U') IS NOT NULL
	DROP TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508]
GO

IF OBJECT_ID('[Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P1]', 'U') IS NOT NULL
	DROP TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P1]
GO

IF OBJECT_ID('[Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2]', 'U') IS NOT NULL
	DROP TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2]
GO

IF OBJECT_ID('[Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3]', 'U') IS NOT NULL
	DROP TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3]
GO


-- Criando a tabela [ETL_SGP_ATRIBUICAOCJ_DADOS]

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS]
(
	[ID]			   INT          NOT NULL IDENTITY (1,1),
	[DISCIPLINA_ID]    INT          NOT NULL,
	[DRE_ID]           VARCHAR (15) NOT NULL,
	[UE_ID]            VARCHAR (15) NOT NULL,
	[PROFESSOR_RF]     VARCHAR (10) NOT NULL,
	[TURMA_ID]         VARCHAR (15) NOT NULL,
	[MODALIDADE]       INT          NOT NULL
);


-- Criando a tabela [ETL_SGP_ATRIBUICAOCJ_DADOS2]

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2]
(
	[ID]			   INT          NOT NULL IDENTITY (1,1),
	[DISCIPLINA_ID]    INT          NOT NULL,
	[DRE_ID]           VARCHAR (15) NOT NULL,
	[UE_ID]            VARCHAR (15) NOT NULL,
	[PROFESSOR_RF]     VARCHAR (10) NOT NULL,
	[TURMA_ID]         VARCHAR (15) NOT NULL,
	[MODALIDADE]       INT          NOT NULL
);


-- Criando a tabela [ETL_SGP_ATRIBUICAOCJ_508]

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508]
(
	[ID]			   INT          NOT NULL IDENTITY (1,1),
	[DISCIPLINA_ID]    INT          NOT NULL,
	[DRE_ID]           VARCHAR (15) NOT NULL,
	[UE_ID]            VARCHAR (15) NOT NULL,
	[PROFESSOR_RF]     VARCHAR (10) NOT NULL,
	[TURMA_ID]         VARCHAR (15) NOT NULL,
	[MODALIDADE]       INT          NOT NULL
);


-- Criando a tabela [ETL_SGP_ATRIBUICAOCJ_P1]

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P1]
(
	[ID]			          INT           NOT NULL IDENTITY (1,1),
    [TURMA_ID]                INT           NOT NULL,
	[DISCIPLINA_ID]           INT           NOT NULL,
	[TUR_ID]                  BIGINT        NOT NULL,
	[TUD_ID]                  BIGINT        NOT NULL,
	[ESC_ID]                  INT           NOT NULL,
	[ESC_CODIGO]              VARCHAR (20)  NOT NULL,
	[DIS_ID]                  INT           NOT NULL,
	[NM_DISCIPLINA]           VARCHAR (200) NOT NULL,
	[USU_ID]                  VARCHAR (500) NOT NULL,
	[USU_IDDOCENTEALTERACAO]  VARCHAR (500)     NULL,
	[USU_LOGIN]               VARCHAR (500) NOT NULL,
	[TAU_DATA]                DATE          NOT NULL
);


-- Criando a tabela [ETL_SGP_ATRIBUICAOCJ_P2]

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2]
(
	[ID]			   INT          NOT NULL IDENTITY (1,1),
	[DISCIPLINA_ID]    INT          NOT NULL,
	[DRE_ID]           VARCHAR (15) NOT NULL,
	[UE_ID]            VARCHAR (15) NOT NULL,
	[PROFESSOR_RF]     VARCHAR (10) NOT NULL,
	[TURMA_ID]         VARCHAR (15) NOT NULL,
	[MODALIDADE]       INT          NOT NULL
);


-- Criando a tabela [ETL_SGP_ATRIBUICAOCJ_P3]

CREATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3]
(
	[ID]			   INT          NOT NULL IDENTITY (1,1),
	[DISCIPLINA_ID]    INT          NOT NULL,
	[DRE_ID]           VARCHAR (15) NOT NULL,
	[UE_ID]            VARCHAR (15) NOT NULL,
	[PROFESSOR_RF]     VARCHAR (10) NOT NULL,
	[TURMA_ID]         VARCHAR (15) NOT NULL,
	[MODALIDADE]       INT          NOT NULL
);

