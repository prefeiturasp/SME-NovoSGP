CREATE TABLE IF NOT EXISTS public.notas_tipo_valor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_nota int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	ativo bool NOT NULL DEFAULT false,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT notas_tipo_valor_pk PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.notas_parametros (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor_minimo float4 NULL,
	valor_medio float4 NOT NULL,
	valor_maximo float4 NOT NULL,
	incremento float4 NOT NULL,
	ativo bool NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT notas_parametros_pk PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.conceito_valores (
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
	CONSTRAINT conceito_valores_pk PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.notas_conceitos_ciclos_parametos (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ciclo int4 NOT NULL,
	tipo_nota int8 NOT NULL,
	qtd_minima_avaliacao int4 NOT NULL,
	percentual_alerta int4 NOT NULL,
	ativo bool NOT NULL DEFAULT true,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT notas_conceitos_ciclos_parametos_pk PRIMARY KEY (id),
	CONSTRAINT notas_conceitos_ciclos_parametos_tipo_nota_id_fk FOREIGN KEY (tipo_nota) REFERENCES notas_tipo_valor(id) ON DELETE CASCADE,
	CONSTRAINT notas_conceitos_ciclos_parametos_ciclo_id_fk FOREIGN KEY (ciclo) REFERENCES tipo_ciclo(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS public.notas_conceito (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	atividade_avaliativa int8 NOT NULL,
	aluno_id varchar(20) NOT NULL,
	nota int4 NULL,
	conceito int8 NULL,
	tipo_nota int8 NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	CONSTRAINT notas_conceito_pk PRIMARY KEY (id),
	CONSTRAINT notas_conceito_tipo_nota_id_fk FOREIGN KEY (tipo_nota) REFERENCES notas_tipo_valor(id) ON DELETE CASCADE	
);


CREATE INDEX notas_tipo_valor_tipo_nota_idx ON public.notas_tipo_valor USING btree (tipo_nota);
CREATE INDEX conceito_valores_valor_idx ON public.conceito_valores USING btree (valor);
CREATE INDEX notas_conceitos_ciclos_parametos_ciclo_idx ON public.notas_conceitos_ciclos_parametos USING btree (ciclo);
CREATE INDEX notas_conceito_avaliacao_idx ON public.notas_conceito USING btree (atividade_avaliativa);
CREATE INDEX notas_conceito_aluno_idx ON public.notas_conceito USING btree (aluno_id);
CREATE INDEX notas_conceito_tipo_nota_idx ON public.notas_conceito USING btree (tipo_nota);

insert
	into
	public.notas_tipo_valor (tipo_nota, descricao, inicio_vigencia, ativo, criado_por, criado_rf, criado_em) 
select
	1, 'Nota', now(), true, 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_tipo_valor
	where
		tipo_nota = 1 );
		

insert
	into
	public.notas_tipo_valor (tipo_nota, descricao, inicio_vigencia, ativo, criado_por, criado_rf, criado_em) 
select
	2, 'Conceito', now(), true, 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_tipo_valor
	where
		tipo_nota = 2 );
		
insert
	into
	public.notas_parametros  (valor_minimo, valor_medio, valor_maximo, incremento, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	0,5,10,0.5,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_parametros 
	where
		id = 1 );
		
		
insert
	into
	public.conceito_valores (valor, descricao, aprovado, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	'P', '	Plenamente Satisfatório', true, true, now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.conceito_valores
	where
		valor = 'P' );
		
insert
	into
	public.conceito_valores (valor, descricao, aprovado, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	'S', '	Satisfatório', true, true, now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.conceito_valores
	where
		valor = 'S' );

insert
	into
	public.conceito_valores (valor, descricao, aprovado, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	'NS', '	Não Satisfatório', false, true, now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.conceito_valores
	where
		valor = 'NS' );
		
insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	1,2,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 1 );

insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	2,1,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 2 );
		
insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	3,1,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 3 );
		
insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	4,1,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 4 );
		
insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	5,2,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 5 );
		
insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	6,2,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 6 );
		
insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	7,1,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 7 );
		
insert
	into
	public.notas_conceitos_ciclos_parametos 
	(ciclo, tipo_nota, qtd_minima_avaliacao, percentual_alerta, ativo, inicio_vigencia, criado_por, criado_rf, criado_em) 
select
	8,1,1,50,true,now(), 'Carga Inicial', '0', now()
where
	not exists(
	select
		1
	from
		public.notas_conceitos_ciclos_parametos
	where
		ciclo = 8 );


