DROP TABLE IF exists  painel_educacional_consolidacao_pap_ue;
CREATE TABLE IF NOT exists  painel_educacional_consolidacao_pap_ue (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
    tipo_pap smallint NOT NULL,
	dre_codigo VARCHAR(15) NOT NULL,
	ue_codigo VARCHAR(15) NOT NULL,
    total_turmas INT NOT NULL,
    total_alunos INT NOT NULL,
    total_alunos_com_frequencia_inferior_limite INT NOT NULL,
    total_alunos_dificuldade_top_1 INT NOT NULL,
    total_alunos_dificuldade_top_2 INT NOT NULL,
    total_alunos_dificuldade_outras INT NOT NULL,
    nome_dificuldade_top_1 VARCHAR(200) NOT NULL,
    nome_dificuldade_top_2 VARCHAR(200) NOT NULL,
	criado_em timestamp NOT NULL,
	CONSTRAINT painel_educacional_consolidacao_pap_ue_pk PRIMARY KEY (id)
);
DROP INDEX IF EXISTS idx_painel_educacional_consolidacao_pap_ue;
CREATE INDEX IF NOT EXISTS idx_painel_educacional_consolidacao_pap_ue ON painel_educacional_consolidacao_pap_ue (ano_letivo, dre_codigo, ue_codigo);
DROP TABLE IF EXISTS painel_educacional_consolidacao_pap_dre;
CREATE TABLE IF NOT EXISTS painel_educacional_consolidacao_pap_dre (
    id int4 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
    tipo_pap smallint NOT NULL,
	dre_codigo VARCHAR(15) NOT null,
    total_turmas INT NOT NULL,
    total_alunos INT NOT NULL,
    total_alunos_com_frequencia_inferior_limite INT NOT NULL,
    total_alunos_dificuldade_top_1 INT NOT NULL,
    total_alunos_dificuldade_top_2 INT NOT NULL,
    total_alunos_dificuldade_outras INT NOT NULL,
    nome_dificuldade_top_1 VARCHAR(200) NOT NULL,
    nome_dificuldade_top_2 VARCHAR(200) NOT NULL,
	criado_em timestamp NOT NULL,
	CONSTRAINT painel_educacional_consolidacao_pap_dre_pk PRIMARY KEY (id)
);
DROP INDEX IF EXISTS idx_painel_educacional_consolidacao_pap_dre;
CREATE INDEX IF NOT EXISTS idx_painel_educacional_consolidacao_pap_dre ON painel_educacional_consolidacao_pap_dre (ano_letivo, dre_codigo);
DROP TABLE IF EXISTS painel_educacional_consolidacao_pap_sme;
CREATE TABLE IF NOT EXISTS painel_educacional_consolidacao_pap_sme (
    id int4 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
    tipo_pap smallint NOT NULL,
    total_turmas INT NOT NULL,
    total_alunos INT NOT NULL,
    total_alunos_com_frequencia_inferior_limite INT NOT NULL,
    total_alunos_dificuldade_top_1 INT NOT NULL,
    total_alunos_dificuldade_top_2 INT NOT NULL,
    total_alunos_dificuldade_outras INT NOT NULL,
    nome_dificuldade_top_1 VARCHAR(200) NOT NULL,
    nome_dificuldade_top_2 VARCHAR(200) NOT NULL,
	criado_em timestamp NOT NULL,
	CONSTRAINT painel_educacional_consolidacao_pap_sme_pk PRIMARY KEY (id)
);
DROP INDEX IF EXISTS idx_painel_educacional_consolidacao_pap_sme;
CREATE INDEX IF NOT EXISTS idx_painel_educacional_consolidacao_pap_sme ON painel_educacional_consolidacao_pap_sme (ano_letivo);
