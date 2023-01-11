drop table if exists encaminhamento_naapa_resposta;
drop table if exists encaminhamento_naapa_questao;
drop table if exists encaminhamento_naapa_secao;
drop table if exists encaminhamento_naapa;
drop table if exists secao_encaminhamento_naapa;

-- SECAO Encaminhamento NAAPA
CREATE table public.secao_encaminhamento_naapa (
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
	CONSTRAINT secao_encaminhamento_naapa_pk PRIMARY KEY (id)
);

CREATE INDEX secao_encaminhamento_naapa_questionario_idx ON public.secao_encaminhamento_naapa USING btree (questionario_id);
ALTER TABLE public.secao_encaminhamento_naapa ADD CONSTRAINT secao_encaminhamento_naapa_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id);


-- Encaminhamento NAAPA
CREATE table public.encaminhamento_naapa (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	aluno_codigo varchar(15) not null,
	aluno_nome varchar not null,
	situacao int4 not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_naapa_pk PRIMARY KEY (id)
);

CREATE INDEX encaminhamento_naapa_turma_idx ON public.encaminhamento_naapa USING btree (turma_id);
ALTER TABLE public.encaminhamento_naapa ADD CONSTRAINT encaminhamento_naapa_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX encaminhamento_naapa_aluno_idx ON public.encaminhamento_naapa USING btree (aluno_codigo);

-- encaminhamento_naapa_SECAO
CREATE table public.encaminhamento_naapa_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_naapa_id int8 not null,
	secao_encaminhamento_id int8 not null,
	concluido bool not null default false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_naapa_secao_pk PRIMARY KEY (id)
);

CREATE INDEX encaminhamento_naapa_secao_encaminhamento_idx ON public.encaminhamento_naapa_secao USING btree (encaminhamento_naapa_id);
ALTER TABLE public.encaminhamento_naapa_secao ADD CONSTRAINT encaminhamento_naapa_secao_encaminhamento_fk FOREIGN KEY (encaminhamento_naapa_id) REFERENCES encaminhamento_naapa(id);

CREATE INDEX encaminhamento_naapa_secao_secao_idx ON public.encaminhamento_naapa_secao USING btree (secao_encaminhamento_id);
ALTER TABLE public.encaminhamento_naapa_secao ADD CONSTRAINT encaminhamento_naapa_secao_secao_fk FOREIGN KEY (secao_encaminhamento_id) REFERENCES secao_encaminhamento_naapa(id);


-- Questão Encaminhamento NAAPA
CREATE table public.encaminhamento_naapa_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_naapa_secao_id int8 not null,
	questao_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_naapa_questao_pk PRIMARY KEY (id)
);

CREATE INDEX encaminhamento_naapa_questao_questao_idx ON public.encaminhamento_naapa_questao USING btree (questao_id);
ALTER TABLE public.encaminhamento_naapa_questao ADD CONSTRAINT encaminhamento_naapa_questao_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);

CREATE INDEX encaminhamento_naapa_questao_secao_idx ON public.encaminhamento_naapa_questao USING btree (encaminhamento_naapa_secao_id);
ALTER TABLE public.encaminhamento_naapa_questao ADD CONSTRAINT encaminhamento_naapa_questao_secao_fk FOREIGN KEY (encaminhamento_naapa_secao_id) REFERENCES encaminhamento_naapa_secao(id);

-- Resposta Encaminhamento NAAPA
CREATE table public.encaminhamento_naapa_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_encaminhamento_id int8 not null,
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
	CONSTRAINT encaminhamento_naapa_resposta_pk PRIMARY KEY (id)
);

CREATE INDEX encaminhamento_naapa_resposta_questao_idx ON public.encaminhamento_naapa_resposta USING btree (questao_encaminhamento_id);
ALTER TABLE public.encaminhamento_naapa_resposta ADD CONSTRAINT encaminhamento_naapa_resposta_questao_fk FOREIGN KEY (questao_encaminhamento_id) REFERENCES encaminhamento_naapa_questao(id);

CREATE INDEX encaminhamento_naapa_resposta_resposta_idx ON public.encaminhamento_naapa_resposta USING btree (resposta_id);
ALTER TABLE public.encaminhamento_naapa_resposta ADD CONSTRAINT encaminhamento_naapa_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id);

CREATE INDEX encaminhamento_naapa_resposta_arquivo_idx ON public.encaminhamento_naapa_resposta USING btree (arquivo_id);
ALTER TABLE public.encaminhamento_naapa_resposta ADD CONSTRAINT encaminhamento_naapa_resposta_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
