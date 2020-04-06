CREATE TABLE IF NOT EXISTS public.sintese_valores (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor varchar(50) NOT NULL,
	descricao varchar(200) NOT NULL,
	aprovado bool NOT NULL DEFAULT true,
	ativo bool NOT NULL DEFAULT true,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT sinstese_valores_pk PRIMARY KEY (id)
);

CREATE INDEX sintese_valores_valor_idx ON public.conceito_valores USING btree (valor);

insert
	into
	public.sintese_valores (valor, descricao, aprovado, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	'F', '	Frequente', true, true, now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.sintese_valores
	where
		valor = 'F' );
		
insert
	into
	public.sintese_valores (valor, descricao, aprovado, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	'NF', '	Não Frequente', false, true, now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.sintese_valores
	where
		valor = 'S' );

ALTER TABLE public.nota_conceito_bimestre ADD COLUMN if not exists sintese_id int8 null;
ALTER TABLE public.nota_conceito_bimestre ADD CONSTRAINT nota_conceito_bimestre_sintese_fk FOREIGN KEY (sintese_id) REFERENCES sintese_valores(id);
