insert into
    public.parametros_sistema (
        tipo,
        nome,
        descricao,
        valor,
        criado_em,
        criado_por,
        criado_rf
    )
select
    22,
    'DataUltimaAtualizacaoObjetivosJurema',
    'Data da última atualizacao dos objetivos da api do Jurema',
    '1900-01-01',
    now(),
    'Carga Inicial',
    'Carga Inicial'
where
    not exists(
        select
            1
        from
            public.parametros_sistema
        where
            tipo = 22
            and nome = 'DataUltimaAtualizacaoObjetivosJurema'
    );

CREATE TABLE if not exists public.objetivo_aprendizagem (
    id int8 NOT NULL,
    descricao varchar(1000) NOT NULL,
    codigo varchar(20) NOT NULL,
    ano_turma varchar(10) NOT NULL,
    componente_curricular_id int8 NOT NULL,
    excluido bool NOT NULL DEFAULT false,
    criado_em date NOT NULL,
    atualizado_em date NOT NULL,
    CONSTRAINT objetivo_aprendizagem_pk PRIMARY KEY (id)
);