CREATE TABLE public.atividade_infantil (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	aviso_classroom_id int8 NOT NULL,
    titulo varchar NOT NULL,
	mensagem varchar NOT NULL,
	email varchar NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT atividade_infantil_pk PRIMARY KEY (id)
);
CREATE INDEX atividade_infantil_aula_idx ON public.atividade_infantil USING btree (aula_id);
CREATE INDEX atividade_infantil_classroom_id_idx ON public.atividade_infantil USING btree (aviso_classroom_id);

ALTER TABLE public.atividade_infantil ADD CONSTRAINT atividade_infantil_aula_fk FOREIGN KEY (aula_id) REFERENCES public.aula(id);