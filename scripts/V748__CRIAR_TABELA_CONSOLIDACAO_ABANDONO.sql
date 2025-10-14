CREATE table if not exists public.painel_educacional_consolidacao_abandono (
  id int8  NOT NULL GENERATED ALWAYS AS IDENTITY,
  ano_letivo      INT          NOT NULL,
  codigo_dre VARCHAR(15) NOT NULL,
  ano			  VARCHAR(5)   NOT NULL,
  quantidade_desistencias      INT      NOT NULL,
  modalidade     VARCHAR(50)  NOT NULL,
  criado_em       timestamptz    NOT NULL,
  CONSTRAINT uq_painel_abandono UNIQUE (id, ano_letivo, codigo_dre, modalidade, ano)
);

CREATE TABLE IF NOT exists  painel_educacional_consolidacao_abandono_ue (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NOT NULL,
	codigo_dre VARCHAR(15) NOT NULL,
	codigo_ue VARCHAR(15) NOT NULL,
    codigo_turma VARCHAR(15) NOT NULL,
    nome_turma VARCHAR(100) NOT NULL,
    modalidade VARCHAR(50) NOT NULL,
    quantidade_desistencias int4 NOT NULL,
	criado_em timestamptz NOT NULL,
	CONSTRAINT uq_painel_abandono_ue UNIQUE (id, ano_letivo, codigo_dre, codigo_ue, modalidade, codigo_turma)
);