USE [Manutencao]
GO

DROP TABLE [dbo].[ETL_SGP_PLANO_DE_CICLO]
GO

CREATE TABLE [dbo].[ETL_SGP_PLANO_DE_CICLO]
(
	id                            INT           NOT NULL IDENTITY (1,1),
	descricao                     VARCHAR (MAX) NOT NULL,
	ano                           INT           NOT NULL,
	ciclo_id                      INT           NOT NULL,
	escola_id                     VARCHAR  (10) NOT NULL,
	migrado                       BIT           NOT NULL,
	criado_em                     DATETIME      NOT NULL,
	criado_por                    VARCHAR (200) NOT NULL,
	alterado_em                   DATETIME      NOT NULL,
	alterado_por                  VARCHAR (200)     NULL,
	criado_rf                     VARCHAR (200) NOT NULL,
	alterado_rf                   VARCHAR (200)     NULL
);
