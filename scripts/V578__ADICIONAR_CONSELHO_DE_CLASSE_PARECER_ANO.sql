ALTER TABLE conselho_classe_parecer_ano ADD COLUMN IF NOT EXISTS etapa_eja int4 null;

--7 Ano Fundamental
--Retido
UPDATE conselho_classe_parecer_ano 
SET fim_vigencia = '2021-12-31'
WHERE parecer_id = 4 
  AND modalidade = 5 
  AND ano_turma = 7
  AND inicio_vigencia = '2014-01-01';
  
--Promovido
UPDATE conselho_classe_parecer_ano 
SET fim_vigencia = '2021-12-31'
WHERE parecer_id = 1 
  AND modalidade = 5 
  AND ano_turma = 7
  AND inicio_vigencia = '2014-01-01';
  
--Promovido pelo conselho
UPDATE conselho_classe_parecer_ano 
SET fim_vigencia = '2021-12-31'
WHERE parecer_id = 2 
  AND modalidade = 5 
  AND ano_turma = 7
  AND inicio_vigencia = '2014-01-01';

--Continuidade dos estudos
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          7,
          5,
          '2022-01-01',
          'SISTEMA',
          '2022-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 7
		  AND inicio_vigencia = '2022-01-01'));

--8 Ano Fundamental
--Retido
UPDATE conselho_classe_parecer_ano 
SET fim_vigencia = '2021-12-31'
WHERE parecer_id = 4 
  AND modalidade = 5 
  AND ano_turma = 8
  AND inicio_vigencia = '2014-01-01';
  
--Promovido
UPDATE conselho_classe_parecer_ano 
SET fim_vigencia = '2021-12-31'  
WHERE parecer_id = 1 
  AND modalidade = 5 
  AND ano_turma = 8
  AND inicio_vigencia = '2014-01-01';
  
--Promovido pelo conselho
UPDATE conselho_classe_parecer_ano 
SET fim_vigencia = '2021-12-31'
WHERE parecer_id = 2 
  AND modalidade = 5 
  AND ano_turma = 8
  AND inicio_vigencia = '2014-01-01';

--Continuidade dos estudos
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          8,
          5,
          '2022-01-01',
          'SISTEMA',
          '2022-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 5
          AND ano_turma = 8
		  AND inicio_vigencia = '2022-01-01'));
		  
--EJA 
--EJA ALFABETIZACAO I
--Continuidade dos estudos
UPDATE conselho_classe_parecer_ano 
SET fim_vigencia = null, etapa_eja = 1  
WHERE parecer_id = 3 
  AND modalidade = 3 
  AND ano_turma = 1
  AND etapa_eja IS NULL;

--Retido por frequência
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1  
WHERE parecer_id = 5 
  AND modalidade = 3 
  AND ano_turma = 1
  AND etapa_eja IS NULL;

--Retido alfabetização II
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          1,
          3,
		  2,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 4
          AND ccpa.modalidade = 3
          AND ano_turma = 1
		  AND etapa_eja = 2));

--Promovido         
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          1,
          3,
		  2,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 1
          AND ccpa.modalidade = 3
          AND ano_turma = 1
		  AND etapa_eja = 2));

--Promovido pelo conselho        
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          1,
          3,
		  2,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 2
          AND ccpa.modalidade = 3
          AND ano_turma = 1
		  AND etapa_eja = 2));
		   
--EJA BASICA I

--Continuidade dos estudos
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 3,
          2,
          3,
		  1,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 3
          AND ccpa.modalidade = 3
          AND ano_turma = 2
		  AND etapa_eja = 1));		   
		  
--Retido por frequência
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          2,
          3,
		  1,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 2
		  AND etapa_eja = 1));		
		  
--EJA BASICA II

--Retido por frequência
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 2  
WHERE parecer_id = 5 
  AND modalidade = 3 
  AND ano_turma = 2
  AND etapa_eja IS NULL;
  
--Retido
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 2 
WHERE parecer_id = 4 
  AND modalidade = 3
  AND ano_turma = 2
  AND etapa_eja IS NULL;
  
--Promovido
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 2 
WHERE parecer_id = 1 
  AND modalidade = 3 
  AND ano_turma = 2
  AND etapa_eja IS NULL;
  
--Promovido pelo conselho
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 2 
WHERE parecer_id = 2 
  AND modalidade = 3 
  AND ano_turma = 2
  AND etapa_eja IS NULL;