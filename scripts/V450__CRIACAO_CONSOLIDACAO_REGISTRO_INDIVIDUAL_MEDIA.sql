DROP TABLE if exists public.consolidacao_registro_individual_media;

CREATE TABLE public.consolidacao_registro_individual_media (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	turma_id int8 NOT NULL,
	quantidade int8 NOT null default 0,
	
    CONSTRAINT consolidacao_registro_individual_media_pk PRIMARY KEY (id)
);

ALTER TABLE public.consolidacao_registro_individual_media ADD CONSTRAINT consolidacao_registro_individual_media_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
CREATE INDEX consolidacao_registro_individual_media_turma_idx ON public.consolidacao_registro_individual_media USING btree (turma_id);