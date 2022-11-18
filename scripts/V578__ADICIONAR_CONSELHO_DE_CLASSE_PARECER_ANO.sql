--7 Ano Fundamental
--Retido
DELETE FROM conselho_classe_parecer_ano WHERE parecer_id = 4 AND modalidade = 5 AND ano_turma = 7;
--Promovido
DELETE FROM conselho_classe_parecer_ano WHERE parecer_id = 1 AND modalidade = 5 AND ano_turma = 7;
--Promovido pelo conselho
DELETE FROM conselho_classe_parecer_ano WHERE parecer_id = 2 AND modalidade = 5 AND ano_turma = 7;

--Continuidade dos estudos
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          7,
          5,
          '2014-01-01',
          'SISTEMA',
          '2022-11-18',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 7));

--8 Ano Fundamental
--Retido
DELETE FROM conselho_classe_parecer_ano WHERE parecer_id = 4 AND modalidade = 5 AND ano_turma = 8;
--Promovido
DELETE FROM conselho_classe_parecer_ano WHERE parecer_id = 1 AND modalidade = 5 AND ano_turma = 8;
--Promovido pelo conselho
DELETE FROM conselho_classe_parecer_ano WHERE parecer_id = 2 AND modalidade = 5 AND ano_turma = 8;

--Continuidade dos estudos
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          8,
          5,
          '2014-01-01',
          'SISTEMA',
          '2022-11-18',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 8));
		  
--EJA 1 ANO 
--Continuidade dos estudos
DELETE FROM conselho_classe_parecer_ano WHERE parecer_id = 3 AND modalidade = 3 AND ano_turma = 1;

--Retido
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          1,
          3,
          '2014-01-01',
          'SISTEMA',
          '2022-11-18',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 3
          AND ano_turma = 1));

--Promovido         
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          1,
          3,
          '2014-01-01',
          'SISTEMA',
          '2022-11-18',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 3
          AND ano_turma = 1));

--Promovido pelo conselho        
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          1,
          3,
          '2014-01-01',
          'SISTEMA',
          '2022-11-18',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 3
          AND ano_turma = 1));