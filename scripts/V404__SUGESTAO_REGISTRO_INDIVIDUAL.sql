CREATE TABLE IF NOT EXISTS public.registro_individual_sugestao
(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	mes int4 NOT NULL,
	descricao varchar(150) NOT NULL,
	excluido boolean not null default false,

	CONSTRAINT registro_individual_sugestao_pk PRIMARY KEY(id)
);

insert into public.registro_individual_sugestao(mes, descricao) values (2, 'Momento de adaptação e acolhimento. Como foi ou está sendo este processo para a criança e a família?');
insert into public.registro_individual_sugestao(mes, descricao) values (3, 'Como a criança brinca e interage no parque, área externa e outros espaços da unidade?');
insert into public.registro_individual_sugestao(mes, descricao) values (4, 'Como as crianças se relacionam consigo mesmas e com o grupo?');
insert into public.registro_individual_sugestao(mes, descricao) values (5, 'Como a criança responde às intervenções do professor(a)?');
insert into public.registro_individual_sugestao(mes, descricao) values (6, 'Quais os maiores interesses da criança? Como está a relação da família com a escola?');
insert into public.registro_individual_sugestao(mes, descricao) values (7, 'Evidências de oferta e evidências de aprendizagem.');
insert into public.registro_individual_sugestao(mes, descricao) values (8, 'Evidências de oferta e evidências de aprendizagem.');
insert into public.registro_individual_sugestao(mes, descricao) values (9, 'Evidências de oferta e evidências de aprendizagem.');
insert into public.registro_individual_sugestao(mes, descricao) values (10, 'Evidências de oferta e evidências de aprendizagem.');
insert into public.registro_individual_sugestao(mes, descricao) values (11, 'Evidências de oferta e evidências de aprendizagem.');
insert into public.registro_individual_sugestao(mes, descricao) values (12, 'Evidências de oferta e evidências de aprendizagem.');