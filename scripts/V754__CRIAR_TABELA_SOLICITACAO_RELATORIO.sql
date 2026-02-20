CREATE TABLE IF NOT EXISTS solicitacao_relatorio (
    id int8 GENERATED ALWAYS AS IDENTITY,
    filtros_usados varchar NOT NULL,
    extensao_relatorio int4 NOT NULL,
    relatorio int4 NOT NULL,
    usuario_que_solicitou varchar NOT NULL,
    status_solicitacao int4 NOT NULL,
    excluido bool DEFAULT false NOT NULL,
    criado_em timestamp NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp NULL,
    alterado_por varchar(200) NULL,
    criado_rf varchar(200) NOT NULL,
    alterado_rf varchar(200) NULL,
    CONSTRAINT solicitacao_relatorios_pk PRIMARY KEY (id)
);