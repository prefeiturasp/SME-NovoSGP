DROP TABLE if exists public.notificacao_frequencia;
CREATE TABLE public.notificacao_frequencia (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	tipo int4 NOT NULL,
	notificacao_codigo int8 NOT NULL,
    disciplina_codigo varchar(15) NOT NULL,
    aula_id int8 NOT NULL,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT notificacao_frequencia_pk PRIMARY KEY (id)
);
CREATE INDEX notificacao_frequencia_disciplina_idx ON public.notificacao_frequencia USING btree (disciplina_codigo);
CREATE INDEX notificacao_frequencia_notificacao_idx ON public.notificacao_frequencia USING btree (notificacao_codigo);

ALTER TABLE public.notificacao_frequencia ADD CONSTRAINT notificacao_frequencia_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id);

