
CREATE table IF NOT EXISTS public.consolidado_encaminhamento_naapa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ano_letivo int4 NOT NULL,
	ue_id int8 NOT NULL,
	quantidade int8 NOT NULL DEFAULT 0,
	situacao int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT consolidado_encaminhamento_naapa_pk PRIMARY KEY (id)
);
CREATE INDEX consolidado_encaminhamento_naapa_ue_idx ON public.consolidado_encaminhamento_naapa USING btree (ue_id);

ALTER TABLE public.consolidado_encaminhamento_naapa ADD CONSTRAINT consolidado_encaminhamento_naapa_ue_fk 
FOREIGN KEY (ue_id) REFERENCES public.ue(id);