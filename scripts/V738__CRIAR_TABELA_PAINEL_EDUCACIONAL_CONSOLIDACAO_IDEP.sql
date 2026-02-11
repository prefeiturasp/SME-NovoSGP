BEGIN TRANSACTION;

CREATE TABLE painel_educacional_consolidacao_idep (
  id              SERIAL PRIMARY KEY,  
  ano_letivo      INT          NOT NULL,
  etapa           VARCHAR(20)  NOT NULL,
  faixa           VARCHAR(10)  NOT NULL,
  quantidade      INT          NOT NULL,
  codigo_dre      VARCHAR(50)  NOT NULL,
  media_geral     NUMERIC(5,2) NOT NULL,
  criado_em       TIMESTAMP    NOT NULL DEFAULT NOW(),
  criado_por      VARCHAR(200) NULL,
  criado_rf       VARCHAR(200) NULL,
  alterado_em     TIMESTAMP    NOT NULL DEFAULT NOW(),
  alterado_por    VARCHAR(200) NULL,
  alterado_rf     VARCHAR(200) NULL,
  
  CONSTRAINT uq_painel_ano_etapa_faixa UNIQUE (ano_letivo, etapa, faixa)
);

CREATE INDEX ix_painel_ano_etapa 
ON painel_educacional_consolidacao_idep (ano_letivo, etapa);

COMMIT;