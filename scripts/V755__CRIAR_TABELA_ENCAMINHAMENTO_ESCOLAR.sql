-- ============================================================================
-- TABELA 1: encaminhamento_escolar
-- Dependências externas: turma, dre, ue
-- Esta é a tabela principal do módulo
-- ============================================================================

CREATE TABLE IF NOT EXISTS public.encaminhamento_escolar (
 id int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
 turma_id int8 NULL,
 aluno_codigo varchar(15) NULL,
 aluno_nome varchar NULL,
 dre_id int8 NULL,
 ue_id int8 NULL,
 tipo int4 NOT NULL,
 situacao int4 NOT NULL,
 excluido bool DEFAULT false NOT NULL,
 criado_em timestamp NOT NULL,
 criado_por varchar(200) NOT NULL,
 alterado_em timestamp NULL,
 alterado_por varchar(200) NULL,
 criado_rf varchar(200) NOT NULL,
 alterado_rf varchar(200) NULL,
 situacao_matricula_aluno int4 NULL,
 motivo_encerramento varchar(500) NULL,
 data_ultima_notificacao_sem_atendimento date NULL,
 CONSTRAINT encaminhamento_escolar_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS encaminhamento_escolar_aluno_idx ON public.encaminhamento_escolar USING btree (aluno_codigo);
CREATE INDEX IF NOT EXISTS encaminhamento_escolar_turma_idx ON public.encaminhamento_escolar USING btree (turma_id);
CREATE INDEX IF NOT EXISTS encaminhamento_escolar_dre_idx ON public.encaminhamento_escolar USING btree (dre_id);
CREATE INDEX IF NOT EXISTS encaminhamento_escolar_ue_idx ON public.encaminhamento_escolar USING btree (ue_id);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_turma_fk') THEN
        ALTER TABLE public.encaminhamento_escolar ADD CONSTRAINT encaminhamento_escolar_turma_fk FOREIGN KEY (turma_id) REFERENCES public.turma(id);
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_dre_fk') THEN
        ALTER TABLE public.encaminhamento_escolar ADD CONSTRAINT encaminhamento_escolar_dre_fk FOREIGN KEY (dre_id) REFERENCES public.dre(id);
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_ue_fk') THEN
        ALTER TABLE public.encaminhamento_escolar ADD CONSTRAINT encaminhamento_escolar_ue_fk FOREIGN KEY (ue_id) REFERENCES public.ue(id);
    END IF;
END $$;