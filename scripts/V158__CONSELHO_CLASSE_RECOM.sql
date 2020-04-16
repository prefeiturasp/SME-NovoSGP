DROP TABLE IF EXISTS public.conselho_classe_recomendacao;

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

create index if not exists ix_conselho_classe_recomendacao_tipo on conselho_classe_recomendacao(tipo);

--FAMILIA
INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Acompanhe a frequência de seus filhos às aulas e às atividades escolares.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Acompanhe a frequência de seus filhos às aulas e às atividades escolares.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Acompanhe seu filho na realização das lições de casa.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Acompanhe seu filho na realização das lições de casa.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Ajude a construir uma escola democrática. Participe do Conselho de Escola.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Ajude a construir uma escola democrática. Participe do Conselho de Escola.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Compareça às reuniões da escola. Dê sua opinião. Ela é muito importante.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Compareça às reuniões da escola. Dê sua opinião. Ela é muito importante.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Confira o boletim escolar de seu filho,caso tenha alguma dúvida procure o professor/coordenador.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Confira o boletim escolar de seu filho, caso tenha alguma dúvida procure o professor/coordenador.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Incentive seu filho a cumprir prazos,tarefas e regras da Unidade Escolar.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Incentive seu filho a cumprir prazos, tarefas e regras da Unidade Escolar.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Leia bilhetes e avisos que a escola mandar e responda quando necessário.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Leia bilhetes e avisos que a escola mandar e responda quando necessário.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Oriente seu filho a cuidar de seu material escolar.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Oriente seu filho a cuidar de seu material escolar.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Peça orientação aos professores e coordenadores caso perceba alguma dificuldade no desempenho do seu filho.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Peça orientação aos professores e coordenadores caso perceba alguma dificuldade no desempenho do seu filho.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Procure visitar a escola de seus filhos sempre que precisar.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Procure visitar a escola de seus filhos sempre que precisar.'
			AND tipo = 1
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Verifique diariamente os cadernos e livros de seus filhos.',
	1,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Verifique diariamente os cadernos e livros de seus filhos.'
			AND tipo = 1
	);

-- ALUNO 
INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Busque ir além dos conhecimentos trabalhados em sala de aula. Seja curioso.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Busque ir além dos conhecimentos trabalhados em sala de aula. Seja curioso.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Cuide de seu material escolar. Ele é de sua responsabilidade.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Cuide de seu material escolar. Ele é de sua responsabilidade.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Cuide de suas relações pessoais. Busque ajuda e orientação de professores,funcionários e gestores sempre que necessário.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Cuide de suas relações pessoais. Busque ajuda e orientação de professores, funcionários e gestores sempre que necessário.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Desenvolva uma rotina de estudo e organização para o cumprimento das tarefas e prazos escolares.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Desenvolva uma rotina de estudo e organização para o cumprimento das tarefas e prazos escolares.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Esclareça suas dúvidas com os professores sempre que necessário.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Esclareça suas dúvidas com os professores sempre que necessário.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Frequente às aulas diariamente. Em caso de ausência,justifique-a.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Frequente às aulas diariamente. Em caso de ausência, justifique-a.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Frequente bibliotecas e sites confiáveis para pesquisa.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Frequente bibliotecas e sites confiáveis para pesquisa.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Leia,releia,converse com seus colegas e outros adultos sobre temas estudados,buscando ampliar seu entendimento sobre eles.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Leia, releia, converse com seus colegas e outros adultos sobre temas estudados, buscando ampliar seu entendimento sobre eles.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Participe das aulas com atenção,pergunte quando tiver dúvidas e faça registro das ideias centrais da aula.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Participe das aulas com atenção, pergunte quando tiver dúvidas e faça registro das ideias centrais da aula.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Peça permissão para falar e saiba ouvir seus colegas.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Peça permissão para falar e saiba ouvir seus colegas.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Pesquise. Reflita. Questione. Discuta. Escreva.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Pesquise. Reflita. Questione. Discuta. Escreva.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Procure desenvolver seus próprios métodos de estudo. Use agenda e anote seus compromissos.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Procure desenvolver seus próprios métodos de estudo. Use agenda e anote seus compromissos.'
			AND tipo = 2
	);

INSERT INTO
	conselho_classe_recomendacao (
		recomendacao,
		tipo,
		criado_em,
		criado_por,
		criado_rf
	)
SELECT
	'Valorize,respeite e coopere com o trabalho de todos no ambiente escolar.',
	2,
	now(),
	'Carga Inicial',
	''
WHERE
	not exists(
		SELECT
			1
		FROM
			public.conselho_classe_recomendacao
		WHERE
			recomendacao = 'Valorize, respeite e coopere com o trabalho de todos no ambiente escolar.'
			AND tipo = 2
	);