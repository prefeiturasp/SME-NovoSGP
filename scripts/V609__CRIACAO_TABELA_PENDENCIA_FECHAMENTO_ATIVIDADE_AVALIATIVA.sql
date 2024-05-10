CREATE TABLE IF NOT EXISTS public.pendencia_fechamento_atividade_avaliativa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	atividade_avaliativa_id int8 NOT NULL,
	pendencia_fechamento_id int8 NOT NULL,
	CONSTRAINT pendencia_fechamento_atividade_avaliativa_pk PRIMARY KEY (id)
);

ALTER TABLE public.pendencia_fechamento_atividade_avaliativa ADD CONSTRAINT pendencia_fechamento_ativ_ativ_fk FOREIGN KEY (atividade_avaliativa_id) REFERENCES atividade_avaliativa(id);
ALTER TABLE public.pendencia_fechamento_atividade_avaliativa ADD CONSTRAINT pendencia_fechamento_ativ_pendenciafechamento_fk FOREIGN KEY (pendencia_fechamento_id) REFERENCES pendencia_fechamento(id);

CREATE INDEX pendencia_fechamento_ativ_ativ_idx ON public.pendencia_fechamento_atividade_avaliativa USING btree (atividade_avaliativa_id);
CREATE INDEX pendencia_fechamento_ativ_pendenciafechamento_idx ON public.pendencia_fechamento_atividade_avaliativa USING btree (pendencia_fechamento_id);