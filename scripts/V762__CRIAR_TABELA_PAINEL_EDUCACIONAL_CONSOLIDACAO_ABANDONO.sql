   CREATE table if not exists public.painel_educacional_consolidacao_abandono (
    id int8 					   NOT NULL ,
    ano_letivo      INT          NOT NULL,
    codigo_dre      VARCHAR(50)  NOT NULL,
    ano			  VARCHAR(5)   NOT NULL,
    quantidade_desistencias      INT      NOT NULL,
    modalidade     VARCHAR(50)  NOT NULL,
    criado_em       TIMESTAMP    NOT NULL,
    
    CONSTRAINT uq_painel_abandono UNIQUE (id, ano_letivo, codigo_dre, modalidade, ano)
    );
    