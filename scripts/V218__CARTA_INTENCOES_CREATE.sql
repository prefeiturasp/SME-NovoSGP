-- Data de criação: 07/08/2020
-- Descrição: Cria tabela par armazenamento das cartas de intenções

DROP TABLE IF EXISTS public.carta_intencoes;

CREATE TABLE public.carta_intencoes (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	periodo_escolar_id int8 not null,
	componente_curricular_id int8 not null,
	planejamento varchar not null,

	excluido bool NOT NULL DEFAULT false,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT carta_intencoes_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS carta_intencoes_turma_idx ON public.carta_intencoes USING btree (turma_id);
ALTER TABLE public.carta_intencoes DROP CONSTRAINT IF EXISTS turma_fk;
ALTER TABLE public.carta_intencoes ADD CONSTRAINT turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX IF NOT EXISTS carta_intencoes_periodo_escolar_idx ON public.carta_intencoes USING btree (periodo_escolar_id);
ALTER TABLE public.carta_intencoes DROP CONSTRAINT IF EXISTS periodo_escolar_fk;
ALTER TABLE public.carta_intencoes ADD CONSTRAINT periodo_escolar_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id);
