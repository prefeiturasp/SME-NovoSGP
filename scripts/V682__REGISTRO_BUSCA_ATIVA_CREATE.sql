drop table if exists registro_acao_busca_ativa_resposta;
drop table if exists registro_acao_busca_ativa_questao;
drop table if exists registro_acao_busca_ativa_secao;
drop table if exists registro_acao_busca_ativa;
drop table if exists secao_registro_acao_busca_ativa;

-- SECAO Registro Busca Ativa
CREATE table public.secao_registro_acao_busca_ativa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questionario_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	nome varchar,
	ordem int4,
	etapa int4,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT secao_registro_acao_busca_ativa_pk PRIMARY KEY (id)
);

CREATE INDEX secao_registro_acao_busca_ativa_questionario_idx ON public.secao_registro_acao_busca_ativa USING btree (questionario_id);
ALTER TABLE public.secao_registro_acao_busca_ativa ADD CONSTRAINT secao_registro_acao_busca_ativa_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id);


-- Registro Busca Ativa
CREATE table public.registro_acao_busca_ativa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	aluno_codigo varchar(15) not null,
	aluno_nome varchar not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT registro_acao_busca_ativa_pk PRIMARY KEY (id)
);

CREATE INDEX registro_acao_busca_ativa_turma_idx ON public.registro_acao_busca_ativa USING btree (turma_id);
ALTER TABLE public.registro_acao_busca_ativa ADD CONSTRAINT registro_acao_busca_ativa_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX registro_acao_busca_ativa_aluno_idx ON public.registro_acao_busca_ativa USING btree (aluno_codigo);

-- registro_acao_busca_ativa_SECAO
CREATE table public.registro_acao_busca_ativa_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	registro_acao_busca_ativa_id int8 not null,
	secao_registro_acao_id int8 not null,
	concluido bool not null default false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT registro_acao_busca_ativa_secao_pk PRIMARY KEY (id)
);

CREATE INDEX registro_acao_busca_ativa_secao_registro_acao_idx ON public.registro_acao_busca_ativa_secao USING btree (registro_acao_busca_ativa_id);
ALTER TABLE public.registro_acao_busca_ativa_secao ADD CONSTRAINT registro_acao_busca_ativa_secao_registro_acao_fk FOREIGN KEY (registro_acao_busca_ativa_id) REFERENCES registro_acao_busca_ativa(id);

CREATE INDEX registro_acao_busca_ativa_secao_secao_idx ON public.registro_acao_busca_ativa_secao USING btree (secao_registro_acao_id);
ALTER TABLE public.registro_acao_busca_ativa_secao ADD CONSTRAINT registro_acao_busca_ativa_secao_secao_fk FOREIGN KEY (secao_registro_acao_id) REFERENCES secao_registro_acao_busca_ativa(id);


-- Questão Registro Busca Ativa
CREATE table public.registro_acao_busca_ativa_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	registro_acao_busca_ativa_secao_id int8 not null,
	questao_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT registro_acao_busca_ativa_questao_pk PRIMARY KEY (id)
);

CREATE INDEX registro_acao_busca_ativa_questao_questao_idx ON public.registro_acao_busca_ativa_questao USING btree (questao_id);
ALTER TABLE public.registro_acao_busca_ativa_questao ADD CONSTRAINT registro_acao_busca_ativa_questao_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);

CREATE INDEX registro_acao_busca_ativa_questao_secao_idx ON public.registro_acao_busca_ativa_questao USING btree (registro_acao_busca_ativa_secao_id);
ALTER TABLE public.registro_acao_busca_ativa_questao ADD CONSTRAINT registro_acao_busca_ativa_questao_secao_fk FOREIGN KEY (registro_acao_busca_ativa_secao_id) REFERENCES registro_acao_busca_ativa_secao(id);

-- Resposta Registro Busca Ativa
CREATE table public.registro_acao_busca_ativa_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_registro_acao_id int8 not null,
	resposta_id int8 null,
	arquivo_id int8 null,
	texto varchar null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT registro_acao_busca_ativa_resposta_pk PRIMARY KEY (id)
);

CREATE INDEX registro_acao_busca_ativa_resposta_questao_idx ON public.registro_acao_busca_ativa_resposta USING btree (questao_registro_acao_id);
ALTER TABLE public.registro_acao_busca_ativa_resposta ADD CONSTRAINT registro_acao_busca_ativa_resposta_questao_fk FOREIGN KEY (questao_registro_acao_id) REFERENCES registro_acao_busca_ativa_questao(id);

CREATE INDEX registro_acao_busca_ativa_resposta_resposta_idx ON public.registro_acao_busca_ativa_resposta USING btree (resposta_id);
ALTER TABLE public.registro_acao_busca_ativa_resposta ADD CONSTRAINT registro_acao_busca_ativa_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id);

CREATE INDEX registro_acao_busca_ativa_resposta_arquivo_idx ON public.registro_acao_busca_ativa_resposta USING btree (arquivo_id);
ALTER TABLE public.registro_acao_busca_ativa_resposta ADD CONSTRAINT registro_acao_busca_ativa_resposta_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
