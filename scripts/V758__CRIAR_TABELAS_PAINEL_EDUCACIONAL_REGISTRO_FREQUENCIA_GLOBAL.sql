  CREATE TABLE IF NOT EXISTS public.painel_educacional_registro_frequencia_agrupamento_global (
        id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
        codigo_dre VARCHAR(50),
        codigo_ue VARCHAR(50),
        modalidade VARCHAR(50),
        percentual_frequencia NUMERIC(5,2) NOT NULL,
        total_ausencias INT NOT NULL,
        total_alunos INT NOT NULL,
        total_aulas INT NOT NULL,
        ano_letivo INT NOT NULL,

        criado_em timestamp NOT NULL,

        CONSTRAINT uk_painel_educacional_registro_frequencia_agrupamento_global
            UNIQUE (codigo_dre, codigo_ue, modalidade,  ano_letivo)
    );
