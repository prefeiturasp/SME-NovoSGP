CREATE TABLE if NOT EXISTS public.atividade_avaliativa_disciplina (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	atividade_avaliativa_id int8 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    CONSTRAINT atividade_avaliativa_disciplina_pk PRIMARY KEY (id)
);

select f_cria_fk_se_nao_existir('atividade_avaliativa_disciplina', 'atividade_avaliativa_disciplina_fk', 'FOREIGN KEY (atividade_avaliativa_id) REFERENCES atividade_avaliativa (id)');

DELETE from public.atividade_avaliativa_regencia where 1=1;
DELETE from public.notas_conceito where 1=1;
DELETE from public.atividade_avaliativa WHERE 1=1;

ALTER TABLE public.atividade_avaliativa DROP COLUMN disciplina_id;
