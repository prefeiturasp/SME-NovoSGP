DROP TABLE if exists public.notificacao_aula_prevista;
CREATE TABLE public.notificacao_aula_prevista (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	bimestre int4 NOT NULL,
	notificacao_id int8 NOT NULL,
    disciplina_id varchar(15) NOT NULL,
    turma_id varchar(15) NOT NULL,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT notificacao_aula_prevista_pk PRIMARY KEY (id)
);
CREATE INDEX notificacao_aula_prevista_disciplina_idx ON public.notificacao_aula_prevista USING btree (disciplina_id);
CREATE INDEX notificacao_aula_prevista_notificacao_idx ON public.notificacao_aula_prevista USING btree (notificacao_id);