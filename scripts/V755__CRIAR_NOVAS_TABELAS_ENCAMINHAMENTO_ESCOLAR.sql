-- ============================================================================
-- SCRIPT DE CRIAÇÃO: TABELAS ENCAMINHAMENTO ESCOLAR
-- ============================================================================
-- Ordem de criação respeitando dependências entre tabelas
-- Com verificação IF NOT EXISTS para execução segura
--
-- OBSERVAÇÃO IMPORTANTE:
-- As tabelas de seção (secao_encaminhamento_*, encaminhamento_*_secao) 
-- NÃO serão criadas pois serão compartilhadas com encaminhamento_naapa
-- ============================================================================

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

-- ============================================================================
-- TABELA 2: consolidado_encaminhamento_escolar
-- Dependências externas: ue
-- Tabela de consolidação/resumo
-- ============================================================================

CREATE TABLE IF NOT EXISTS public.consolidado_encaminhamento_escolar (
	id int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	ano_letivo int4 NOT NULL,
	ue_id int8 NOT NULL,
	quantidade int8 DEFAULT 0 NOT NULL,
	situacao int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	modalidade_codigo int4 NOT NULL,
	CONSTRAINT consolidado_encaminhamento_escolar_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS consolidado_encaminhamento_escolar_ue_idx ON public.consolidado_encaminhamento_escolar USING btree (ue_id);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'consolidado_encaminhamento_escolar_ue_fk') THEN
        ALTER TABLE public.consolidado_encaminhamento_escolar ADD CONSTRAINT consolidado_encaminhamento_escolar_ue_fk FOREIGN KEY (ue_id) REFERENCES public.ue(id);
    END IF;
END $$;

-- ============================================================================
-- TABELA 3: encaminhamento_escolar_observacao
-- Dependências: encaminhamento_escolar
-- ============================================================================

