CREATE TABLE IF NOT EXISTS public.pedencia_registro_individual(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	titulo varchar(50) NOT NULL,
	pendencia_id int8 NOT NULL,
	turma_id int8 NOT NULL,
	CONSTRAINT pedencia_registro_individual_pk PRIMARY KEY (id),
	CONSTRAINT pendencia_uk UNIQUE (pendencia_id)
);

select
	f_cria_fk_se_nao_existir(
		'pedencia_registro_individual',
		'pedencia_registro_individual_pendencia_fk',
		'FOREIGN KEY (pendencia_id) REFERENCES pendencia (id)'
	);
	
select
	f_cria_fk_se_nao_existir(
		'pedencia_registro_individual',
		'pedencia_registro_individual_turma_fk',
		'FOREIGN KEY (turma_id) REFERENCES turma (id)'
	);
	
CREATE INDEX pedencia_registro_individual_turma_idx ON public.pedencia_registro_individual USING btree (turma_id);

CREATE TABLE IF NOT EXISTS public.pedencia_registro_individual_aluno(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	pedencia_registro_individual_id int8 NOT NULL,
	codigo_aluno int8 NOT NULL,
	CONSTRAINT pedencia_registro_individual_aluno_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'pedencia_registro_individual_aluno',
		'pedencia_registro_individual_aluno_pedencia_registro_individual_fk',
		'FOREIGN KEY (pedencia_registro_individual_id) REFERENCES pedencia_registro_individual (id)'
	);
