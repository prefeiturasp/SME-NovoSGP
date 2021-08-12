create table if not exists public.comunicado_ano_escolar
( 
	id int8 not null generated always as identity,
	comunicado_id int not null,
	ano_escolar varchar(1) not null,
	excluido boolean not null DEFAULT false,
constraint comunicado_ano_escolar_pk primary key (id) );

ALTER TABLE public.comunicado_ano_escolar ADD CONSTRAINT comunicado_ano_escolar_comunicado_fk FOREIGN KEY (comunicado_id) REFERENCES comunicado(id);