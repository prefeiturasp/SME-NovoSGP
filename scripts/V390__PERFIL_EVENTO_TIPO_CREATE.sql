drop table if exists public.perfil_evento_tipo;
alter table evento_tipo drop constraint if exists evento_tipo_pk;

alter table evento_tipo add constraint evento_tipo_pk primary key(id);

CREATE TABLE public.perfil_evento_tipo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_perfil uuid not null,
	evento_tipo_id int8,
	excluido bool not null default false,
	CONSTRAINT perfil_evento_tipo_pk PRIMARY KEY (id)
);
-- foreign keys
ALTER TABLE public.perfil_evento_tipo ADD CONSTRAINT perfil_evento_tipo_tipo_fk FOREIGN KEY (evento_tipo_id) REFERENCES evento_tipo(id);

-- indexes
CREATE INDEX perfil_evento_tipo_perfil_idx ON public.perfil_evento_tipo USING btree (codigo_perfil);
CREATE INDEX perfil_evento_tipo_tipo_idx ON public.perfil_evento_tipo USING btree (evento_tipo_id);

insert into perfil_evento_tipo (codigo_perfil, evento_tipo_id)
values ('4AE1E074-37D6-E911-ABD6-F81654FE895D', (select id from evento_tipo where codigo = 28));

