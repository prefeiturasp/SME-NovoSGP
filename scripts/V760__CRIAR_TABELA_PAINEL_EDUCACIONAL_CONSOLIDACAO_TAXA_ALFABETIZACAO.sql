
    CREATE TABLE IF NOT EXISTS public.painel_educacional_consolidacao_taxa_alfabetizacao (
        id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
        codigo_dre        VARCHAR(15)   NOT NULL,
        codigo_ue        VARCHAR(15)   NOT NULL,
        ano_letivo           INT        NOT NULL,
        taxa   NUMERIC(11,2) NOT NULL,
        criado_em            TIMESTAMP     NOT NULL,

        CONSTRAINT uk_painel_educacional_consolidacao_taxa_alfabetizacao
            UNIQUE (codigo_dre, codigo_ue,  ano_letivo)
    );
