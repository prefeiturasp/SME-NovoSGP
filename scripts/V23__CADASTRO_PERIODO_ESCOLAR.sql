CREATE TABLE IF NOT EXISTS public.periodo_escolar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_calendario_id int8 NOT NULL,
	bimestre int NOT NULL,
	periodo_inicio timestamp NOT NULL,
	periodo_fim timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL
);