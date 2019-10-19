CREATE TABLE public.evento_tipo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(200) NOT NULL,
	local_ocorrencia int4 NOT NULL,
	concomitancia bool NOT NULL DEFAULT true,
	tipo_data int4 NOT NULL,
	dependencia bool NOT NULL DEFAULT false,
	letivo int4 NOT NULL,
	ativo bool NOT NULL DEFAULT true,
	criado_em timestamp NOT NULL,
	alterado_em timestamp,
	alterado_por varchar(200),
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200),
	migrado bool NOT NULL DEFAULT false
);
