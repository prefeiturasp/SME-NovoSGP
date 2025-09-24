CREATE TABLE IF NOT EXISTS public."taxa-alfabetizacao" (
    "codigo-escola"        VARCHAR(15)   NOT NULL,
    "taxa-alfabetizacao"   NUMERIC(11,2) NOT NULL,
    "ano-letivo"           BIGINT        NOT NULL,
    "criado_em"            TIMESTAMP     NOT NULL,
    "criado_por"           VARCHAR(200)  NOT NULL,
    "alterado_em"          TIMESTAMP     NOT NULL,
    "alterado_por"         VARCHAR(200),
    "criado_rf"            VARCHAR(200)  NOT NULL,
    "alterado_rf"          VARCHAR(200)
);
