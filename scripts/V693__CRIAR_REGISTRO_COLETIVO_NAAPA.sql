-- Tipo de reunião NAAPA

CREATE TABLE public.tipo_reuniao_naapa (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	titulo varchar(50) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(30) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(30) NULL,
	CONSTRAINT TipoReuniao_pk PRIMARY KEY (id)
);