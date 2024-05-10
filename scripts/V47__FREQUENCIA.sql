CREATE TABLE if not exists public.registro_frequencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT registro_frequencia_pk PRIMARY KEY (id)
);
select f_cria_fk_se_nao_existir('registro_frequencia', 'registro_frequencia_aula_fk', 'FOREIGN KEY (aula_id) REFERENCES aula(id)');

CREATE INDEX if not exists registro_frequencia_aula_id_idx ON public.registro_frequencia USING btree (aula_id);



CREATE TABLE if not exists public.registro_ausencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno varchar(7) NOT NULL,
	numero_aula int4 NOT NULL,
	registro_frequencia_id int8 NOT NULL,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT registro_ausencia_aluno_pk PRIMARY KEY (id)
);
CREATE INDEX if not exists registro_ausencia_aluno_registro_frequencia_id_idx ON public.registro_ausencia_aluno USING btree (registro_frequencia_id);


select f_cria_fk_se_nao_existir('registro_ausencia_aluno', 'registro_ausencia_aluno_fk', 'FOREIGN KEY (registro_frequencia_id) REFERENCES registro_frequencia(id)');