DROP TABLE if exists public.conselho_classe_recomendacao;

CREATE TABLE public.conselho_classe_recomendacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	recomendacao varchar NOT NULL,
	tipo int NOT NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT conselho_classe_recomendacao_pk PRIMARY KEY (id)
);

--FAMILIA

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Acompanhe a frequência de seus filhos às aulas e às atividades escolares.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Acompanhe a frequência de seus filhos às aulas e às atividades escolares.' and tipo = 1  );


INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Acompanhe seu filho na realização das lições de casa.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Acompanhe seu filho na realização das lições de casa.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Ajude a construir uma escola democrática. Participe do Conselho de Escola.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Ajude a construir uma escola democrática. Participe do Conselho de Escola.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Compareça às reuniões da escola. Dê sua opinião. Ela é muito importante.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Compareça às reuniões da escola. Dê sua opinião. Ela é muito importante.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Confira o boletim escolar de seu filho, caso tenha alguma dúvida procure o professor/coordenador.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Confira o boletim escolar de seu filho, caso tenha alguma dúvida procure o professor/coordenador.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Incentive seu filho a cumprir prazos, tarefas e regras da Unidade Escolar.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Incentive seu filho a cumprir prazos, tarefas e regras da Unidade Escolar.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Leia bilhetes e avisos que a escola mandar e responda quando necessário.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Leia bilhetes e avisos que a escola mandar e responda quando necessário.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Oriente seu filho a cuidar de seu material escolar.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Oriente seu filho a cuidar de seu material escolar.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Peça orientação aos professores e coordenadores caso perceba alguma dificuldade no desempenho do seu filho.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Peça orientação aos professores e coordenadores caso perceba alguma dificuldade no desempenho do seu filho.' and tipo = 1  );


INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Procure visitar a escola de seus filhos sempre que precisar.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Procure visitar a escola de seus filhos sempre que precisar.' and tipo = 1  );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Verifique diariamente os cadernos e livros de seus filhos.', 1, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Verifique diariamente os cadernos e livros de seus filhos.' and tipo = 1 );

-- ALUNO

	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Busque ir além dos conhecimentos trabalhados em sala de aula. Seja curioso.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Busque ir além dos conhecimentos trabalhados em sala de aula. Seja curioso.' and tipo = 2 );
	
	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Cuide de seu material escolar. Ele é de sua responsabilidade.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Cuide de seu material escolar. Ele é de sua responsabilidade.' and tipo = 2 );
	
	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Cuide de suas relações pessoais. Busque ajuda e orientação de professores, funcionários e gestores sempre que necessário.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Cuide de suas relações pessoais. Busque ajuda e orientação de professores, funcionários e gestores sempre que necessário.' and tipo = 2 );
	
INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Desenvolva uma rotina de estudo e organização para o cumprimento das tarefas e prazos escolares.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Desenvolva uma rotina de estudo e organização para o cumprimento das tarefas e prazos escolares.' and tipo = 2 );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Esclareça suas dúvidas com os professores sempre que necessário.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Esclareça suas dúvidas com os professores sempre que necessário.' and tipo = 2 );
	
	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Frequente às aulas diariamente. Em caso de ausência, justifique-a.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Frequente às aulas diariamente. Em caso de ausência, justifique-a.' and tipo = 2 );

INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Frequente bibliotecas e sites confiáveis para pesquisa.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Frequente bibliotecas e sites confiáveis para pesquisa.' and tipo = 2 );

	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Leia, releia, converse com seus colegas e outros adultos sobre temas estudados, buscando ampliar seu entendimento sobre eles.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Leia, releia, converse com seus colegas e outros adultos sobre temas estudados, buscando ampliar seu entendimento sobre eles.' and tipo = 2 );

	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Participe das aulas com atenção, pergunte quando tiver dúvidas e faça registro das ideias centrais da aula.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Participe das aulas com atenção, pergunte quando tiver dúvidas e faça registro das ideias centrais da aula.' and tipo = 2 );

	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Peça permissão para falar e saiba ouvir seus colegas.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Peça permissão para falar e saiba ouvir seus colegas.' and tipo = 2 );

		INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Pesquise. Reflita. Questione. Discuta. Escreva.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Pesquise. Reflita. Questione. Discuta. Escreva.' and tipo = 2 );


	INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Procure desenvolver seus próprios métodos de estudo. Use agenda e anote seus compromissos.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Procure desenvolver seus próprios métodos de estudo. Use agenda e anote seus compromissos.' and tipo = 2 );

		INSERT INTO conselho_classe_recomendacao 
(recomendacao, tipo, criado_em, criado_por, criado_rf)
select
'Valorize, respeite e coopere com o trabalho de todos no ambiente escolar.', 2, now(), 'Carga Inicial', ''
where
	not exists(
	select
		1
	from
		public.conselho_classe_recomendacao
	where
		recomendacao = 'Valorize, respeite e coopere com o trabalho de todos no ambiente escolar.' and tipo = 2 );
 