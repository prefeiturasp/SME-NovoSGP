CREATE TABLE IF NOT EXISTS public.fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	periodo_escolar_id int8 NOT NULL,
	turma_id int8 NOT NULL,
    disciplina_id varchar(15) NOT NULL,
    situacao int4 NOT NULL,
    excluido boolean not null default false,
    migrado boolean not null default FALSE,
    criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200)
);

select f_cria_constraint_se_nao_existir ('fechamento', 'fechamento_id_pk', 'PRIMARY KEY (id)');

select f_cria_fk_se_nao_existir('fechamento', 'periodo_fechamento_fk', 'FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar (id)');

select f_cria_fk_se_nao_existir('fechamento', 'turma_fechamento_fk', 'FOREIGN KEY (turma_id) REFERENCES turma (id)');


CREATE TABLE IF NOT EXISTS public.pendencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	titulo varchar(100) NOT NULL,
	descricao varchar NOT NULL,
	situacao int4 NOT NULL,
	tipo int4 NOT NULL,
    excluido boolean not null default false,
    migrado boolean not null default FALSE,
    criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200)
);

select f_cria_constraint_se_nao_existir ('pendencia', 'pendencia_pk', 'PRIMARY KEY (id)');



CREATE TABLE IF NOT EXISTS public.pendencia_fechamento (
	pendencia_id int8 NOT NULL,
	fechamento_id int8 NOT NULL
);

select f_cria_constraint_se_nao_existir ('pendencia_fechamento', 'pendencia_fechamento_un', 'UNIQUE (pendencia_id,fechamento_id)');

select f_cria_fk_se_nao_existir('pendencia_fechamento', 'pendencia_fechamento_fk', 'FOREIGN KEY (fechamento_id) REFERENCES fechamento (id)');

select f_cria_fk_se_nao_existir('pendencia_fechamento', 'pendencia_fechamento__pendencia_fk', 'FOREIGN KEY (pendencia_id) REFERENCES pendencia (id)');


CREATE INDEX if not exists fechamento_periodo_escolar_id_idx ON public.fechamento (periodo_escolar_id);
CREATE INDEX if not exists fechamento_turma_id_idx ON public.fechamento (turma_id);
CREATE INDEX if not exists fechamento_disciplina_id_idx ON public.fechamento (disciplina_id);


