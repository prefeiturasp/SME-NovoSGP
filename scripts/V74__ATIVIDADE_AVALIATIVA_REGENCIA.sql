CREATE TABLE if not exists public.atividade_avaliativa_regencia (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	atividade_avaliativa_id int8 NOT NULL,
	disciplina_contida_regencia_id varchar(15) not null,
    criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    CONSTRAINT atividade_avaliativa_regencia_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('atividade_avaliativa_regencia', 'atividade_avaliativa_regencia_atividade_avaliativa_fk', 'FOREIGN KEY (atividade_avaliativa_id) REFERENCES atividade_avaliativa (id)');