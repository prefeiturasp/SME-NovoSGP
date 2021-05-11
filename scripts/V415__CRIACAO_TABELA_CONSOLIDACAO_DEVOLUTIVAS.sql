DROP TABLE if exists public.consolidacao_devolutivas;

CREATE TABLE public.consolidacao_devolutivas (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	turma_id int8 NOT NULL,
	turma_id ano_letivo NOT NULL,
	quantidade_estimada_devolutivas int8 NOT null default 0,
	quantidade_registrada_devolutivas int8 NOT null default 0,
	
	quantidade_com_devolutiva int8 NOT null default 0,
	quantidade_com_devolutiva_pendente int8 NOT null default 0,
	
	quantidade_com_reflexoes_planejamentos_preenchidos int8 NOT null default 0,
	quantidade_sem_reflexoes_planejamentos_preenchidos int8 NOT null default 0,
	
    CONSTRAINT consolidacao_devolutivas_pk PRIMARY KEY (id)
);

ALTER TABLE public.consolidacao_devolutivas ADD CONSTRAINT consolidacao_devolutivas_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
CREATE INDEX consolidacao_devolutivas_idx ON public.consolidacao_devolutivas USING btree (turma_id);
