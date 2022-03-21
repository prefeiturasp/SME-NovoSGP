USE [Manutencao]
GO

DROP TABLE [dbo].[ETL_SGP_DISCIPLINAS]
GO

DROP TABLE [dbo].[ETL_SGP_PLANO_ANUAL]
GO

DROP TABLE [dbo].[ETL_SGP_PLANO_ANUAL508]
GO


CREATE TABLE [dbo].[ETL_SGP_DISCIPLINAS]
(
	IdDisciplina INT,
	NmDisciplina VARCHAR(200)
);


CREATE TABLE [dbo].[ETL_SGP_PLANO_ANUAL]
(
	id                            INT           NOT NULL IDENTITY (1,1),
	escola_id                     VARCHAR  (10) NOT NULL,
	turma_id                      INT           NOT NULL,
	ano                           INT           NOT NULL,
	bimestre                      INT           NOT NULL,
	descricao                     VARCHAR (MAX)     NULL,
	criado_em                     DATETIME      NOT NULL,
	criado_por                    VARCHAR (200) NOT NULL,
	alterado_em                   DATETIME      NOT NULL,
	alterado_por                  VARCHAR (200)     NULL,
	criado_rf                     VARCHAR (200) NOT NULL,
	alterado_rf                   VARCHAR (200)     NULL,
	migrado                       BIT           NOT NULL,
	componente_curricular_eol_id  INT           NOT NULL
);
			

CREATE TABLE [dbo].[ETL_SGP_PLANO_ANUAL508]
(
	id                            INT           NOT NULL,
	escola_id                     VARCHAR  (10) NOT NULL,
	turma_id                      INT           NOT NULL,
	ano                           INT           NOT NULL,
	bimestre                      INT           NOT NULL,
	descricao                     VARCHAR (MAX)     NULL,
	criado_em                     DATETIME      NOT NULL,
	criado_por                    VARCHAR (200) NOT NULL,
	alterado_em                   DATETIME      NOT NULL,
	alterado_por                  VARCHAR (200)     NULL,
	criado_rf                     VARCHAR (200) NOT NULL,
	alterado_rf                   VARCHAR (200)     NULL,
	migrado                       BIT           NOT NULL,
	componente_curricular_eol_id  INT           NOT NULL
);
