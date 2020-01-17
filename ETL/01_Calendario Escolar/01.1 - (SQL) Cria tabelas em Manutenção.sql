USE [Manutencao]
GO

DROP TABLE [dbo].[ETL_SGP_CALENDARIO_ESCOLAR_TIPO_CALENDARIO]
GO

DROP TABLE [dbo].[ETL_SGP_CALENDARIO_ESCOLAR_PERIODO_ESCOLAR]
GO

DROP TABLE [dbo].[ETL_SGP_CALENDARIO_ESCOLAR_EVENTO]
GO

CREATE TABLE [dbo].[ETL_SGP_CALENDARIO_ESCOLAR_TIPO_CALENDARIO]
(
	id           INT           NOT NULL IDENTITY (1,1),
	id_LEGADO_TC INT           NOT NULL,
	ano_letivo   INT           NOT NULL,
	nome         VARCHAR (50)  NOT NULL,
	periodo      INT           NOT NULL,
	modalidade   INT           NOT NULL,
	situacao     BIT           NOT NULL DEFAULT 1,
	criado_em    DATETIME      NOT NULL,
	criado_por   VARCHAR (200) NOT NULL,
	alterado_em  DATETIME          NULL,
	alterado_por VARCHAR (200) NULL,
	criado_rf    VARCHAR (200) NOT NULL,
	alterado_rf  VARCHAR (200)     NULL,
	excluido     BIT           NOT NULL DEFAULT 0,
	migrado      BIT           NOT NULL DEFAULT 0
);

CREATE TABLE [dbo].[ETL_SGP_CALENDARIO_ESCOLAR_PERIODO_ESCOLAR]
(
	id                 INT           NOT NULL IDENTITY (1,1),
	id_LEGADO_PE       INT           NOT NULL,
	tipo_calendario_id INT           NOT NULL,
	bimestre           INT           NOT NULL,
	periodo_inicio     DATETIME      NOT NULL,
	periodo_fim        DATETIME      NOT NULL,
	alterado_por       VARCHAR (200)     NULL,
	alterado_rf        VARCHAR (200)     NULL,
	alterado_em        DATETIME          NULL,
	criado_por         VARCHAR (200) NOT NULL,
	criado_rf          VARCHAR (200) NOT NULL,
	criado_em          DATETIME      NOT NULL,
	migrado            BIT           NOT NULL DEFAULT 0
);

CREATE TABLE [dbo].[ETL_SGP_CALENDARIO_ESCOLAR_EVENTO]
(
	id                 INT NOT NULL IDENTITY (1,1),
	id_LEGADO_EV       INT NOT NULL,
	nome               VARCHAR (200) NOT NULL,
	descricao          VARCHAR (500)     NULL,
	data_inicio        DATETIME      NOT NULL,
	data_fim           DATETIME          NULL,
	letivo             INT           NOT NULL,
	feriado_id         INT               NULL,
	tipo_calendario_id INT           NOT NULL,
	tipo_evento_id     INT           NOT NULL,
	dre_id             VARCHAR (15)      NULL,
	ue_id              VARCHAR (15)      NULL,
	criado_em          DATETIME      NOT NULL,
	criado_por         VARCHAR (200) NOT NULL,
	alterado_em        DATETIME          NULL,
	alterado_por       VARCHAR (200)     NULL,
	criado_rf          VARCHAR (200) NOT NULL,
	alterado_rf        VARCHAR (200)     NULL,
	excluido           BIT           NOT NULL DEFAULT 0,
	migrado	           BIT	         NOT NULL DEFAULT 0
);