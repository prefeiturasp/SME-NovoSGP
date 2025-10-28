CREATE TABLE IF NOT exists  painel_educacional_consolidacao_proficiencia_ideb_ue (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
    codigo_ue VARCHAR(15) NOT NULL,
	serie_ano smallint NOT NULL,
    componente_curricular_id smallint NULL,
    nota numeric(4,2) NULL,
    proficiencia numeric (5,2) null,
    boletim varchar(500) NULL,
	criado_em timestamptz NOT NULL,
	CONSTRAINT painel_educacional_consolidacao_proficiencia_ideb_pk PRIMARY KEY (id)
);
CREATE INDEX IF NOT EXISTS idx_painel_educacional_consolidacao_proficiencia_ideb_ue on painel_educacional_consolidacao_proficiencia_ideb_ue (ano_letivo, codigo_ue);
CREATE TABLE IF NOT exists  painel_educacional_consolidacao_proficiencia_idep_ue (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
    codigo_ue VARCHAR(15) NOT NULL,
	serie_ano smallint NOT NULL,
    componente_curricular_id smallint NULL,
    nota numeric(4,2) NULL,
    proficiencia numeric (5,2) null,
    boletim varchar(500) NULL,
	criado_em timestamptz NOT NULL,
	CONSTRAINT painel_educacional_consolidacao_proficiencia_idep_pk PRIMARY KEY (id)
);
CREATE INDEX IF NOT EXISTS idx_painel_educacional_consolidacao_proficiencia_idep_ue on painel_educacional_consolidacao_proficiencia_idep_ue (ano_letivo, codigo_ue);