CREATE table IF NOT EXISTS public.consolidado_atendimento_naapa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY ,
	ano_letivo int4 NOT NULL,
	mes int4 NOT NULL,
	nome_profissional varchar not null,
	rf_profissional varchar not null,
	ue_id int8 NOT NULL,
	quantidade int8 NOT NULL DEFAULT 0,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT consolidado_atendimento_naapa_pk PRIMARY KEY (id)
);
CREATE INDEX consolidado_atendimento_naapa_ue_idx ON public.consolidado_atendimento_naapa USING btree (ue_id);
ALTER TABLE public.consolidado_atendimento_naapa ADD CONSTRAINT consolidado_atendimento_naapa_ue_fk 
FOREIGN KEY (ue_id) REFERENCES public.ue(id);