drop table if exists plano_aee_reestruturacao;

CREATE table public.plano_aee_reestruturacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_versao_id int8 not null,
	semestre int not null,
	descricao varchar not null,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_reestruturacao_pk PRIMARY KEY (id)
);

CREATE INDEX plano_aee_reestruturacao_versao_idx ON public.plano_aee_reestruturacao USING btree (plano_aee_versao_id);
ALTER TABLE public.plano_aee_reestruturacao ADD CONSTRAINT plano_aee_reestruturacao_versao_fk FOREIGN KEY (plano_aee_versao_id) REFERENCES plano_aee_versao(id);
