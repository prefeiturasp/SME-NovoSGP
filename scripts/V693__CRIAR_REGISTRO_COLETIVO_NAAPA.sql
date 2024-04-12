-- Tipo de reunião NAAPA

CREATE TABLE public.tipo_reuniao_naapa (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	titulo varchar(50) NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(30) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(30) NULL,
	CONSTRAINT TipoReuniao_pk PRIMARY KEY (id)
);

--Atendimento não presencial
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Atendimento não presencial',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 1);

--Grupo de Trabalho NAAPA
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Grupo de Trabalho NAAPA',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 2);

--Grupo Focal
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Grupo Focal',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 3);

--Itinerância
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Itinerância',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 4);

--Projeto Tecer
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Projeto Tecer',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 5);

--Reunião Compartilhada
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Reunião Compartilhada',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 6);

--Reunião de Rede Macro (formada pelo território)
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Reunião de Rede Macro (formada pelo território)',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 7);

--Reunião de Rede Micro (formada pelo NAAPA)
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Reunião de Rede Micro (formada pelo NAAPA)',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 8);

--Reunião de Rede Micro na UE
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Reunião de Rede Micro na UE',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 9);

--Reunião em Horários Coletivos/ Formação
insert into tipo_reuniao_naapa (titulo, criado_em, criado_por, criado_rf) 
select 'Reunião em Horários Coletivos/ Formação',  NOW(), 'SISTEMA', '0' where not exists(select 1 from tipo_reuniao_naapa where id = 10);

CREATE TABLE public.registrocoletivo (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	dre_id int8 NOT NULL,
	tipo_reuniao_id int8 NOT NULL,
	data_registro timestamp NOT NULL,
	quantidade_participantes int4 NOT NULL,
	quantidade_educadores int4 NOT NULL,
	quantidade_educandos int4 NOT NULL,
	quantidade_cuidadores int4 NOT NULL,
	descricao varchar(250) NOT NULL,
	observacao varchar(250) NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(30) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(30) NULL,
	CONSTRAINT registrocoletivo_pk PRIMARY KEY (id),
	CONSTRAINT registroColetivo_dre_fk FOREIGN KEY (dre_id) REFERENCES public.dre(id),
	CONSTRAINT registroColetivo_tipo_reuniao_fk FOREIGN KEY (tipo_reuniao_id) REFERENCES public.tipo_reuniao_naapa(id)
);

CREATE TABLE public.registrocoletivo_ue (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	ue_id int8 NOT NULL,
	registrocoletivo_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(30) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(30) NULL,
	CONSTRAINT registrocoletivo_ue_pk PRIMARY KEY (id),
	CONSTRAINT registroColetivo_ue_ue_fk FOREIGN KEY (ue_id) REFERENCES public.ue(id),
	CONSTRAINT registroColetivo_ue_registroColetivo_fk FOREIGN KEY (registrocoletivo_id) REFERENCES public.registrocoletivo(id)
);
CREATE INDEX registrocoletivo_ue_rc_idx ON public.registrocoletivo_ue USING btree (registrocoletivo_id);
CREATE INDEX registrocoletivo_ue_ue_idx ON public.registrocoletivo_ue USING btree (ue_id);

CREATE TABLE public.registrocoletivo_anexo (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	registrocoletivo_id int8 NOT NULL,
	arquivo_id int8 NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT registroColetivo_anexo_pk PRIMARY KEY (id),
	CONSTRAINT registroColetivo_anexo_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES public.arquivo(id),
	CONSTRAINT registrocoletivo_id_fk FOREIGN KEY (registrocoletivo_id) REFERENCES public.registrocoletivo(id)
);
CREATE INDEX registrocoletivo_anexo_arquivo_idx ON public.registrocoletivo_anexo USING btree (arquivo_id);
CREATE INDEX registrocoletivo_id_idx ON public.registrocoletivo_anexo USING btree (registrocoletivo_id);