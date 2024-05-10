
CREATE TABLE IF NOT EXISTS public.historico_nota (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nota_anterior numeric(5,2) NOT NULL,
	nota_nova numeric(5,2) NOT NULL,	
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT null,
	CONSTRAINT historico_nota_pk PRIMARY KEY (id)
);


CREATE TABLE IF NOT EXISTS public.historico_nota_fechamento(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	historico_nota_id int8 NOT NULL,
	fechamento_nota_id int8 NOT NULL,
	CONSTRAINT historico_nota_fechamento_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'historico_nota_fechamento',
		'historico_nota_fechamento_fechamento_nota_fk',
		'FOREIGN KEY (fechamento_nota_id) REFERENCES fechamento_nota (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'historico_nota_fechamento',
		'historico_nota_fechamento_historico_nota_fk',
		'FOREIGN KEY (historico_nota_id) REFERENCES historico_nota (id)'
	);


CREATE TABLE IF NOT EXISTS public.historico_nota_conselho_classe(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	historico_nota_id int8 NOT NULL,
	conselho_classe_nota_id int8 NOT NULL,
	CONSTRAINT historico_nota_conselho_classe_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'historico_nota_conselho_classe',
		'historico_nota_conselho_classe_conselho_classe_nota_fk',
		'FOREIGN KEY (conselho_classe_nota_id) REFERENCES conselho_classe_nota (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'historico_nota_conselho_classe',
		'historico_nota_conselho_classe_historico_nota_fk',
		'FOREIGN KEY (historico_nota_id) REFERENCES historico_nota (id)'
	);

CREATE INDEX historico_nota_fechamento_historico_nota_idx ON public.historico_nota_fechamento USING btree (historico_nota_id);
CREATE INDEX historico_nota_fechamento_fechamento_nota_idx ON public.historico_nota_fechamento USING btree (fechamento_nota_id);

CREATE INDEX historico_nota_conselho_classe_historico_nota_idx ON public.historico_nota_fechamento USING btree (historico_nota_id);
CREATE INDEX historico_nota_conselho_classe_conselho_classe_nota_idx ON public.historico_nota_conselho_classe USING btree (conselho_classe_nota_id);
