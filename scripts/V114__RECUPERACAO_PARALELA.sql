CREATE TABLE if not exists eixo (
	id int8 NOT NULL,
	descricao varchar(200) NOT NULL,
	excluido boolean NOT NULL DEFAULT false,
	dt_inicio timestamp NOT NULL,
	dt_fim timestamp NULL,
	recuperacao_paralela_periodo_id int8 NULL,
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
	ordem int8 NOT NULL,
	ehEspecifico boolean NOT NULL DEFAULT false,
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
	objetivo_id int8  NOT NULL,
	resposta_id int8  NOT NULL,		
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
	id int8 NOT NULL,
	nome varchar(100) NOT NULL,
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
	id int8 NOT NULL ,
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
ALTER TABLE public.eixo ADD CONSTRAINT eixo_recuperacao_paralela_periodo_fk FOREIGN KEY (recuperacao_paralela_periodo_id) REFERENCES recuperacao_paralela_periodo(id);

insert into public.recuperacao_paralela_periodo (id,nome,descricao,criado_em,criado_por, criado_rf,excluido)
select
	1,'Encaminhamento','Encaminhamento',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.recuperacao_paralela_periodo
		where
			nome = 'Encaminhamento' );

insert into public.recuperacao_paralela_periodo (id,nome,descricao,criado_em,criado_por, criado_rf,excluido)
select
	2,'Acompanhamento 1º Semestre','Acompanhamento 1º Semestre',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.recuperacao_paralela_periodo
		where
			nome = 'Acompanhamento 1º Semestre' );

insert into public.recuperacao_paralela_periodo (id,nome,descricao,criado_em,criado_por, criado_rf,excluido)
select
	3,'Acompanhamento 2º Semestre','Acompanhamento 2º Semestre',now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.recuperacao_paralela_periodo
		where
			nome = 'Acompanhamento 2º Semestre' );
			
			
			
			
insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,recuperacao_paralela_periodo_id)
select
	1,'Informações escolares',now(),now(),'Carga Inicial','Carga Inicial',false,1
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Informações escolares' );

insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	2,'Frequência',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Frequência' );
insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	3,'Sondagem',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Sondagem' );

insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	4,'Prática e Leitura de Textos',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Prática e Leitura de Textos' );		
insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	5,'Prática de Produção de Textos Escritos',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Prática de Produção de Textos Escritos' );

insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	6,'Prática de Escuta e Produção de Textos Orais',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Prática de Escuta e Produção de Textos Orais' );
insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	7,'Análise Linguística / Multimodal',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Análise Linguística / Multimodal' );

insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	8,'Números',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Números' );	

