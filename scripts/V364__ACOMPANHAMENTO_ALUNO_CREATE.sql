DROP TABLE if exists public.acompanhamento_aluno_foto;
DROP TABLE if exists public.acompanhamento_aluno_semestre;
DROP TABLE if exists public.acompanhamento_aluno;


CREATE TABLE public.acompanhamento_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
    turma_id int8 NOT NULL,
    aluno_codigo varchar(15) not null,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT acompanhamento_aluno_pk PRIMARY KEY (id)
);
ALTER TABLE public.acompanhamento_aluno ADD CONSTRAINT acompanhamento_aluno_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX acompanhamento_aluno_turma_idx ON public.acompanhamento_aluno USING btree (turma_id);
CREATE INDEX acompanhamento_aluno_aluno_idx ON public.acompanhamento_aluno USING btree (aluno_codigo);


CREATE TABLE public.acompanhamento_aluno_semestre (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
    acompanhamento_aluno_id int8 NOT NULL,
    semestre int4 not null,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT acompanhamento_aluno_semestre_pk PRIMARY KEY (id)
);
ALTER TABLE public.acompanhamento_aluno_semestre ADD CONSTRAINT acompanhamento_aluno_semestre_acompanhamento_fk FOREIGN KEY (acompanhamento_aluno_id) REFERENCES acompanhamento_aluno(id);
CREATE INDEX acompanhamento_aluno_semestre_acompanhamento_idx ON public.acompanhamento_aluno_semestre USING btree (acompanhamento_aluno_id);


CREATE TABLE public.acompanhamento_aluno_foto (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
    acompanhamento_aluno_semestre_id int8 NOT NULL,
    arquivo_id int8 not null,
    miniatura_id int8 null,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT acompanhamento_aluno_foto_pk PRIMARY KEY (id)
);
ALTER TABLE public.acompanhamento_aluno_foto ADD CONSTRAINT acompanhamento_aluno_foto_semestre_fk FOREIGN KEY (acompanhamento_aluno_semestre_id) REFERENCES acompanhamento_aluno_semestre(id);
ALTER TABLE public.acompanhamento_aluno_foto ADD CONSTRAINT acompanhamento_aluno_foto_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
ALTER TABLE public.acompanhamento_aluno_foto ADD CONSTRAINT acompanhamento_aluno_foto_miniatura_fk FOREIGN KEY (miniatura_id) REFERENCES acompanhamento_aluno_foto(id);

CREATE INDEX acompanhamento_aluno_foto_semestre_idx ON public.acompanhamento_aluno_foto USING btree (acompanhamento_aluno_semestre_id);
CREATE INDEX acompanhamento_aluno_foto_arquivo_idx ON public.acompanhamento_aluno_foto USING btree (arquivo_id);
CREATE INDEX acompanhamento_aluno_foto_miniatura_idx ON public.acompanhamento_aluno_foto USING btree (miniatura_id);
