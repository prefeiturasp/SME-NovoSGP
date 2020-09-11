alter table recuperacao_paralela 
add column if not exists ano_letivo int null;

update recuperacao_paralela set ano_letivo = 2020;

alter table recuperacao_paralela 
alter column ano_letivo
set not null;


-- Melhorando modalegam -> turma_id
alter table recuperacao_paralela 
add column if not exists turma_id_n bigint null;

UPDATE recuperacao_paralela 
SET turma_id_n = t.id 
FROM (
    SELECT id, turma_id 
    FROM turma) AS t
WHERE 
    t.turma_id = recuperacao_paralela.turma_id;
   
   alter table recuperacao_paralela 
   drop column turma_id;
  
  alter table recuperacao_paralela 
  RENAME turma_id_n to turma_id;

 
 ALTER TABLE recuperacao_paralela ALTER COLUMN turma_id SET NOT NULL;
 
  select
	f_cria_fk_se_nao_existir(
		'recuperacao_paralela',
		'recuperacao_paralela_turma_fk',
		'FOREIGN KEY (turma_id) REFERENCES turma (id)'
	);

CREATE INDEX IF NOT EXISTS recuperacao_paralela_turma_idx ON public.recuperacao_paralela USING btree (turma_id);



-- Melhorando modalegam -> turma_recuperacao_paralela_id
alter table recuperacao_paralela 
add column if not exists turma_recuperacao_paralela_id_n bigint null;

UPDATE recuperacao_paralela 
SET turma_recuperacao_paralela_id_n = t.id 
FROM (
    SELECT id, turma_id 
    FROM turma) AS t
WHERE 
    t.turma_id = recuperacao_paralela.turma_recuperacao_paralela_id;
   
  alter table recuperacao_paralela 
   drop column turma_recuperacao_paralela_id;
  
  alter table recuperacao_paralela 
  RENAME turma_recuperacao_paralela_id_n to turma_recuperacao_paralela_id;

 
 ALTER TABLE recuperacao_paralela ALTER COLUMN turma_recuperacao_paralela_id SET NOT NULL;
   
 
  select
	f_cria_fk_se_nao_existir(
		'recuperacao_paralela',
		'recuperacao_paralela_turma_rp_fk',
		'FOREIGN KEY (turma_recuperacao_paralela_id) REFERENCES turma (id)'
	);

CREATE INDEX IF NOT EXISTS recuperacao_paralela_turma_rp_idx ON public.recuperacao_paralela USING btree (turma_recuperacao_paralela_id);


-- Melhorando modalegam -> eixo

ALTER TABLE eixo 
    RENAME TO recuperacao_paralela_eixo;
    
-- Melhorando modalegam -> resposta

ALTER TABLE resposta 
    RENAME TO recuperacao_paralela_resposta;


-- objetivos  

ALTER TABLE objetivo_resposta 
	drop constraint  objetivo_resposta_objetivo_fk;

ALTER TABLE objetivo 
    RENAME TO recuperacao_paralela_objetivo;
   
ALTER TABLE objetivo_desenvolvimento 
    RENAME TO recuperacao_paralela_objetivo_desenvolvimento;
   
ALTER TABLE objetivo_desenvolvimento_plano 
    RENAME TO recuperacao_paralela_objetivo_desenvolvimento_plano;
   
ALTER TABLE objetivo_resposta 
    RENAME TO recuperacao_paralela_objetivo_resposta;

   
   
   
 









