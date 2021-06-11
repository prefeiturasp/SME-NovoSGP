CREATE TABLE if not exists public.aluno_foto (
    id int8 NOT NULL GENERATED ALWAYS AS identity,
    aluno_codigo varchar(15) not null,
    miniatura_id int8 null,
    arquivo_id int8 not null,

    criado_em timestamp NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp,
    alterado_por varchar(200),
    criado_rf varchar(200) NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    CONSTRAINT aluno_foto_pk PRIMARY KEY (id)
);

ALTER TABLE public.aluno_foto ADD CONSTRAINT aluno_foto_miniatura_fk FOREIGN KEY (miniatura_id) REFERENCES aluno_foto(id);
ALTER TABLE public.aluno_foto ADD CONSTRAINT aluno_foto_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);


CREATE INDEX if not exists aluno_foto_miniatura_idx ON public.aluno_foto USING btree (miniatura_id);
CREATE INDEX if not exists aluno_foto_arquivo_idx ON public.aluno_foto USING btree (arquivo_id);
CREATE INDEX if not exists aluno_foto_aluno_codigo_idx ON public.aluno_foto USING btree (aluno_codigo);