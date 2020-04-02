alter table nota_conceito_bimestre drop column if exists anotacao;

DROP TABLE if exists public.anotacao_aluno_fechamento;
create table public.anotacao_aluno_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_disciplina_id int8 not null,
	aluno_codigo varchar(15) not null,
	anotacao varchar not null,

	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT anotacao_aluno_fechamento_pk PRIMARY KEY (id)
);
CREATE INDEX anotacao_aluno_fechamento_fechamento_idx ON public.anotacao_aluno_fechamento USING btree (fechamento_turma_disciplina_id);
CREATE INDEX anotacao_aluno_fechamento_aluno_idx ON public.anotacao_aluno_fechamento USING btree (aluno_codigo);

ALTER TABLE public.anotacao_aluno_fechamento ADD CONSTRAINT anotacao_aluno_fechamento_fechamento_fk FOREIGN KEY (fechamento_turma_disciplina_id) REFERENCES fechamento_turma_disciplina(id);
