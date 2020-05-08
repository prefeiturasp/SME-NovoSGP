CREATE table if not exists public.conselho_classe_parecer (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) NOT NULL,
	aprovado bool NOT NULL DEFAULT false,
	frequencia bool NOT NULL DEFAULT false,
	conselho bool NOT NULL DEFAULT false,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(20) not null,
	criado_em timestamp not null,
	alterado_por varchar(200),
	alterado_rf varchar(20),
	alterado_em timestamp,
	PRIMARY KEY (id)
);

CREATE table if not exists public.conselho_classe_parecer_ano (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	parecer_id int8 NOT NULL,
	ano_turma int4 NOT NULL,
	modalidade int4 NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(20) not null,
	criado_em timestamp not null,
	alterado_por varchar(200),
	alterado_rf varchar(20),
	alterado_em timestamp,
	PRIMARY KEY (id),
	FOREIGN KEY (parecer_id) REFERENCES conselho_classe_parecer (id)
);  

CREATE INDEX conselho_classe_parecer_ano_ano_turma_idx ON public.conselho_classe_parecer_ano (ano_turma,modalidade);

INSERT INTO public.conselho_classe_parecer (nome, aprovado, frequencia, conselho, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 'Promovido',
          TRUE,
          TRUE,
          FALSE,
          '2014-01-01',
		  'SISTEMA',
		  '2014-01-01',
		  '0'
   WHERE NOT EXISTS
       (SELECT id
        FROM conselho_classe_parecer ccp
        WHERE ccp.nome = 'Promovido'));


INSERT INTO public.conselho_classe_parecer (nome, aprovado, frequencia, conselho, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 'Promovido pelo conselho',
          TRUE,
          FALSE,
          TRUE,
          '2014-01-01',
		  'SISTEMA',
		  '2014-01-01',
		  '0'
   WHERE NOT EXISTS
       (SELECT id
        FROM conselho_classe_parecer ccp
        WHERE ccp.nome = 'Promovido pelo conselho'));


INSERT INTO public.conselho_classe_parecer (nome, aprovado, frequencia, conselho, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 'Continuidade dos estudos',
          TRUE,
          TRUE,
          FALSE,
          '2014-01-01',
		  'SISTEMA',
		  '2014-01-01',
		  '0'
   WHERE NOT EXISTS
       (SELECT id
        FROM conselho_classe_parecer ccp
        WHERE ccp.nome = 'Continuidade dos estudos'));


INSERT INTO public.conselho_classe_parecer (nome, aprovado, frequencia, conselho, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 'Retido',
          FALSE,
          FALSE,
          FALSE,
          '2014-01-01',
		  'SISTEMA',
		  '2014-01-01',
		  '0'
   WHERE NOT EXISTS
       (SELECT id
        FROM conselho_classe_parecer ccp
        WHERE ccp.nome = 'Retido'));


INSERT INTO public.conselho_classe_parecer (nome, aprovado, frequencia, conselho, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 'Retido por frequência',
          FALSE,
          TRUE,
          FALSE,
          '2014-01-01',
		  'SISTEMA',
		  '2014-01-01',
		  '0'
   WHERE NOT EXISTS
       (SELECT id
        FROM conselho_classe_parecer ccp
        WHERE ccp.nome = 'Retido por frequência'));

INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          1,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 3
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          1,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          2,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          4,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 4));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          5,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 5));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          1,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          1,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          2,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          4,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 4));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          5,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 5));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          2,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 3
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          3,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 3
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          4,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 3
          AND ano_turma = 4));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          3,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 5
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          6,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 5
          AND ano_turma = 6));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          7,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 5
          AND ano_turma = 7));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          8,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 5
          AND ano_turma = 8));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          9,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 5
          AND ano_turma = 9));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          1,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 6
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          2,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 6
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          2,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 3
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          3,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 3
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          4,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 3
          AND ano_turma = 4));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          3,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 5
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          6,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 5
          AND ano_turma = 6));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          7,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 5
          AND ano_turma = 7));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          8,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 5
          AND ano_turma = 8));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          9,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 5
          AND ano_turma = 9));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          1,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 6
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          2,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 6
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          3,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 6
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          2,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 3
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          3,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 3
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          4,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 3
          AND ano_turma = 4));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          3,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 5
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          6,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 5
          AND ano_turma = 6));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          7,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 5
          AND ano_turma = 7));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          8,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 5
          AND ano_turma = 8));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          9,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 5
          AND ano_turma = 9));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          1,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 6
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          2,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 6
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          3,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 6
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          2,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          3,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          4,
          3,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 4));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          3,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 3));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          6,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 6));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          7,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 7));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          8,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 8));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          9,
          5,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 5
          AND ano_turma = 9));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          1,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 6
          AND ano_turma = 1));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          2,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 6
          AND ano_turma = 2));


INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          3,
          6,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 6
          AND ano_turma = 3));
