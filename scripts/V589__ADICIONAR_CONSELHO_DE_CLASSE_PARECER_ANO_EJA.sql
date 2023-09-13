--EJA alfabetização II - ANO 1 - ETAPA 2
--Retido por frequência
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
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
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 1
		  AND etapa_eja = 2));

--EJA COMPLEMENTAR I - ANO 3 - ETAPA 1
--Promovido
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 1 
  AND modalidade = 3 
  AND ano_turma = 3
  AND etapa_eja IS NULL;

--Promovido pelo conselho
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 2 
  AND modalidade = 3 
  AND ano_turma = 3
  AND etapa_eja IS NULL;
  
--Retido
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 4 
  AND modalidade = 3
  AND ano_turma = 3
  AND etapa_eja IS NULL;
  
--Retido por frequência
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 5 
  AND modalidade = 3 
  AND ano_turma = 3
  AND etapa_eja IS NULL;
  
--EJA COMPLEMENTAR II - ANO 3 - ETAPA 2
--Promovido  
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          3,
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
          AND ano_turma = 3
		  AND etapa_eja = 2));
		  
--Promovido pelo conselho
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          3,
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
          AND ano_turma = 3
		  AND etapa_eja = 2));
		  
--Retido
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          3,
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
          AND ano_turma = 3
		  AND etapa_eja = 2));
		  
--Retido por frequência
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          3,
          3,
		  2,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 3
		  AND etapa_eja = 2));
		  
--EJA FINAL I - ANO 4 - ETAPA 1
--Promovido
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 1 
  AND modalidade = 3 
  AND ano_turma = 4
  AND etapa_eja IS NULL;

--Promovido pelo conselho
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 2 
  AND modalidade = 3 
  AND ano_turma = 4
  AND etapa_eja IS NULL;
  
--Retido
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 4 
  AND modalidade = 3
  AND ano_turma = 4
  AND etapa_eja IS NULL;
  
--Retido por frequência
UPDATE conselho_classe_parecer_ano 
SET etapa_eja = 1
WHERE parecer_id = 5 
  AND modalidade = 3 
  AND ano_turma = 4
  AND etapa_eja IS NULL;
  
--EJA FINAL II - ANO 4 - ETAPA 2
--Promovido  
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 1,
          4,
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
          AND ano_turma = 4
		  AND etapa_eja = 2));
		  
--Promovido pelo conselho
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 2,
          4,
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
          AND ano_turma = 4
		  AND etapa_eja = 2));
		  
--Retido
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 4,
          4,
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
          AND ano_turma = 4
		  AND etapa_eja = 2));
		  
--Retido por frequência
INSERT INTO public.conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, etapa_eja, inicio_vigencia, criado_por, criado_em, criado_rf)
  (SELECT 5,
          4,
          3,
		  2,
          '2014-01-01',
          'SISTEMA',
          '2014-01-01',
          '0'
   WHERE NOT EXISTS
       (SELECT ccpa.id
        FROM conselho_classe_parecer_ano ccpa
        WHERE ccpa.parecer_id = 5
          AND ccpa.modalidade = 3
          AND ano_turma = 4
		  AND etapa_eja = 2));