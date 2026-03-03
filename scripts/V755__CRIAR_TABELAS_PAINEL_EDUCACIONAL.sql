begin
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



    CREATE INDEX idx_cfam_turma_ano ON turma (ano_letivo);
    CREATE INDEX idx_cfam_mes ON consolidacao_frequencia_aluno_mensal (mes);
    CREATE INDEX idx_cfam_turma_id ON consolidacao_frequencia_aluno_mensal (turma_id);
    CREATE INDEX idx_ue_dre_id ON ue (dre_id);




    CREATE TABLE IF NOT EXISTS public.taxa_alfabetizacao (
        id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
        codigo_eol_escola        VARCHAR(15)   NOT NULL,
        taxa   NUMERIC(11,2) NOT NULL,
        ano_letivo           BIGINT        NOT NULL,
        criado_em            TIMESTAMP     NOT NULL,
        criado_por           VARCHAR(200)  NOT NULL,
        alterado_em          TIMESTAMP,
        alterado_por         VARCHAR(200),
        criado_rf           VARCHAR(200)  NOT NULL,
        alterado_rf          VARCHAR(200),
        
        CONSTRAINT uk_ptaxa_alfabetizacao
            UNIQUE (codigo_eol_escola,  ano_letivo)
    );


    CREATE TABLE IF NOT EXISTS public.painel_educacional_consolidacao_taxa_alfabetizacao (
        id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
        codigo_dre        VARCHAR(15)   NOT NULL,
        codigo_ue        VARCHAR(15)   NOT NULL,
        ano_letivo           INT        NOT NULL,
        taxa   NUMERIC(11,2) NOT NULL,
        criado_em            TIMESTAMP     NOT NULL,

        CONSTRAINT uk_painel_educacional_consolidacao_taxa_alfabetizacao
            UNIQUE (codigo_dre, codigo_ue,  ano_letivo)
    );
    
    
    
    
    
    CREATE table if not exists public.painel_educacional_consolidacao_ideb (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo      INT          NOT NULL,
    codigo_dre      VARCHAR(50)  NOT NULL,
    codigo_ue       VARCHAR(50)  NULL,
    etapa           INT  NOT NULL,
    faixa           VARCHAR(10)  NOT NULL,
    quantidade      INT          NOT NULL,
    media_geral     NUMERIC(5,2) NOT NULL,
    criado_em       TIMESTAMP     NOT NULL,
    
    CONSTRAINT uq_painel_ideb_ano_etapa_faixa UNIQUE (ano_letivo, codigo_dre, codigo_ue, etapa, faixa)
    );
    
    
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
    
    
    
    CREATE table if not exists public.painel_educacional_consolidacao_idep (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    ano_letivo      INT          NOT NULL,
    codigo_dre      VARCHAR(50)  NOT NULL,
    codigo_ue       VARCHAR(50)  NULL,
    etapa           INT          NOT NULL,
    faixa           VARCHAR(10)  NOT NULL,
    quantidade      INT          NOT NULL,
    media_geral     NUMERIC(5,2) NOT NULL,
    criado_em       TIMESTAMP    NOT NULL,
    
    CONSTRAINT uq_painel_educacional_consolidacao_idep UNIQUE (ano_letivo, codigo_dre, codigo_ue, etapa, faixa)
    );


    CREATE table if not exists public.painel_educacional_consolidacao_frequencia_diaria (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_dre             VARCHAR(10) NOT  NULL,
    codigo_ue              VARCHAR(10) NOT  NULL,
    ue                     VARCHAR(500)  NOT NULL,
    nivel_frequencia       INT NOT  NULL,
    total_estudantes       INT NOT NULL,
    total_presentes        INT NOT NULL,
    percentual_frequencia  NUMERIC(5,2) NOT NULL,
    data_aula              TIMESTAMP    NOT NULL,
    ano_letivo             INT          NOT NULL,
    criado_em              TIMESTAMP    NOT NULL
    );


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


    CREATE table if not exists public.painel_educacional_consolidacao_aprovacao (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_dre             VARCHAR(10) NOT  NULL,
    serie_ano              VARCHAR(10) NOT NULL,
    modalidade             VARCHAR(50) NOT NULL,
    total_promocoes        INT      NOT NULL,
    total_retencoes_ausencias   INT      NOT NULL,
    total_retencoes_notas   INT      NOT NULL,
    ano_letivo             INT         NOT NULL,
    criado_em              TIMESTAMP   NOT NULL,
    
    CONSTRAINT painel_educacional_consolidacao_aprovacao_dre UNIQUE (id)
    );

    CREATE table if not exists public.painel_educacional_consolidacao_aprovacao_ue (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_dre             VARCHAR(10) NOT  NULL,
    codigo_ue             VARCHAR(10) NOT  NULL,
    turma              VARCHAR(50) NOT NULL,
    modalidade_codigo      INT NOT NULL,
    modalidade             VARCHAR(50) NOT NULL,
    total_promocoes        INT      NOT NULL,
    total_retencoes_ausencias   INT      NOT NULL,
    total_retencoes_notas   INT      NOT NULL,
    ano_letivo             INT         NOT NULL,
    criado_em              TIMESTAMP   NOT NULL,
    
    CONSTRAINT painel_educacional_consolidacao_aprovacao_ue_turma UNIQUE (id)
    );
COMMIT;