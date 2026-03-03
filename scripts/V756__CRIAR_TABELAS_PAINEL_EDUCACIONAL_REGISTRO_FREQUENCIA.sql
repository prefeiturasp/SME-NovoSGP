    CREATE TABLE IF NOT EXISTS public.painel_educacional_registro_frequencia_agrupamento_escola (

        id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
        codigo_dre VARCHAR(50),
        codigo_ue VARCHAR(50),
        dre VARCHAR(500),
        ue VARCHAR(500),
        total_aulas INT NOT NULL,
        percentual_frequencia NUMERIC(5,2) NOT NULL,
        total_ausencias INT NOT NULL,
        total_alunos INT NOT NULL,
        mes INT NOT NULL,
        ano_letivo INT NOT NULL,


        criado_em timestamp NOT NULL,

        CONSTRAINT uk_painel_educacional_registro_frequencia_agrupamento_escola
            UNIQUE (codigo_dre, codigo_ue,  ano_letivo, mes)
    );
