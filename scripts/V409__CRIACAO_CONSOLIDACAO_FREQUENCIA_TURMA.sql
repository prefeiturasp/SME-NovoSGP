CREATE TABLE if not exists public.consolidacao_frequencia_turma (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
    modalidade int2 NOT NULL,
	ano_letivo int2 NOT NULL,
    semestre int not null,
	codigo_dre varchar,
	sigla_dre varchar,
	codigo_ue varchar,
	sigla_ue varchar,
	codigo_turma varchar,
	quantidade_acima_minimo_frequencia int8 NOT null default 0,
	quantidade_abaixo_minimo_frequencia int8 NOT null default 0,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT consolidacao_frequencia_turma_pk PRIMARY KEY (id)
);

