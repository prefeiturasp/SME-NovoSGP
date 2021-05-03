-- DROP SCHEMA public;

CREATE SCHEMA public AUTHORIZATION postgres;

-- DROP SEQUENCE public.componentecurriculargrupomatriz_id_seq;

CREATE SEQUENCE public.componentecurriculargrupomatriz_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;-- public.abrangencia definition

-- Drop table

-- DROP TABLE public.abrangencia;

CREATE TABLE public.abrangencia (
	id int4 NOT NULL,
	descricao varchar(30) NOT NULL,
	CONSTRAINT abrangencia_pk PRIMARY KEY (id)
);


-- public.acoes definition

-- Drop table

-- DROP TABLE public.acoes;

CREATE TABLE public.acoes (
	id int4 NOT NULL,
	descricao varchar(20) NOT NULL,
	CONSTRAINT acoes_pk PRIMARY KEY (id)
);


-- public.componentecurricular definition

-- Drop table

-- DROP TABLE public.componentecurricular;

CREATE TABLE public.componentecurricular (
	idcomponentecurricular int8 NOT NULL,
	ehcompartilhada bool NOT NULL DEFAULT false,
	ehregencia bool NOT NULL DEFAULT false,
	permiteregistrofrequencia bool NOT NULL DEFAULT false,
	idcomponentecurricularpai int8 NULL,
	ehterritorio bool NOT NULL DEFAULT false,
	permitelancamentodenota bool NOT NULL DEFAULT true,
	ehbasenacional bool NOT NULL DEFAULT false,
	idgrupomatriz int8 NOT NULL DEFAULT 0,
	idareadoconhecimento int8 NULL,
	descricao varchar(100) NULL
);


-- public.componentecurricularareadoconhecimento definition

-- Drop table

-- DROP TABLE public.componentecurricularareadoconhecimento;

CREATE TABLE public.componentecurricularareadoconhecimento (
	id int8 NOT NULL,
	nome varchar(200) NOT NULL
);


-- public.componentecurriculargrupomatriz definition

-- Drop table

-- DROP TABLE public.componentecurriculargrupomatriz;

CREATE TABLE public.componentecurriculargrupomatriz (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) NOT NULL
);


-- public.componentecurricularpap definition

-- Drop table

-- DROP TABLE public.componentecurricularpap;

CREATE TABLE public.componentecurricularpap (
	id int8 NULL
);


-- public.flyway_schema_history definition

-- Drop table

-- DROP TABLE public.flyway_schema_history;

CREATE TABLE public.flyway_schema_history (
	installed_rank int4 NOT NULL,
	"version" varchar(50) NULL,
	description varchar(200) NOT NULL,
	"type" varchar(20) NOT NULL,
	script varchar(1000) NOT NULL,
	checksum int4 NULL,
	installed_by varchar(100) NOT NULL,
	installed_on timestamp NOT NULL DEFAULT now(),
	execution_time int4 NOT NULL,
	success bool NOT NULL,
	CONSTRAINT flyway_schema_history_pk PRIMARY KEY (installed_rank)
);
CREATE INDEX flyway_schema_history_s_idx ON public.flyway_schema_history USING btree (success);


-- public.parametros definition

-- Drop table

-- DROP TABLE public.parametros;

CREATE TABLE public.parametros (
	nome varchar(100) NULL,
	valor varchar(100) NULL
);


-- public.regenciacomponentecurricular definition

-- Drop table

-- DROP TABLE public.regenciacomponentecurricular;

CREATE TABLE public.regenciacomponentecurricular (
	idcomponentecurricular int8 NOT NULL,
	turno int8 NULL,
	ano int8 NULL,
	idgrupomatriz int8 NOT NULL DEFAULT 0
);


-- public.rf_servidor definition

-- Drop table

-- DROP TABLE public.rf_servidor;

CREATE TABLE public.rf_servidor (
	codigo_rf bpchar(7) NOT NULL,
	codigo_servidor int4 NOT NULL,
	CONSTRAINT rf_pkey PRIMARY KEY (codigo_rf)
);


