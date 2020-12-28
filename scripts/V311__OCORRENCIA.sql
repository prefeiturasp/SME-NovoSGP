-- Tipo de ocorrência
CREATE TABLE IF NOT EXISTS public.tipo_ocorrencia(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	descricao varchar(20) NOT NULL,
	CONSTRAINT documento_pk PRIMARY KEY (id)
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
	tipo_ocorrencia_id int8 NOT NULL,
	excluido boolean NOT NULL DEFAULT FALSE,
	CONSTRAINT documento_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'ocorrencia',
		'ocorrencia_tipo_ocorrencia_fk',
		'FOREIGN KEY (tipo_ocorrencia_id) REFERENCES tipo_ocorrencia (id)'
	);
	
CREATE INDEX ocorrencia_tipo_ocorrencia_idx ON public.ocorrencia USING btree (tipo_ocorrencia_id);

-- Aluno da ocorrência
CREATE TABLE IF NOT EXISTS public.ocorrencia_aluno(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	codigo_aluno varchar(15) NOT NULL,
	ocorrencia_id int8 NOT NULL,
	CONSTRAINT documento_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'ocorrencia_aluno',
		'ocorrencia_aluno_ocorrencia_fk',
		'FOREIGN KEY (ocorrencia_id) REFERENCES ocorrencia (id)'
	);