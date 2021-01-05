-- Tipo de ocorrência
CREATE TABLE IF NOT EXISTS public.ocorrencia_tipo(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	descricao varchar(20) NOT NULL,
	excluido boolean NOT NULL DEFAULT FALSE,
	CONSTRAINT ocorrencia_tipo_pk PRIMARY KEY (id)
);

-- Ocorrência
CREATE TABLE IF NOT EXISTS public.ocorrencia(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	titulo varchar(50) NOT NULL,
	data_ocorrencia timestamp NOT NULL,
	hora_ocorrencia time NULL,
	descricao varchar NOT NULL,
	ocorrencia_tipo_id int8 NOT NULL,
	turma_id int8 NOT NULL,
	excluido boolean NOT NULL DEFAULT FALSE,
	CONSTRAINT ocorrencia_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'ocorrencia',
		'ocorrencia_ocorrencia_tipo_fk',
		'FOREIGN KEY (ocorrencia_tipo_id) REFERENCES ocorrencia_tipo (id)'
	);
	
select
	f_cria_fk_se_nao_existir(
		'ocorrencia',
		'ocorrencia_turma_fk',
		'FOREIGN KEY (turma_id) REFERENCES turma (id)'
	);

CREATE INDEX ocorrencia_turma_idx ON public.ocorrencia USING btree (turma_id);
CREATE INDEX ocorrencia_ocorrencia_tipo_idx ON public.ocorrencia USING btree (ocorrencia_tipo_id);
CREATE INDEX ocorrencia_data_ocorrencia_idx ON public.ocorrencia USING btree (data_ocorrencia);
CREATE INDEX ocorrencia_titulo_idx ON public.ocorrencia USING btree (titulo);

-- Aluno da ocorrência
CREATE TABLE IF NOT EXISTS public.ocorrencia_aluno(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno int8 NOT NULL,
	ocorrencia_id int8 NOT NULL,
	CONSTRAINT ocorrencia_aluno_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'ocorrencia_aluno',
		'ocorrencia_aluno_ocorrencia_fk',
		'FOREIGN KEY (ocorrencia_id) REFERENCES ocorrencia (id)'
	);
	
CREATE INDEX ocorrencia_aluno_codigo_aluno_idx ON public.ocorrencia_aluno USING btree (codigo_aluno);