-- public.grupos definition

-- Drop table

-- DROP TABLE public.grupos;

CREATE TABLE public.grupos (
	id int8 NOT NULL,
	guidperfil uuid NOT NULL,
	nome varchar(50) NOT NULL,
	cdtipofuncaoatividade int4 NULL,
	idabrangencia int4 NOT NULL,
	ehperfilmanual bool NOT NULL DEFAULT false,
	CONSTRAINT grupos_pk PRIMARY KEY (id),
	CONSTRAINT grupos_abrangencia_fk FOREIGN KEY (idabrangencia) REFERENCES abrangencia(id)
);


-- public.modulos definition

-- Drop table

-- DROP TABLE public.modulos;

CREATE TABLE public.modulos (
	id int4 NOT NULL,
	descricao varchar(150) NOT NULL,
	idmodcoresso int4 NULL,
	idacao int4 NOT NULL,
	CONSTRAINT modulos_pk PRIMARY KEY (id),
	CONSTRAINT modulos_acao_fk FOREIGN KEY (idacao) REFERENCES acoes(id)
);


-- public.permissoes definition

-- Drop table

-- DROP TABLE public.permissoes;

CREATE TABLE public.permissoes (
	idgrupo int8 NOT NULL,
	idmodulo int8 NOT NULL,
	CONSTRAINT permissoes_pk PRIMARY KEY (idgrupo, idmodulo),
	CONSTRAINT permissoes_grupo_fk FOREIGN KEY (idgrupo) REFERENCES grupos(id),
	CONSTRAINT permissoes_modulo_fk FOREIGN KEY (idmodulo) REFERENCES modulos(id)
);


-- public.grupocargos definition

-- Drop table

-- DROP TABLE public.grupocargos;

CREATE TABLE public.grupocargos (
	idgrupo int8 NOT NULL,
	cargo int4 NOT NULL,
	CONSTRAINT grupocargos_pk PRIMARY KEY (idgrupo, cargo),
	CONSTRAINT grupocargos_grupos_fk FOREIGN KEY (idgrupo) REFERENCES grupos(id)
);



CREATE OR REPLACE FUNCTION public.uuid_generate_v1()
 RETURNS uuid
 LANGUAGE c
 PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_generate_v1$function$
;

CREATE OR REPLACE FUNCTION public.uuid_generate_v1mc()
 RETURNS uuid
 LANGUAGE c
 PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_generate_v1mc$function$
;

CREATE OR REPLACE FUNCTION public.uuid_generate_v3(namespace uuid, name text)
 RETURNS uuid
 LANGUAGE c
 IMMUTABLE PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_generate_v3$function$
;

CREATE OR REPLACE FUNCTION public.uuid_generate_v4()
 RETURNS uuid
 LANGUAGE c
 PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_generate_v4$function$
;

CREATE OR REPLACE FUNCTION public.uuid_generate_v5(namespace uuid, name text)
 RETURNS uuid
 LANGUAGE c
 IMMUTABLE PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_generate_v5$function$
;

CREATE OR REPLACE FUNCTION public.uuid_nil()
 RETURNS uuid
 LANGUAGE c
 IMMUTABLE PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_nil$function$
;

CREATE OR REPLACE FUNCTION public.uuid_ns_dns()
 RETURNS uuid
 LANGUAGE c
 IMMUTABLE PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_ns_dns$function$
;

CREATE OR REPLACE FUNCTION public.uuid_ns_oid()
 RETURNS uuid
 LANGUAGE c
 IMMUTABLE PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_ns_oid$function$
;

CREATE OR REPLACE FUNCTION public.uuid_ns_url()
 RETURNS uuid
 LANGUAGE c
 IMMUTABLE PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_ns_url$function$
;

CREATE OR REPLACE FUNCTION public.uuid_ns_x500()
 RETURNS uuid
 LANGUAGE c
 IMMUTABLE PARALLEL SAFE STRICT
AS '$libdir/uuid-ossp', $function$uuid_ns_x500$function$
;
