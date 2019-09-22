CREATE TABLE IF NOT EXISTS public.prioridade_perfil (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ordem int8 NOT NULL,
	tipo int4 not null,
	nome_perfil varchar(100) NOT NULL,
	codigo_perfil uuid NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT prioridade_perfil_ordem_un UNIQUE (ordem),
	CONSTRAINT prioridade_perfil_un UNIQUE (codigo_perfil)
);

insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(10,1,'Adm COTIC','5BE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(20,1,'Adm SME/COPED','5AE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(30,1,'COPED','59E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(40,1,'COPED Básico','51E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(50,1,'DIEFEM','58E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(60,1,'DIEJA','56E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(70,1,'DIEI','57E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(80,1,'NTA','54E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(90,1,'NTC','53E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(100,1,'NTC - NAAPA','52E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(110,1,'DIEE','55E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(120,2,'Adm DRE','48E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(130,2,'Diretor Regional','50E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(140,2,'Diretor DIPED','4DE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(150,2,'DIPED','49E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(160,2,'Supervisor Técnico','4FE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(170,2,'Supervisor','4EE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(180,2,'NAAPA','4CE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(190,2,'CEFAI','4BE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(200,2,'PAAI','4AE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(210,2,'DRE Básico','47E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(220,3,'Diretor','46E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(230,3,'AD','45E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(240,3,'CP','44E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(250,3,'Secretário','43E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(260,3,'Adm UE','42E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(270,3,'ATE','3BE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(280,3,'POA','3FE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(290,3,'Professor','40E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(300,3,'PAEE','3DE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(310,3,'PAP','3EE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(320,3,'Professor CJ','41E1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
insert into public.prioridade_perfil (ordem,tipo,nome_perfil,codigo_perfil,criado_em,criado_por,criado_rf) values(330,3,'Professor Readaptado','3CE1E074-37D6-E911-ABD6-F81654FE895D',now(),'Carga','Carga');
