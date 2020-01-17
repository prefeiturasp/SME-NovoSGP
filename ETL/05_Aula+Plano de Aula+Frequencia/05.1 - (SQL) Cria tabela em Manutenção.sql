USE Manutencao
GO

DROP TABLE [dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
GO

DROP TABLE [dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO508]
GO

DROP TABLE [dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO_OK]
GO

CREATE TABLE [dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
(
	id							INT           NOT NULL IDENTITY (1,1),
	AUL_ue_id					VARCHAR  (15) NOT NULL,
	AUL_disciplina_id			VARCHAR  (15) NOT NULL,
	AUL_turma_id				VARCHAR  (15) NOT NULL,
	AUL_tipo_calendario_id		INT			  NOT NULL,
	AUL_professor_rf			VARCHAR  (15) NOT NULL,
	AUL_quantidade				INT			  NOT NULL,
	AUL_data_aula				DATETIME	  NOT NULL,
	AUL_recorrencia_aula		INT			  NOT NULL,
	AUL_tipo_aula				INT			  NOT NULL,
	AUL_criado_em				DATETIME	  NOT NULL,
	AUL_criado_por				VARCHAR (200) NOT NULL,
	AUL_alterado_em				DATETIME		  NULL,
	AUL_alterado_por			VARCHAR (200)	  NULL,
	AUL_criado_rf				VARCHAR  (15) NOT NULL,
	AUL_alterado_rf				VARCHAR  (15)     NULL,
	PLA_descricao				VARCHAR (MAX)     NULL,
	PLA_desenvolvimento_aula	VARCHAR (MAX) NOT NULL,
	PLA_recuperacao_aula		VARCHAR (MAX)     NULL,
	PLA_licao_casa				VARCHAR (MAX)     NULL,
	SQL_tud_id					INT			      NULL,
	SQL_tpc_id					INT			      NULL,
	SQL_tau_id					INT			      NULL,
	SQL_tur_id					INT			      NULL,
	SQL_tur_codigoEOL			INT			      NULL,
	SQL_esc_id					INT			      NULL,
	SQL_esc_codigoEOL			VARCHAR  (20)     NULL
);
GO

CREATE TABLE [dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO508]
(
	id							INT           NOT NULL,
	AUL_ue_id					VARCHAR  (15) NOT NULL,
	AUL_disciplina_id			VARCHAR  (15) NOT NULL,
	AUL_turma_id				VARCHAR  (15) NOT NULL,
	AUL_tipo_calendario_id		INT			  NOT NULL,
	AUL_professor_rf			VARCHAR  (15) NOT NULL,
	AUL_quantidade				INT			  NOT NULL,
	AUL_data_aula				DATETIME	  NOT NULL,
	AUL_recorrencia_aula		INT			  NOT NULL,
	AUL_tipo_aula				INT			  NOT NULL,
	AUL_criado_em				DATETIME	  NOT NULL,
	AUL_criado_por				VARCHAR (200) NOT NULL,
	AUL_alterado_em				DATETIME		  NULL,
	AUL_alterado_por			VARCHAR (200)	  NULL,
	AUL_criado_rf				VARCHAR  (15) NOT NULL,
	AUL_alterado_rf				VARCHAR  (15)     NULL,
	PLA_descricao				VARCHAR (MAX)     NULL,
	PLA_desenvolvimento_aula	VARCHAR (MAX) NOT NULL,
	PLA_recuperacao_aula		VARCHAR (MAX)     NULL,
	PLA_licao_casa				VARCHAR (MAX)     NULL,
	SQL_tud_id					INT			      NULL,
	SQL_tpc_id					INT			      NULL,
	SQL_tau_id					INT			      NULL,
	SQL_tur_id					INT			      NULL,
	SQL_tur_codigoEOL			INT			      NULL,
	SQL_esc_id					INT			      NULL,
	SQL_esc_codigoEOL			VARCHAR  (20)     NULL
);
GO

CREATE TABLE [dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO_OK]
(
	id							INT           NOT NULL IDENTITY (1,1),
	AUL_ue_id					VARCHAR  (15) NOT NULL,
	AUL_disciplina_id			VARCHAR  (15) NOT NULL,
	AUL_turma_id				VARCHAR  (15) NOT NULL,
	AUL_tipo_calendario_id		INT			  NOT NULL,
	AUL_professor_rf			VARCHAR  (15) NOT NULL,
	AUL_quantidade				INT			  NOT NULL,
	AUL_data_aula				DATETIME	  NOT NULL,
	AUL_recorrencia_aula		INT			  NOT NULL,
	AUL_tipo_aula				INT			  NOT NULL,
	AUL_criado_em				DATETIME	  NOT NULL,
	AUL_criado_por				VARCHAR (200) NOT NULL,
	AUL_alterado_em				DATETIME		  NULL,
	AUL_alterado_por			VARCHAR (200)	  NULL,
	AUL_criado_rf				VARCHAR  (15) NOT NULL,
	AUL_alterado_rf				VARCHAR  (15)     NULL,
	PLA_descricao				VARCHAR (MAX)     NULL,
	PLA_desenvolvimento_aula	VARCHAR (MAX) NOT NULL,
	PLA_recuperacao_aula		VARCHAR (MAX)     NULL,
	PLA_licao_casa				VARCHAR (MAX)     NULL,
	SQL_tud_id					INT			      NULL,
	SQL_tpc_id					INT			      NULL,
	SQL_tau_id					INT			      NULL,
	SQL_tur_id					INT			      NULL,
	SQL_tur_codigoEOL			INT			      NULL,
	SQL_esc_id					INT			      NULL,
	SQL_esc_codigoEOL			VARCHAR  (20)     NULL
);
GO