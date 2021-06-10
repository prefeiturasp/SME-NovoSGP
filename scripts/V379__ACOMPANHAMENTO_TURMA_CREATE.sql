CREATE TABLE if not exists public.acompanhamento_turma (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
    turma_id int8 NOT NULL,
    semestre int not null,
	apanhado_geral varchar,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT acompanhamento_turma_pk PRIMARY KEY (id)
);
ALTER TABLE public.acompanhamento_turma ADD CONSTRAINT acompanhamento_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
CREATE INDEX if not exists acompanhamento_turma_idx ON public.acompanhamento_turma USING btree (turma_id);
