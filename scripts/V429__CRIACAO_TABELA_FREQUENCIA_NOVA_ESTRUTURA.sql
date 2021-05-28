CREATE TABLE IF NOT EXISTS public.registro_frequencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	
	valor int8 NOT NULL,	
    codigo_aluno varchar(15) NOT NULL,
	numero_aula int4 NOT NULL,
	registro_frequencia_id int8 NOT NULL,
	criado_em timestamp not NULL,
	criado_por varchar(200) not NULL,
	criado_rf varchar(200) not NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	
	CONSTRAINT registro_frequencia_aluno_pk PRIMARY KEY (id)
);

CREATE INDEX registro_frequencia_aluno_codigo_aluno_idx ON public.registro_frequencia_aluno USING btree (codigo_aluno);


ALTER TABLE public.registro_frequencia_aluno ADD CONSTRAINT registro_frequencia_aluno_registro_frequencia_fk FOREIGN KEY (registro_frequencia_id) REFERENCES public.registro_frequencia(id);	