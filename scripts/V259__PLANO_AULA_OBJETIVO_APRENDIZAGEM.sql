alter table objetivo_aprendizagem_aula add column if not exists objetivo_aprendizagem_id int8 null;

select
	f_cria_fk_se_nao_existir(
		'objetivo_aprendizagem_aula',
		'objetivo_aprendizagem_aula_objetivo_aprendizagem_fk',
		'FOREIGN KEY (objetivo_aprendizagem_id) REFERENCES objetivo_aprendizagem (id)'
	);

CREATE INDEX objetivo_aprendizagem_aula_objetivo_aprendizagem_idx ON public.objetivo_aprendizagem_aula USING btree (objetivo_aprendizagem_id);
