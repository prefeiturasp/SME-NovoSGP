USE Manutencao
CREATE TABLE etl_sgp_periodo_reabertura(
			id						int					NOT NULL IDENTITY(1,1),
			descricao				varchar(200)		NULL,
			inicio					datetime			NOT NULL,
			fim						datetime			NOT NULL,
			tipo_calendario_id		int					NOT NULL,
			dre_id_eol				varchar(200)		NULL,
			ue_id_eol				varchar(200)		NULL,
			status					int					NULL,
			criado_em				datetime			NOT NULL,
			bimestre				int					NULL
	)