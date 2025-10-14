CREATE table if not exists public.consolidacao_alfabetizacao_critica_escrita (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	dre_codigo varchar(15) not null,
	ue_codigo varchar(15) not null,	
	dre_nome varchar(100) not null,
	ue_nome varchar(200) not null,
	posicao int2 not null,
	total_alunos_nao_alfabetizados int8 not null,
	percentual_total_alunos NUMERIC(5, 2) not null,
	CONSTRAINT consolidacao_alfabetizacao_critica_escrita_pk PRIMARY KEY (id)
);