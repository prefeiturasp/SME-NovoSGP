CREATE TABLE IF NOT EXISTS public.notas_tipo_valor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_nota int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	tipo_valor int4 NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	ativo bool NOT NULL DEFAULT false
);

CREATE TABLE IF NOT EXISTS public.notas_parametros (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor_minimo float4 NULL,
	valor_medio float4 NOT NULL,
	valor_maximo float4 NOT NULL,
	incremento float4 NOT NULL,
	ativo bool NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL
);

CREATE TABLE IF NOT EXISTS public.conceito_valores (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor varchar(50) NOT NULL,
	descricao varchar(200) NOT NULL,
	aprovado bool NOT NULL DEFAULT true,
	ativo bool NOT NULL DEFAULT true,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL
);

CREATE TABLE public.notas_conceitos_ciclos_parametos (
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
	alterado_em timestamp NULL
);