BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS painel_educacional_consolidacao_idep (
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
  alterado_rf     VARCHAR(200) NULL
);

DO $$ 
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'uq_painel_ano_etapa_faixa' 
        AND conrelid = 'painel_educacional_consolidacao_idep'::regclass
    ) THEN
        ALTER TABLE painel_educacional_consolidacao_idep
        ADD CONSTRAINT uq_painel_ano_etapa_faixa
        UNIQUE (ano_letivo, etapa, faixa);
    END IF;
END $$;

DO $$ 
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_indexes 
        WHERE indexname = 'ix_painel_ano_etapa' 
        AND tablename = 'painel_educacional_consolidacao_idep'
    ) THEN
        CREATE INDEX ix_painel_ano_etapa 
        ON painel_educacional_consolidacao_idep (ano_letivo, etapa);
    END IF;
END $$;

COMMIT;