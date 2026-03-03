    CREATE table if not exists public.painel_educacional_consolidacao_idep (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo      INT          NOT NULL,
    codigo_dre      VARCHAR(50)  NOT NULL,
    codigo_ue       VARCHAR(50)  NULL,
    etapa           INT          NOT NULL,
    faixa           VARCHAR(10)  NOT NULL,
    quantidade      INT          NOT NULL,
    media_geral     NUMERIC(5,2) NOT NULL,
    criado_em       TIMESTAMP    NOT NULL,
    
    CONSTRAINT uq_painel_educacional_consolidacao_idep UNIQUE (ano_letivo, codigo_dre, codigo_ue, etapa, faixa)
    );
