CREATE TABLE if not exists public.anotacao_frequencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	motivo_ausencia_id int8 NULL,
	anotacao varchar NULL,
	aula_id int8 NOT NULL,
	codigo_aluno varchar(15) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT anotacao_frequencia_aluno_pk PRIMARY KEY (id),
	CONSTRAINT anotacao_frequencia_aluno_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id),
	CONSTRAINT anotacao_frequencia_aluno_motivo_ausencia_fk FOREIGN KEY (motivo_ausencia_id) REFERENCES motivo_ausencia(id)
);


create index if not exists anotacao_frequencia_aluno_aula_idx on anotacao_frequencia_aluno USING btree (aula_id);
create index if not exists anotacao_frequencia_aluno_aluno_idx on anotacao_frequencia_aluno USING btree (codigo_aluno);