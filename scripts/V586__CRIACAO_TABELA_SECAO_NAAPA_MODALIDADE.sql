drop table if exists secao_encaminhamento_naapa_modalidade;

-- SECAO Encaminhamento NAAPA
CREATE table public.secao_encaminhamento_naapa_modalidade (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	secao_encaminhamento_id int8 not null,
	modalidade_codigo integer,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT secao_encaminhamento_naapa_modalidade_pk PRIMARY KEY (id)
);

CREATE INDEX secao_encaminhamento_naapa_modalidade_secao_idx ON public.secao_encaminhamento_naapa_modalidade USING btree (secao_encaminhamento_id);
ALTER TABLE public.secao_encaminhamento_naapa_modalidade ADD CONSTRAINT secao_encaminhamento_naapa_modalidade_secao_fk FOREIGN KEY (secao_encaminhamento_id) REFERENCES secao_encaminhamento_naapa(id);

CREATE INDEX secao_encaminhamento_naapa_modalidade_codigo_idx ON public.secao_encaminhamento_naapa_modalidade USING btree (modalidade_codigo);