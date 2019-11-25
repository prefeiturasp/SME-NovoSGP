CREATE TABLE public.notas_tipo_valor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_nota int4 NOT NULL,
	descricao varchar(200) NOT NULL,
	tipo_valor int4 NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL,
	ativo bool NOT NULL DEFAULT false
);

CREATE TABLE public.notas_parametros (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	valor_minimo float4 NULL,
	valor_medio float4 NOT NULL,
	valor_maximo float4 NOT NULL,
	incremento float4 NOT NULL,
	ativo bool NOT NULL,
	inicio_vigencia timestamp NOT NULL,
	fim_vigencia timestamp NULL
);

