CREATE TABLE IF NOT EXISTS public.pendencia_fechamento_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	pendencia_fechamento_id int8 NOT NULL,
	CONSTRAINT pendencia_fechamento_aula_pk PRIMARY KEY (id)
);

ALTER TABLE public.pendencia_fechamento_aula ADD CONSTRAINT pendencia_fechamento_aula_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id);
ALTER TABLE public.pendencia_fechamento_aula ADD CONSTRAINT pendencia_fechamento_aula_pendenciafechamento_fk FOREIGN KEY (pendencia_fechamento_id) REFERENCES pendencia_fechamento(id);

CREATE INDEX pendencia_fechamento_aula_aula_idx ON public.pendencia_fechamento_aula USING btree (aula_id);
CREATE INDEX pendencia_fechamento_aula_pendenciafechamento_idx ON public.pendencia_fechamento_aula USING btree (pendencia_fechamento_id);