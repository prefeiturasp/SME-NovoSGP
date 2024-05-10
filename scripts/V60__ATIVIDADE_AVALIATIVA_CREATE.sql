CREATE TABLE if NOT EXISTS public.tipo_avaliacao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	nome varchar(30) NOT NULL,
	descricao varchar(200) NOT NULL,  
    situacao boolean not null default true,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    CONSTRAINT tipo_avaliacao_pk PRIMARY KEY (id)
);

CREATE TABLE if not exists public.atividade_avaliativa (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	dre_id varchar(15) NOT NULL,
	ue_id varchar(15) NOT NULL,
    professor_rf varchar(10) NOT NULL,
    turma_id varchar(15) NOT NULL,
    categoria_id int4 NOT NULL,
    componente_curricular_id int8 NOT NULL,
    tipo_avaliacao_id int4 NOT NULL,
    nome_avaliacao varchar(100) NOT NULL,
    descricao_avaliacao varchar(500) NOT NULL,
    data_avaliacao timestamp NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    CONSTRAINT atividade_avaliativa_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('atividade_avaliativa', 'atividade_avaliativa_tipo_atividade_avaliativa_fk', 'FOREIGN KEY (tipo_avaliacao_id) REFERENCES tipo_avaliacao (id)');
select f_cria_fk_se_nao_existir('atividade_avaliativa', 'atividade_avaliativa_componente_curricular_fk', 'FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular (id)');