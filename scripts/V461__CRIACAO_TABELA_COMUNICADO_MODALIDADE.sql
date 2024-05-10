create table if not exists public.comunicado_modalidade
( 
	id int8 not null generated always as identity,
	comunicado_id int not null,
	modalidade int not null,
	excluido boolean not null DEFAULT false,
constraint comunicado_modalidade_pk primary key (id) );

ALTER TABLE public.comunicado_modalidade ADD CONSTRAINT comunicado_modalidade_comunicado_fk FOREIGN KEY (comunicado_id) REFERENCES comunicado(id);