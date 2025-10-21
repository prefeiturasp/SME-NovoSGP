CREATE TABLE IF NOT EXISTS public.painel_educacional_consolidacao_reclassificacao (
    id               BIGSERIAL       PRIMARY KEY,
    codigo_dre      VARCHAR(20)     NOT NULL,
    codigo_ue       VARCHAR(20)     NOT NULL,
    ano_letivo              INT             NOT NULL,
    modalidade_turma          INT             NOT NULL,
    quantidade_alunos_reclassificados INT             NOT NULL,
    criado_em        TIMESTAMP       NOT NULL
);
