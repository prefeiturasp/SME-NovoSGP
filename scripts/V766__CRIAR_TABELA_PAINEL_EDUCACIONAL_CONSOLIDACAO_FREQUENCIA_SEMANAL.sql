
    CREATE table if not exists public.painel_educacional_consolidacao_frequencia_semanal (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_dre             VARCHAR(10) NOT  NULL,
    codigo_ue              VARCHAR(10) NOT  NULL,
    total_estudantes       INT NOT NULL,
    total_presentes        INT NOT NULL,
    percentual_frequencia  NUMERIC(5,2) NOT NULL,
    data_aula              TIMESTAMP    NOT NULL,
    ano_letivo             INT          NOT NULL,
    criado_em              TIMESTAMP    NOT NULL,
    
    CONSTRAINT painel_educacional_consolidacao_frequencia_semanal_ue UNIQUE (id)
    );