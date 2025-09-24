CREATE TABLE IF NOT EXISTS public.consolidacao_painel_educacional_fluencia_leitora (
    id               BIGSERIAL       PRIMARY KEY,
    fluencia         VARCHAR(50)     NOT NULL,
    descricao_fluencia         VARCHAR(100),
    percentual       NUMERIC(11,2)   NOT NULL,
    quantidade_alunos INT             NOT NULL,
    ano              INT             NOT NULL,
    periodo          INT             NOT NULL,
    criado_em        TIMESTAMP       NOT NULL DEFAULT now(),
    criado_por       VARCHAR(200)    NOT NULL,
    alterado_em      TIMESTAMP       NULL,
    alterado_por     VARCHAR(200)    NULL,
    criado_rf        VARCHAR(200)    NOT NULL,
    alterado_rf      VARCHAR(200)    NULL
);

CREATE INDEX IF NOT EXISTS idx_consolidacao_fluencia_ano_periodo
    ON public.consolidacao_painel_educacional_fluencia_leitora (ano, periodo, fluencia);
