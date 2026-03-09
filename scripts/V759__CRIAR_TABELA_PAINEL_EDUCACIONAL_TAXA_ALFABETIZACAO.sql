
    CREATE TABLE IF NOT EXISTS public.taxa_alfabetizacao (
        id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
        codigo_eol_escola        VARCHAR(15)   NOT NULL,
        taxa   NUMERIC(11,2) NOT NULL,
        ano_letivo           BIGINT        NOT NULL,
        criado_em            TIMESTAMP     NOT NULL,
        criado_por           VARCHAR(200)  NOT NULL,
        alterado_em          TIMESTAMP,
        alterado_por         VARCHAR(200),
        criado_rf           VARCHAR(200)  NOT NULL,
        alterado_rf          VARCHAR(200),
        
        CONSTRAINT uk_ptaxa_alfabetizacao
            UNIQUE (codigo_eol_escola,  ano_letivo)
    );