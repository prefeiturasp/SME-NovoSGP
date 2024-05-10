--ITINERANCIA
DROP TABLE if exists public.itinerancia;
CREATE TABLE public.itinerancia (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	data_visita timestamp  NOT NULL,
	data_retorno_verificacao timestamp  NOT NULL,
    situacao int4 not null default 1,	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT itinerancia_pk PRIMARY KEY (id)
);

--ITINERANCIA OBJETIVO BASE
DROP TABLE if exists public.itinerancia_objetivo_base;
CREATE TABLE public.itinerancia_objetivo_base (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	nome varchar(200)  NOT NULL,
	tem_descricao boolean  NOT NULL,
	permite_varias_ues boolean  NOT NULL,
	ordem int4 NOT NULL,
    excluido boolean not null default false,    
    CONSTRAINT itinerancia_objetivo_base_pk PRIMARY KEY (id)
);

--ITINERANCIA OBJETIVO
DROP TABLE if exists public.itinerancia_objetivo;
CREATE TABLE public.itinerancia_objetivo (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	itinerancia_base_id int8 NOT NULL,
	itinerancia_id int8 NOT NULL,
	descricao varchar,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT itinerancia_objetivo_pk PRIMARY KEY (id)
);

ALTER TABLE public.itinerancia_objetivo ADD CONSTRAINT itinerancia_objetivo_base_fk FOREIGN KEY (itinerancia_base_id) REFERENCES itinerancia_objetivo_base(id);
ALTER TABLE public.itinerancia_objetivo ADD CONSTRAINT itinerancia_objetivo_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id);

--ITINERANCIA UE
DROP TABLE if exists public.itinerancia_ue;
CREATE TABLE public.itinerancia_ue (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	ue_id int8 NOT NULL,
	itinerancia_id int8 NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT itinerancia_ue_pk PRIMARY KEY (id)
);

ALTER TABLE public.itinerancia_ue ADD CONSTRAINT itinerancia_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);
ALTER TABLE public.itinerancia_ue ADD CONSTRAINT itinerancia_ue_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id);

--ITINERANCIA QUESTAO
DROP TABLE if exists public.itinerancia_questao;
CREATE TABLE public.itinerancia_questao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	questao_id int8 NOT NULL,
	itinerancia_id int8 NOT NULL,
	resposta varchar NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT itinerancia_questao_pk PRIMARY KEY (id)
);

ALTER TABLE public.itinerancia_questao ADD CONSTRAINT itinerancia_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);
ALTER TABLE public.itinerancia_questao ADD CONSTRAINT itinerancia_questao_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id);

--ALUNO
DROP TABLE if exists public.itinerancia_aluno;
CREATE TABLE public.itinerancia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	codigo_aluno varchar(100) NOT NULL,
	itinerancia_id int8 NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT itinerancia_aluno_pk PRIMARY KEY (id)
);

ALTER TABLE public.itinerancia_aluno ADD CONSTRAINT itinerancia_aluno_itinerancia_fk FOREIGN KEY (itinerancia_id) REFERENCES itinerancia(id);

--ALUNO QUESTAO
DROP TABLE if exists public.itinerancia_aluno_questao;
CREATE TABLE public.itinerancia_aluno_questao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	questao_id int8 NOT NULL,
	itinerancia_aluno_id int8 NOT NULL,
	resposta varchar NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT itinerancia_aluno_questao_pk PRIMARY KEY (id)
);

ALTER TABLE public.itinerancia_questao ADD CONSTRAINT itinerancia_aluno_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);
ALTER TABLE public.itinerancia_aluno_questao ADD CONSTRAINT itinerancia_aluno_itinerancia_fk FOREIGN KEY (itinerancia_aluno_id) REFERENCES itinerancia_aluno(id);

--INSERT OBJETIVOS BASE
DELETE FROM public.itinerancia_objetivo_base WHERE 1=1;
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Mapeamento dos estudantes público da Educação Especial', FALSE, FALSE, 0);
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Reorganização e/ou remanejamento de apoios e serviços', FALSE, FALSE, 10);
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Atendimento de solicitação da U.E', TRUE, FALSE, 20);
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Acompanhamento professor de sala regular', FALSE, TRUE, 30);
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Acompanhamento professor de SRM', FALSE, TRUE, 40);
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Ação Formativa em JEIF', FALSE, TRUE, 50);
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Reunião', FALSE, TRUE, 60);
INSERT INTO public.itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues, ordem) VALUES ('Outros', TRUE, FALSE, 70);

--INSERT QUESTIONARIO
insert into public.questionario (nome, tipo, criado_em, criado_por, criado_rf) values ('Questionário Registro Itinerância', 3, now(),'Carga Inicial','Carga Inicial');
insert into public.questionario (nome, tipo, criado_em, criado_por, criado_rf) values ('Questionário Registro Itinerância do Aluno', 4, now(),'Carga Inicial','Carga Inicial');

--INSET QUESTAO ITINERANCIA
insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf) values(
	(select id from public.questionario where tipo = 3), 0, 'Acompanhamento da situação', false, 2, now(),'Carga Inicial','Carga Inicial'); 
insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf) values(
	(select id from public.questionario where tipo = 3), 1, 'Encaminhamentos', false, 2, now(),'Carga Inicial','Carga Inicial'); 

--INSERT QUESTAO ITINERANCIA ALUNO
insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf) values(
	(select id from public.questionario where tipo = 4), 0, 'Descritivo do estudante', false, 2, now(),'Carga Inicial','Carga Inicial'); 
insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf) values(
	(select id from public.questionario where tipo = 4), 1, 'Acompanhamento da situação', false, 2, now(),'Carga Inicial','Carga Inicial'); 
insert into public.questao(questionario_id, ordem, nome, obrigatorio, tipo, criado_em, criado_por, criado_rf) values(
	(select id from public.questionario where tipo = 4), 2, 'Encaminhamentos', false, 2, now(),'Carga Inicial','Carga Inicial'); 