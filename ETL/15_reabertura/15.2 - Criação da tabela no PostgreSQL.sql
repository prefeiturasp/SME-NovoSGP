-- Drop table
-- DROP TABLE public.etl_sgp_periodo_fechamento;

CREATE TABLE public.etl_sgp_periodo_fechamento (
	id int8 NOT NULL,
	esc_id int8 NULL,
	tpc_id int8 NULL,
	dre_id_eol varchar(15) NULL,
	ue_id_eol varchar(6) NULL,
	nome varchar(200) NULL,
	datainicio timestamp NULL,
	datafim timestamp NULL,
	datacriacao timestamp NULL,
	dataalteracao timestamp NULL,
	tipo int4 NULL
);
