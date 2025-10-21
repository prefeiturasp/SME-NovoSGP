CREATE TABLE IF NOT EXISTS public.painel_educacional_consolidacao_reclassificacao (
    id               BIGSERIAL       PRIMARY KEY,
    dre              VARCHAR(100)    NOT NULL,
    ue               VARCHAR(100)   NOT NULL,
    ano              INT             NOT NULL,
    modalidade_turma          INT             NOT NULL,
    quantidade_alunos_reclassificados INT             NOT NULL,
    criado_em        TIMESTAMP       NOT NULL
);