CREATE TABLE IF NOT EXISTS public.encaminhamento_escolar_observacao (
	id int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	encaminhamento_escolar_id int8 NOT NULL,
	observacao varchar NOT NULL,
	excluido bool DEFAULT false NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_escolar_observacao_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS encaminhamento_escolar_observacao_encaminhamento_idx ON public.encaminhamento_escolar_observacao USING btree (encaminhamento_escolar_id);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_observacao_encaminhamento_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_observacao ADD CONSTRAINT encaminhamento_escolar_observacao_encaminhamento_fk FOREIGN KEY (encaminhamento_escolar_id) REFERENCES public.encaminhamento_escolar(id);
    END IF;
END $$;

-- ============================================================================
-- TABELA 4: encaminhamento_escolar_questao
-- Dependências: encaminhamento_naapa_secao (tabela compartilhada), questao (externa)
-- IMPORTANTE: Esta tabela referencia encaminhamento_naapa_secao (estrutura compartilhada)
-- ============================================================================

CREATE TABLE IF NOT EXISTS public.encaminhamento_escolar_questao (
	id int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	encaminhamento_escolar_secao_id int8 NOT NULL,
	questao_id int8 NOT NULL,
	excluido bool DEFAULT false NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_escolar_questao_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS encaminhamento_escolar_questao_questao_idx ON public.encaminhamento_escolar_questao USING btree (questao_id);
CREATE INDEX IF NOT EXISTS encaminhamento_escolar_questao_secao_idx ON public.encaminhamento_escolar_questao USING btree (encaminhamento_escolar_secao_id);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_questao_questao_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_questao ADD CONSTRAINT encaminhamento_escolar_questao_questao_fk FOREIGN KEY (questao_id) REFERENCES public.questao(id);
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_questao_secao_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_questao ADD CONSTRAINT encaminhamento_escolar_questao_secao_fk FOREIGN KEY (encaminhamento_escolar_secao_id) REFERENCES public.encaminhamento_naapa_secao(id);
    END IF;
END $$;

-- ============================================================================
-- TABELA 5: encaminhamento_escolar_resposta
-- Dependências: encaminhamento_escolar_questao, opcao_resposta (externa), arquivo (externa)
-- ============================================================================

CREATE TABLE IF NOT EXISTS public.encaminhamento_escolar_resposta (
	id int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	questao_encaminhamento_id int8 NOT NULL,
	resposta_id int8 NULL,
	arquivo_id int8 NULL,
	texto varchar NULL,
	excluido bool DEFAULT false NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_escolar_resposta_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS encaminhamento_escolar_resposta_arquivo_idx ON public.encaminhamento_escolar_resposta USING btree (arquivo_id);
CREATE INDEX IF NOT EXISTS encaminhamento_escolar_resposta_questao_idx ON public.encaminhamento_escolar_resposta USING btree (questao_encaminhamento_id);
CREATE INDEX IF NOT EXISTS encaminhamento_escolar_resposta_resposta_idx ON public.encaminhamento_escolar_resposta USING btree (resposta_id);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_resposta_arquivo_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_resposta ADD CONSTRAINT encaminhamento_escolar_resposta_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id);
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_resposta_questao_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_resposta ADD CONSTRAINT encaminhamento_escolar_resposta_questao_fk FOREIGN KEY (questao_encaminhamento_id) REFERENCES public.encaminhamento_escolar_questao(id);
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_resposta_resposta_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_resposta ADD CONSTRAINT encaminhamento_escolar_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES public.opcao_resposta(id);
    END IF;
END $$;

-- ============================================================================
-- TABELA 6: encaminhamento_escolar_historico_alteracoes
-- Dependências: encaminhamento_escolar, secao_encaminhamento_naapa (tabela compartilhada), usuario (externa)
-- IMPORTANTE: Esta tabela referencia secao_encaminhamento_naapa (estrutura compartilhada)
-- ============================================================================

CREATE TABLE IF NOT EXISTS public.encaminhamento_escolar_historico_alteracoes (
	id int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	encaminhamento_escolar_id int8 NOT NULL,
	secao_encaminhamento_escolar_id int8 NULL,
	usuario_id int8 NOT NULL,
	campos_inseridos text NULL,
	campos_alterados text NULL,
	data_atendimento varchar(20) NULL,
	data_historico timestamp NOT NULL,
	tipo_historico int4 NOT NULL,
	CONSTRAINT encaminhamento_escolar_historico_alteracoes_pk PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS encaminhamento_escolar_historico_alteracoes_id_ix ON public.encaminhamento_escolar_historico_alteracoes USING btree (encaminhamento_escolar_id);
CREATE INDEX IF NOT EXISTS encaminhamento_escolar_historico_alteracoes_secao_id_ix ON public.encaminhamento_escolar_historico_alteracoes USING btree (secao_encaminhamento_escolar_id);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_historico_alteracoes_escolar_id_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_historico_alteracoes ADD CONSTRAINT encaminhamento_escolar_historico_alteracoes_escolar_id_fk FOREIGN KEY (encaminhamento_escolar_id) REFERENCES public.encaminhamento_escolar(id);
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_historico_alteracoes_escolar_secao_id_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_historico_alteracoes ADD CONSTRAINT encaminhamento_escolar_historico_alteracoes_escolar_secao_id_fk FOREIGN KEY (secao_encaminhamento_escolar_id) REFERENCES public.secao_encaminhamento_naapa(id);
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'encaminhamento_escolar_historico_alteracoes_usuario_id_fk') THEN
        ALTER TABLE public.encaminhamento_escolar_historico_alteracoes ADD CONSTRAINT encaminhamento_escolar_historico_alteracoes_usuario_id_fk FOREIGN KEY (usuario_id) REFERENCES public.usuario(id);
    END IF;
END $$;

-- ============================================================================
-- FIM DO SCRIPT - TABELAS
-- ============================================================================
-- Total: 6 Tabelas criadas
-- 
-- Ordem de criação (respeitando dependências):
-- 1. encaminhamento_escolar (tabela principal - relaciona com turma, dre, ue)
-- 2. consolidado_encaminhamento_escolar (consolidação)
-- 3. encaminhamento_escolar_observacao (depende de encaminhamento_escolar)
-- 4. encaminhamento_escolar_questao (depende de encaminhamento_naapa_secao)
-- 5. encaminhamento_escolar_resposta (depende de encaminhamento_escolar_questao)
-- 6. encaminhamento_escolar_historico_alteracoes (depende de encaminhamento_escolar e secao_encaminhamento_naapa)
--
-- TABELAS COMPARTILHADAS COM NAAPA (não criadas):
-- - secao_encaminhamento_naapa
-- - secao_encaminhamento_naapa_modalidade
-- - encaminhamento_naapa_secao
--
-- VIEWS E FUNCTIONS:
-- Não é necessário criar novas views e functions, pois as existentes da estrutura
-- NAAPA continuam funcionando com as tabelas compartilhadas.
-- ============================================================================