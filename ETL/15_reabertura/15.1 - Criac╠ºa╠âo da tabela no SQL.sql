USE Manutencao
CREATE TABLE etl_sgp_periodo_fechamento (
	id							int				 IDENTITY(1,1) NOT NULL,
	esc_id						int				 NULL,
	tpc_id						int				 NULL,
	dre_id_eol					varchar(15)		 NULL,
	ue_id_eol					varchar(6)		 NULL,
	nome						varchar(200)	 NULL,
	datainicio					datetime		 NULL,
	datafim						datetime		 NULL,
	datacriacao					datetime		 NULL,
	dataalteracao				datetime		 NULL,
	tipo						int				 NULL
);