CREATE table IF NOT EXISTS public.cadastro_acesso_abae (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id int8 not null,
	nome varchar(100) not null,
	cpf varchar(15) not null,
	email varchar(80) not null,
	telefone varchar(15) not null,
	situacao bool NOT NULL DEFAULT false,
	cep varchar(10) not null,
	endereco varchar(200) not null,
	numero integer not null,
	complemento  varchar(20) NULL,
	cidade varchar(50) NULL,
	estado varchar(5) NULL,	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT cadastro_acesso_abae_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists cadastro_acesso_abae_ue_idx ON public.cadastro_acesso_abae USING btree (ue_id);

ALTER TABLE public.cadastro_acesso_abae DROP CONSTRAINT if exists cadastro_acesso_abae_ue_fk;
ALTER TABLE public.cadastro_acesso_abae ADD CONSTRAINT cadastro_acesso_abae_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);