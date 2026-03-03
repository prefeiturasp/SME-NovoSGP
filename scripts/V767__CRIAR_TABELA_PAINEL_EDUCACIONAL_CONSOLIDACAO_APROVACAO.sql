    CREATE table if not exists public.painel_educacional_consolidacao_aprovacao (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_dre             VARCHAR(10) NOT  NULL,
    serie_ano              VARCHAR(10) NOT NULL,
    modalidade             VARCHAR(50) NOT NULL,
    total_promocoes        INT      NOT NULL,
    total_retencoes_ausencias   INT      NOT NULL,
    total_retencoes_notas   INT      NOT NULL,
    ano_letivo             INT         NOT NULL,
    criado_em              TIMESTAMP   NOT NULL,
    
    CONSTRAINT painel_educacional_consolidacao_aprovacao_dre UNIQUE (id)
    );