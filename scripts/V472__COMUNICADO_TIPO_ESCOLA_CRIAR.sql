create table if not exists public.comunicado_tipo_escola
( 
	id int8 not null generated always as identity,
	comunicado_id int not null,
	tipo_escola int not null,
	excluido boolean not null DEFAULT false,
constraint comunicado_tipo_escola_pk primary key (id) );

ALTER TABLE public.comunicado_tipo_escola ADD CONSTRAINT comunicado_tipo_escola_comunicado_fk FOREIGN KEY (comunicado_id) REFERENCES comunicado(id);