insert into public.eixo (id,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	9,'Analisa, interpreta e soluciona problemas envolvendo...',now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.eixo
		where
			descricao = 'Analisa, interpreta e soluciona problemas envolvendo...' );						
			
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	1,1,'É atendido pelo AEE?','É atendido pelo AEE?',now(),now(),'Carga Inicial','Carga Inicial',false,false,10
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=1 );						
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	2,1,'É atendido pelo NAAPA?','É atendido pelo NAAPA?',now(),now(),'Carga Inicial','Carga Inicial',false,false,20
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=2 );		
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	3,1,'Parecer conclusivo do ano anterior','Parecer conclusivo do ano anterior',now(),now(),'Carga Inicial','Carga Inicial',false,false,30
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=3 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	4,2,'Frequência na turma de PAP','Frequência na turma de PAP',now(),now(),'Carga Inicial','Carga Inicial',false,true,40
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=4 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	5,3,'Hipótese de escrita','Hipótese de escrita',now(),now(),'Carga Inicial','Carga Inicial',false,false,50
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=5 );			
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	6,4,'Prática e Leitura de Textos','Lê por si mesmo textos diversos utilizando-se de índice linguísticos e contextuais para antecipar, inferir e validar o que está escrito',now(),now(),'Carga Inicial','Carga Inicial',false,false,60
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=6 );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	7,4,'Prática e Leitura de Textos','Realiza antecipações a respeito do conteúdo do texto, utilizando o repertório pessoal de conhecimento sobre o assunto, gênero, autor, portador e veículo de publicação, verificando ao longo da leitura se as antecipações realizadas se confirmaram ou nã',now(),now(),'Carga Inicial','Carga Inicial',false,false,70
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=7 );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	8,4,'Prática e Leitura de Textos','Localiza informações explícitas, considerando a finalidade da leitura',now(),now(),'Carga Inicial','Carga Inicial',false,false,80
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=8 );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	9,4,'Prática e Leitura de Textos','Infere informações a partir do texto (inferência local) ou de conhecimento prévio do assunto (inferência global), a depender da complexidade do texto selecionado',now(),now(),'Carga Inicial','Carga Inicial',false,false,90
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=9 );				
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	10,5,'Prática de Produção de Textos Escritos','Produz o texto planejado, refletindo sobre o encadeamento das ideias',now(),now(),'Carga Inicial','Carga Inicial',false,false,100
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=10 );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	11,5,'Prática de Produção de Textos Escritos','Revisa o texto, considerando as características do contexto de produção e realizando os ajustes necessários para garantir a sua legibilidade e efeitos de sentido pretendidos',now(),now(),'Carga Inicial','Carga Inicial',false,false,110
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=11 );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	12,6,'Prática de Escuta e Produção de Textos Orais','Produz textos orais, considerando a situação comunicativa',now(),now(),'Carga Inicial','Carga Inicial',false,false,120
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=12 );	
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	13,7,'Análise Linguística / Multimodal ','Elimina repetições indesejadas nos textos, substituindo o referente por outra palavra – nome, pronome, apelido, classe relacionada etc., ou utilizando elipse',now(),now(),'Carga Inicial','Carga Inicial',false,false,130
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=13 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	14,7,'Análise Linguística / Multimodal ','Organiza o texto de acordo com as especificidades do gênero',now(),now(),'Carga Inicial','Carga Inicial',false,false,140
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=14 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	15,8,'Números ','Lê, escreve, compara, ordena, compõe e decompõe números naturais pela compreensão e uso das regras do sistema de numeração decimal',now(),now(),'Carga Inicial','Carga Inicial',false,false,150
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=15 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	16,9,'Analisa, interpreta e soluciona problemas envolvendo...','Significados do campo aditivo composição e transformação',now(),now(),'Carga Inicial','Carga Inicial',false,false,160
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=16 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	17,9,'Analisa, interpreta e soluciona problemas envolvendo...','Significados do campo multiplicativo proporcionalidade e multiplicação comparativa',now(),now(),'Carga Inicial','Carga Inicial',false,false,170
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=17 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	18,9,'Analisa, interpreta e soluciona problemas envolvendo...','Grandezas de comprimento, capacidade e massa',now(),now(),'Carga Inicial','Carga Inicial',false,false,180
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=18 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	19,9,'Analisa, interpreta e soluciona problemas envolvendo...','Leitura e representação da localização/ movimentação de pessoas ou objetos no espaço',now(),now(),'Carga Inicial','Carga Inicial',false,false,190
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=19 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	20,9,'Analisa, interpreta e soluciona problemas envolvendo...','Dados apresentados em tabelas e gráficos',now(),now(),'Carga Inicial','Carga Inicial',false,false,200
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=20 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	21,9,'Analisa, interpreta e soluciona problemas envolvendo...','Comparação e a equivalência de valores do sistema monetário brasileiro',now(),now(),'Carga Inicial','Carga Inicial',false,false,210
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=21 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	22,9,'Analisa, interpreta e soluciona problemas envolvendo...','Regularidades em sequências numéricas ou figurais recursivas',now(),now(),'Carga Inicial','Carga Inicial',false,false,220
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=22 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	23,9,'Analisa, interpreta e soluciona problemas envolvendo...','Significados do campo aditivo comparação e composição de transformações',now(),now(),'Carga Inicial','Carga Inicial',false,false,230
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=23 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	24,9,'Analisa, interpreta e soluciona problemas envolvendo...','Significados do campo multiplicativo configuração retangular e combinatória',now(),now(),'Carga Inicial','Carga Inicial',false,false,240
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=24 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	25,9,'Analisa, interpreta e soluciona problemas envolvendo...','Cálculo do perímetro de figuras planas',now(),now(),'Carga Inicial','Carga Inicial',false,false,250
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=25 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	26,9,'Analisa, interpreta e soluciona problemas envolvendo...','Grandezas tempo e temperatura',now(),now(),'Carga Inicial','Carga Inicial',false,false,260
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=26 );
insert into public.objetivo (id,eixo_id,nome,descricao,dt_inicio,criado_em,criado_por, criado_rf,excluido,ehEspecifico,ordem)
select
	27,9,'Analisa, interpreta e soluciona problemas envolvendo...','Cálculo de áreas de figuras planas',now(),now(),'Carga Inicial','Carga Inicial',false,false,270
where
	not exists(
		select
			1
		from
			public.objetivo
		where
			id=27 );


insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	1,'Sim','Sim',true,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 1 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	2,'Não','Não',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 2 );
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	3,'Aprovado','O aluno está aprovado',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 3 );
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	4,'Aprovado pelo conselho','O aluno foi considerado aprovado após alteração de notas no conselho de classe',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 4 );
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	5,'Continuidade dos estudos',' O aluno está aprovado, porque neste ano não há promoção ou retenção por rendimento',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 5 );
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	6,'Retido','O aluno está reprovado por conta do seu rendimento (notas baixas)',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 6 );
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	7,'Retido por frequência','O aluno está reprovado por conta do baixo percentual de frequência',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id =7 );					
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	8,'Frequente: Alunos com frequência acima de 75%','Frequente: Alunos com frequência acima de 75%',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 8 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	9,'Pouco frequente: Alunos com frequência abaixo de 75%','Pouco frequente: Alunos com frequência abaixo de 75%',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 9 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	10,'Não comparece: Alunos com frequência abaixo de 50','Não comparece: Alunos com frequência abaixo de 50',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 10 );
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	11,'Alfabético','Alfabético',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 11 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	7,'Silábico alfabético','Silábico alfabético',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 7 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	12,'Silábico com valor sonoro','Silábico com valor sonoro',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 12 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	13,'Silábico sem valor sonoro','Silábico sem valor sonoro',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 13 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	14,'Pré silábico','Pré silábico',false,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 14 );								
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	15,'Realizou plenamente','Realizou plenamente',null,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id = 15 );			
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	16,'Realizou','Realizou',null,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id=16 );				
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	17,'Não realizou','Não realizou',null,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id=17 );				
insert into public.resposta (id,nome,descricao,sim,dt_inicio,criado_em,criado_por, criado_rf,excluido)
select
	18,'Não avaliado','Não avaliado',null,now(),now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.resposta
		where
			id=18 );	
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	1,1,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 1 
		and resposta_id = 1);	
		
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	1,2,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 1 
		and resposta_id = 2);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	2,1,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 2 
		and resposta_id = 1);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	2,2,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 2 
		and resposta_id = 2);
		
		
		
		
		
		
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	3,3,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 3 
		and resposta_id = 3);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	3,4,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 3 
		and resposta_id = 4);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	3,5,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 3 
		and resposta_id = 5);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	3,6,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 3 
		and resposta_id = 6);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	3,7,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 3 
		and resposta_id = 7);
		





insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	4,8,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 4 
		and resposta_id = 8);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	4,9,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 4 
		and resposta_id = 9);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	4,10,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 4 
		and resposta_id = 10);




insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	5,11,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 5 
		and resposta_id = 11);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	5,12,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 5 
		and resposta_id = 12);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	5,13,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 5 
		and resposta_id = 13);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	5,14,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 5 
		and resposta_id = 14);
		
		


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 18);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	6,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 6 
		and resposta_id = 18);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	7,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 7 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	7,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 7 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	7,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 7 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	7,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 7 
		and resposta_id = 18);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 8 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 18);


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 8 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	8,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 18);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	9,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 9 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	9,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 9 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	9,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 9 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	9,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 9 
		and resposta_id = 18);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	10,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 10 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	10,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 10 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	10,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 10 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	10,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 10);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	11,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 11 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	11,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 11 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	11,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 11 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	11,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 11 
		and resposta_id = 18);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	12,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 12 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	12,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 12 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	12,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 12 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	12,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 8 
		and resposta_id = 12);

	insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	13,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 13 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	13,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 13 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	13,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 13 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	13,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 13 
		and resposta_id = 18);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 14 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 14 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 14 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 14 
		and resposta_id = 18);


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 14 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 14 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 14 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	14,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 14 
		and resposta_id = 18);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	15,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 15 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	15,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 15 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	15,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 15 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	15,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 15 
		and resposta_id = 18);


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	16,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 16 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	16,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 16 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	16,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 16 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	16,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 16 
		and resposta_id = 18);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	17,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 17 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	17,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 17 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	17,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 17 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	17,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 17 
		and resposta_id = 18);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	18,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 18 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	18,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 18 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	18,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 18 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	18,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 18 
		and resposta_id = 18);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	19,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 19 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	19,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 19 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	19,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 19 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	19,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 19 
		and resposta_id = 18);


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	20,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 20 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	20,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 20 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	20,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 20 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	20,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 20 
		and resposta_id = 18);



insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	21,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 21 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	21,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 21 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	21,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 21 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	21,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 21 
		and resposta_id = 18);



insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 22 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 22 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 22 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 22 
		and resposta_id = 18);

insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 21 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 22 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 22 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	22,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 22 
		and resposta_id = 18);


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	23,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 23 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	23,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 23 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	23,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 23 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	23,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 23 
		and resposta_id = 18);


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	24,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 24 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	24,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 24 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	24,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 24 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	24,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 24 
		and resposta_id = 18);


		insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	25,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 25 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	25,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 25 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	25,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 25 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	25,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 25 
		and resposta_id = 18);


insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	26,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 26 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	26,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 26 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	26,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 26 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	26,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 26 
		and resposta_id = 18);



insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	27,15,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id = 27 
		and resposta_id = 15);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	27,16,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 27 
		and resposta_id = 16);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	27,17,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 27 
		and resposta_id = 17);
insert into public.objetivo_resposta (objetivo_id,resposta_id,criado_em,criado_por, criado_rf,excluido)
select 
	27,18,now(),'Carga Inicial','Carga Inicial',false
where
	not exists(
		select
			1
		from
			public.objetivo_resposta
		where
			objetivo_id= 27 
		and resposta_id = 18);

