alter table itinerancia drop if exists ano_letivo;

alter table itinerancia add column ano_letivo int4 null;


update itinerancia 
   set ano_letivo = 2021
 where ano_letivo is null;
  

alter table itinerancia alter column ano_letivo set not null;


alter table itinerancia_aluno drop if exists turma_id;

alter table itinerancia_aluno add column turma_id int4 null;

select
	f_cria_fk_se_nao_existir(
		'itinerancia_aluno',
		'itinerancia_aluno_turma_fk',
		'FOREIGN KEY (turma_id) REFERENCES turma (id)'
	);

CREATE INDEX itinerancia_aluno_turma__idx ON public.itinerancia_aluno USING btree (turma_id);