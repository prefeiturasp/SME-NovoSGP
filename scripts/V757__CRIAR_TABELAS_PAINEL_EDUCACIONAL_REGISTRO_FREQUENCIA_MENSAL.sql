
    CREATE TABLE IF NOT EXISTS public.painel_educacional_registro_frequencia_agrupamento_mensal (
        id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
        codigo_dre VARCHAR(50),
        codigo_ue VARCHAR(50),
        modalidade VARCHAR(50),
        ano_letivo INT NOT NULL,
        mes INT NOT NULL,
        total_aulas INT NOT NULL,
        total_faltas INT NOT NULL,
        percentual_frequencia NUMERIC(5,2) NOT NULL,


        criado_em timestamp NOT NULL,

        CONSTRAINT uk_frequencia_agrupamento
            UNIQUE (codigo_dre, codigo_ue, modalidade, ano_letivo, mes)
    );
          