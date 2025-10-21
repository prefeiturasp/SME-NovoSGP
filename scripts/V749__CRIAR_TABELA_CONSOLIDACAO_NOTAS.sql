DROP TABLE IF exists  painel_educacional_consolidacao_nota_ue;
CREATE TABLE IF NOT exists  painel_educacional_consolidacao_nota_ue (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
    codigo_dre VARCHAR(15) NOT NULL,
	codigo_ue VARCHAR(15) NOT NULL,
    bimestre smallint NULL,
    modalidade int4 NOT NULL,
    serie_turma VARCHAR(20) NOT NULL,
    quantidade_abaixo_media_portugues int NOT NULL,
    quantidade_acima_media_portugues int NOT NULL,
    quantidade_abaixo_media_matematica int NOT NULL,
    quantidade_acima_media_matematica int NOT NULL,
    quantidade_abaixo_media_ciencias int NOT NULL,
    quantidade_acima_media_ciencias int NOT NULL,
	criado_em timestamptz NOT NULL,
	CONSTRAINT painel_educacional_consolidacao_nota_ue_pk PRIMARY KEY (id)
);
DROP INDEX IF EXISTS idx_painel_educacional_consolidacao_nota_ue;
CREATE INDEX IF NOT EXISTS idx_painel_educacional_consolidacao_nota_ue ON painel_educacional_consolidacao_nota_ue (ano_letivo, codigo_dre, codigo_ue, bimestre, modalidade, serie_turma);
DROP TABLE IF EXISTS painel_educacional_consolidacao_nota;
CREATE TABLE IF NOT EXISTS painel_educacional_consolidacao_nota (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
    codigo_dre VARCHAR(15) NOT null,
    bimestre smallint NULL,
    modalidade int4 NOT NULL,
    ano_turma CHAR(1) NULL,
    quantidade_abaixo_media_portugues int NOT NULL,
    quantidade_acima_media_portugues int NOT NULL,
    quantidade_abaixo_media_matematica int NOT NULL,
    quantidade_acima_media_matematica int NOT NULL,
    quantidade_abaixo_media_ciencias int NOT NULL,
    quantidade_acima_media_ciencias int NOT NULL,
	criado_em timestamptz NOT NULL,
	CONSTRAINT painel_educacional_consolidacao_nota_pk PRIMARY KEY (id)
);
DROP INDEX IF EXISTS idx_painel_educacional_consolidacao_nota;
CREATE INDEX IF NOT EXISTS idx_painel_educacional_consolidacao_nota ON painel_educacional_consolidacao_nota (ano_letivo, codigo_dre, bimestre, modalidade, ano_turma);