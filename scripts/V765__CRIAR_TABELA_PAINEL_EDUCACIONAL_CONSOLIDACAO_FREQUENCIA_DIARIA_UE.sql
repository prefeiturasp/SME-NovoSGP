
    CREATE table if not exists public.painel_educacional_consolidacao_frequencia_diaria_ue (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_dre             VARCHAR(10) NOT  NULL,
    codigo_ue              VARCHAR(10) NOT  NULL,
    turma_id               int8 NOT  NULL,
    turma                  VARCHAR(50)  NOT NULL,
    nivel_frequencia       INT NOT  NULL,
    total_estudantes       INT NOT NULL,
    total_presentes        INT NOT NULL,
    percentual_frequencia  NUMERIC(5,2) NOT NULL,
    data_aula              TIMESTAMP    NOT NULL,
    ano_letivo             INT          NOT NULL,
    criado_em              TIMESTAMP    NOT NULL,
    
    CONSTRAINT uq_painel_educacional_consolidacao_frequencia_diaria_ue UNIQUE (turma_id, codigo_dre, codigo_ue, ano_letivo, data_aula)
    );
