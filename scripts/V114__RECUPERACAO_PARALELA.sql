CREATE TABLE if not exists eixo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(200) NOT NULL,
	excluido boolean NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT eixo_pk PRIMARY KEY (id)
);



CREATE TABLE if not exists recuperacao_paralela (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aluno_id int8 not null,
	turma_id int8 NOT NULL,			
	excluido boolean NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT recuperacao_paralela_pk PRIMARY KEY (id)
);



CREATE TABLE if not exists objetivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	eixo_id int8 NOT NULL,
	nome varchar(20) NOT NULL,
	descricao varchar(600) NOT NULL,
	excluido boolean NOT NULL DEFAULT false,
	dt_inicio timestamp NOT NULL,
	dt_fim timestamp NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_pk PRIMARY KEY (id)
);



CREATE TABLE if not exists objetivo_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	objetivo_id int  NOT NULL,
	resposta_id int  NOT NULL,
	descricao varchar(600) NOT NULL,
	excluido boolean NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT objetivo_resposta_pk PRIMARY KEY (id)
);



CREATE TABLE if not exists resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(20) NOT NULL,
	descricao varchar(600) NOT NULL,
	sim boolean NULL,
	excluido boolean NOT NULL DEFAULT false,
	dt_inicio timestamp NOT NULL,
	dt_fim timestamp NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT resposta_pk PRIMARY KEY (id)
);





CREATE TABLE if not exists recuperacao_paralela_periodo_objetivo_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	recuperacao_paralela_id int8 not null,
	objetivo_id int8 NOT NULL,
	resposta_id int8 NOT NULL,
	periodo_recuperacao_paralela_id int8 NOT NULL,
	excluido boolean NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_pk PRIMARY KEY (id)
);

CREATE TABLE if not exists recuperacao_paralela_periodo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(100) not null,
	descricao varchar(200) NOT NULL,
	excluido boolean NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT recuperacao_paralela_periodo_pk PRIMARY KEY (id)
);



ALTER TABLE public.recuperacao_paralela_periodo_objetivo_resposta ADD CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_objetivo_fk FOREIGN KEY (objetivo_id) REFERENCES objetivo(id);
ALTER TABLE public.recuperacao_paralela_periodo_objetivo_resposta ADD CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES resposta(id);
ALTER TABLE public.recuperacao_paralela_periodo_objetivo_resposta ADD CONSTRAINT recuperacao_paralela_periodo_objetivo_resposta_periodo_fk FOREIGN KEY (periodo_recuperacao_paralela_id) REFERENCES recuperacao_paralela_periodo(id);
ALTER TABLE public.objetivo_resposta ADD CONSTRAINT objetivo_resposta_objetivo_fk FOREIGN KEY (objetivo_id) REFERENCES objetivo(id);
ALTER TABLE public.objetivo_resposta ADD CONSTRAINT objetivo_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES resposta(id);
ALTER TABLE public.objetivo ADD CONSTRAINT objetivo_eixo_fk FOREIGN KEY (eixo_id) REFERENCES eixo(id);

insert into public.recuperacao_paralela_periodo (nome,descricao,criado_em,criado_por, criado_rf,excluido)
select
	'Encaminhamento','Encaminhamento',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.recuperacao_paralela_periodo
		where
			nome = 'Encaminhamento' );

insert into public.recuperacao_paralela_periodo (nome,descricao,criado_em,criado_por, criado_rf,excluido)
select
	'Acompanhamento 1º Semestre','Acompanhamento 1º Semestre',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.recuperacao_paralela_periodo
		where
			nome = 'Acompanhamento 1º Semestre' );

insert into public.recuperacao_paralela_periodo (nome,descricao,criado_em,criado_por, criado_rf,excluido)
select
	'Acompanhamento 2º Semestre','Acompanhamento 2º Semestre',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.recuperacao_paralela_periodo
		where
			nome = 'Acompanhamento 2º Semestre' );

