drop table if exists public.aviso;

CREATE TABLE public.aviso (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,

	aula_id int8 not null,
	aviso_classroom_id int8 not null,
	mensagem varchar not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT avisos_pk PRIMARY KEY (id)
);

ALTER TABLE public.aviso ADD CONSTRAINT avisos_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id);
CREATE INDEX aviso_aula_idx ON public.aviso USING btree (aula_id);
CREATE INDEX aviso_classroom_id_idx ON public.aviso USING btree (aviso_classroom_id);
