CREATE TABLE if not exists public.atribuicao_esporadica (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	professor_rf varchar(10) NOT NULL,
	ue_id varchar(15) NOT NULL,
	dre_id varchar(15) NOT NULL,
	data_inicio date NOT NULL,
	data_fim date NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false
);
