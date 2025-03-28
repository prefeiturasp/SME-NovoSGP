CREATE table if not exists public.consolidacao_reflexo_frequencia_busca_ativa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id varchar(15) not null,
	ue_id varchar(15) not null,
	ano_letivo int4 not null,
	modalidade_codigo int4 not null,
	aluno_codigo varchar(15) not null,
	aluno_nome varchar not null,
	data_acao date not null,
	mes int4 not null,
	percentual_frequencia_anterior_acao numeric(5,2) not null,
	percentual_frequencia_atual numeric(5,2) not null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT consolidacao_reflexo_frequencia_buscaativa_pk PRIMARY KEY (id)
);                  