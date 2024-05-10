CREATE TABLE IF NOT EXISTS public.historico_escolar_observacao  (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aluno_codigo varchar(15) NOT NULL,
	observacao varchar(500) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT historico_escolar_observacao_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS historico_escolar_observacao_aluno_idx ON public.historico_escolar_observacao USING btree (aluno_codigo);