CREATE table IF NOT EXISTS public.painel_educacional_consolidacao_distorcao_serie_idade (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo int4 NULL,
    codigo_dre varchar(200) NOT NULL,
    codigo_ue varchar(200) NOT NULL,
    modalidade varchar(200) NOT NULL,
    ano varchar(200) NOT NULL,
    quantidade_alunos int4 NULL,
    criado_em timestamp without time zone NOT NULL,
    CONSTRAINT painel_educacional_consolidacao_distorcao_serie_idade_pk PRIMARY KEY (id)
);