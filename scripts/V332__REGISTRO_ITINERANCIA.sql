DROP TABLE if exists public.registro_itinerancia;
CREATE TABLE public.registro_itinerancia (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	data_visita timestamp  NOT NULL,
	data_retorno_verificacao timestamp  NOT NULL,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT registro_itinerancia_pk PRIMARY KEY (id)
);

DROP TABLE if exists public.registro_itinerancia_objetivo_base;
CREATE TABLE public.registro_itinerancia_objetivo_base (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	nome varchar(200)  NOT NULL,
	tem_descricao boolean  NOT NULL,
	permite_varias_ues boolean  NOT NULL,
    excluido boolean not null default false,    
    CONSTRAINT registro_itinerancia_objetivo_base_pk PRIMARY KEY (id)
);

DROP TABLE if exists public.registro_itinerancia_objetivo;
CREATE TABLE public.registro_itinerancia_objetivo (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	registro_itinerancia_base_id int8 NOT NULL,
	registro_itinerancia_id int8 NOT NULL,
	descricao varchar,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT registro_itinerancia_objetivo_pk PRIMARY KEY (id)
);

ALTER TABLE public.registro_itinerancia_objetivo ADD CONSTRAINT registro_itinerancia_objetivo_base_fk FOREIGN KEY (registro_itinerancia_base_id) REFERENCES registro_itinerancia_objetivo_base(id);
ALTER TABLE public.registro_itinerancia_objetivo ADD CONSTRAINT registro_itinerancia_objetivo_itinerancia_fk FOREIGN KEY (registro_itinerancia_id) REFERENCES registro_itinerancia(id);

DROP TABLE if exists public.registro_itinerancia_ue;
CREATE TABLE public.registro_itinerancia_ue (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	ue_id int8 NOT NULL,
	registro_itinerancia_id int8 NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT registro_itinerancia_ue_pk PRIMARY KEY (id)
);

ALTER TABLE public.registro_itinerancia_ue ADD CONSTRAINT registro_itinerancia_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);
ALTER TABLE public.registro_itinerancia_ue ADD CONSTRAINT registro_itinerancia_ue_itinerancia_fk FOREIGN KEY (registro_itinerancia_id) REFERENCES registro_itinerancia(id);

DROP TABLE if exists public.registro_itinerancia_questao;
CREATE TABLE public.registro_itinerancia_questao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	questao_id int8 NOT NULL,
	registro_itinerancia_id int8 NOT NULL,
	resposta varchar NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT registro_itinerancia_questao_pk PRIMARY KEY (id)
);

ALTER TABLE public.registro_itinerancia_questao ADD CONSTRAINT registro_itinerancia_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);
ALTER TABLE public.registro_itinerancia_questao ADD CONSTRAINT registro_itinerancia_questao_itinerancia_fk FOREIGN KEY (registro_itinerancia_id) REFERENCES registro_itinerancia(id);

DROP TABLE if exists public.registro_itinerancia_aluno;
CREATE TABLE public.registro_itinerancia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	codigo_aluno varchar(100) NOT NULL,
	registro_itinerancia_id int8 NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT registro_itinerancia_aluno_pk PRIMARY KEY (id)
);

ALTER TABLE public.registro_itinerancia_aluno ADD CONSTRAINT registro_itinerancia_aluno_itinerancia_fk FOREIGN KEY (registro_itinerancia_id) REFERENCES registro_itinerancia(id);

DROP TABLE if exists public.registro_itinerancia_aluno_questao;
CREATE TABLE public.registro_itinerancia_aluno_questao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	questao_id int8 NOT NULL,
	registro_itinerancia_aluno_id int8 NOT NULL,
	resposta varchar NOT NULL,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT registro_itinerancia_aluno_questao_pk PRIMARY KEY (id)
);

ALTER TABLE public.registro_itinerancia_questao ADD CONSTRAINT registro_itinerancia_aluno_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);
ALTER TABLE public.registro_itinerancia_aluno_questao ADD CONSTRAINT registro_itinerancia_aluno_itinerancia_fk FOREIGN KEY (registro_itinerancia_aluno_id) REFERENCES registro_itinerancia_aluno(id);

DELETE FROM public.registro_itinerancia_objetivo_base WHERE 1=1;
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Mapeamento dos estudantes público da Educação Especial', FALSE, FALSE);
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Reorganização e/ou remanejamento de apoios e serviços', FALSE, FALSE);
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Atendimento de solicitação da U.E', TRUE, FALSE);
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Acompanhamento professor de sala regular', FALSE, TRUE);
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Acompanhamento professor de SRM', FALSE, TRUE);
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Ação Formativa em JEIF', FALSE, TRUE);
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Reunião', FALSE, TRUE);
INSERT INTO public.registro_itinerancia_objetivo_base (nome, tem_descricao, permite_varias_ues) VALUES ('Outros', TRUE, FALSE);
