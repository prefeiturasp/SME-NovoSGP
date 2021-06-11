drop table if exists plano_aee_resposta;
drop table if exists plano_aee_questao;
drop table if exists plano_aee_versao;
drop table if exists plano_aee;

-- plano AEE
CREATE table public.plano_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	aluno_codigo varchar(15) not null,
	aluno_nome varchar not null,
	aluno_numero int4 not null,
	situacao int4 not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_pk PRIMARY KEY (id)
);

CREATE INDEX plano_aee_turma_idx ON public.plano_aee USING btree (turma_id);
ALTER TABLE public.plano_aee ADD CONSTRAINT plano_aee_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX plano_aee_aluno_idx ON public.plano_aee USING btree (aluno_codigo);


-- Plano AEE Versao
CREATE table public.plano_aee_versao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_id int8 not null,
	numero int4 not null default 1,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_versao_pk PRIMARY KEY (id)
);

CREATE INDEX plano_aee_versao_plano_idx ON public.plano_aee_versao USING btree (plano_aee_id);
ALTER TABLE public.plano_aee_versao ADD CONSTRAINT plano_aee_versao_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id);

CREATE INDEX plano_aee_versao_numero_idx ON public.plano_aee_versao USING btree (numero);


-- Questão Plano AEE
CREATE table public.plano_aee_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_versao_id int8 not null,
	questao_id int8 not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_questao_pk PRIMARY KEY (id)
);

CREATE INDEX plano_aee_questao_versao_idx ON public.plano_aee_questao USING btree (plano_aee_versao_id);
ALTER TABLE public.plano_aee_questao ADD CONSTRAINT plano_aee_questao_versao_fk FOREIGN KEY (plano_aee_versao_id) REFERENCES plano_aee_versao(id);

CREATE INDEX plano_aee_questao_questao_idx ON public.plano_aee_questao USING btree (questao_id);
ALTER TABLE public.plano_aee_questao ADD CONSTRAINT plano_aee_questao_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);


-- Resposta Plano AEE
CREATE table public.plano_aee_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_questao_id int8 not null,
	resposta_id int8 null,
	arquivo_id int8 null,
	texto varchar null,
	periodo_inicio timestamp null,
	periodo_fim timestamp null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_resposta_pk PRIMARY KEY (id)
);

CREATE INDEX plano_aee_resposta_questao_idx ON public.plano_aee_resposta USING btree (plano_questao_id);
ALTER TABLE public.plano_aee_resposta ADD CONSTRAINT plano_aee_resposta_questao_fk FOREIGN KEY (plano_questao_id) REFERENCES plano_aee_questao(id);

CREATE INDEX plano_aee_resposta_resposta_idx ON public.plano_aee_resposta USING btree (resposta_id);
ALTER TABLE public.plano_aee_resposta ADD CONSTRAINT plano_aee_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id);

CREATE INDEX plano_aee_resposta_arquivo_idx ON public.plano_aee_resposta USING btree (arquivo_id);
ALTER TABLE public.plano_aee_resposta ADD CONSTRAINT plano_aee_resposta_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
