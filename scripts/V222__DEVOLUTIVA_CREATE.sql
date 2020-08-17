-- Data de criação: 17/08/2020
-- Descrição: recria tabela devolutiva

drop table if exists public.devolutiva_diario_bordo
drop table if exists public.devolutiva

CREATE TABLE public.devolutiva (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar not null,
	componente_curricular_codigo int8 NOT NULL,
	periodo_inicio timestamp NOT NULL,
	periodo_fim timestamp NOT NULL,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT devolutiva_pk PRIMARY KEY (id)
);