CREATE TABLE if not exists eixo (
	id int8 NOT NULL,
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
	id int8 NOT NULL,
	eixo_id int8 NOT NULL,
	nome varchar(200) NOT NULL,
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
			
			
			
			
insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	1,'Informações escolares',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Informações escolares' );

insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	2,'Frequência',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Frequência' );
insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	3,'Sondagem',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Sondagem' );

insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	4,'Prática e Leitura de Textos',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Prática e Leitura de Textos' );		
insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	5,'Prática de Produção de Textos Escritos',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Prática de Produção de Textos Escritos' );

insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	6,'Prática de Escuta e Produção de Textos Orais',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = '"Prática de Escuta e Produção de Textos Orais' );
insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	7,'Análise Linguística / Multimodal',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Análise Linguística / Multimodal' );

insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	8,'Números',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Números' );	

insert into public.eixo (id,descricao,criado_em,criado_por, criado_rf,excluido)
select
	9,'Analisa, interpreta e soluciona problemas envolvendo...',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Analisa, interpreta e soluciona problemas envolvendo...' );						




insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	1,1,'É atendido pelo AEE?','É atendido pelo AEE?',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'É atendido pelo AEE?' );						
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	2,1,'É atendido pelo NAAPA?','É atendido pelo NAAPA?',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'É atendido pelo NAAPA?' );		
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	2,1,'É atendido pelo NAAPA?','É atendido pelo NAAPA?',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'É atendido pelo NAAPA?' );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	3,1,'Parecer conclusivo do ano anterior','Parecer conclusivo do ano anterior',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'Parecer conclusivo do ano anterior' );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	4,2,'Frequência na turma de PAP','Frequência na turma de PAP',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'Frequência na turma de PAP' );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	5,3,'Hipótese de escrita','Hipótese de escrita',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'Hipótese de escrita' );			
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	6,4,'OBJETIVO 1','Lê por si mesmo textos diversos utilizando-se de índice linguísticos e contextuais para antecipar, inferir e validar o que está escrito',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'OBJETIVO 1' );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	7,4,'OBJETIVO 2','Realiza antecipações a respeito do conteúdo do texto, utilizando o repertório pessoal de conhecimento sobre o assunto, gênero, autor, portador e veículo de publicação, verificando ao longo da leitura se as antecipações realizadas se confirmaram ou nã',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'OBJETIVO 2' );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	8,4,'OBJETIVO 3','Localiza informações explícitas, considerando a finalidade da leitura',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			nome = 'OBJETIVO 3' );	
			


insert into public.resposta (nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	'Sim','Sim',true,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			nome = 'sim' );			
insert into public.resposta (nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	'Não','Não',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			nome = 'Não' );				
		
			
			