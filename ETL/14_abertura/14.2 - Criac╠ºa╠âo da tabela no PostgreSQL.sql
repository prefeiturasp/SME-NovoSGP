CREATE TABLE  public.etl_sgp_fechamento_reabertura(
			id						int8				NOT NULL GENERATED ALWAYS AS IDENTITY,
			descricao				varchar(200)		NULL,
			inicio					timestamp			NOT NULL,
			fim						timestamp			NOT NULL,
			tipo_calendario_id		int4				NOT NULL,
			dre_id_eol				varchar(200)		NULL,
			ue_id_eol				varchar(200)		NULL,
			status					int4				NULL,
			criado_em				timestamp			NOT null,
			bimestre				int4				null
	)