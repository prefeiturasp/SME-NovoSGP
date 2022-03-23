DROP TABLE if exists anotacao_fechamento_aluno;

CREATE TABLE anotacao_fechamento_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_aluno_id int8 NOT NULL,
	anotacao varchar NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL
);

ALTER TABLE anotacao_fechamento_aluno ADD CONSTRAINT anotacao_fechamento_aluno_fk FOREIGN KEY (fechamento_aluno_id) REFERENCES fechamento_aluno(id);
CREATE INDEX anotacao_fechamento_aluno_fechamento_aluno_idx ON public.anotacao_fechamento_aluno USING btree (fechamento_aluno_id);
