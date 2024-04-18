drop table if exists mapeamento_estudante_resposta;
drop table if exists mapeamento_estudante_questao;
drop table if exists mapeamento_estudante_secao;
drop table if exists mapeamento_estudante;
drop table if exists secao_mapeamento_estudante;

-- SECAO Mapeamento de Estudante
CREATE table public.secao_mapeamento_estudante (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questionario_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	nome varchar,
	ordem int4,
	etapa int4,
	nome_componente varchar(50),
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT secao_mapeamento_estudante_pk PRIMARY KEY (id)
);

CREATE INDEX secao_mapeamento_estudante_questionario_idx ON public.secao_mapeamento_estudante USING btree (questionario_id);
ALTER TABLE public.secao_mapeamento_estudante ADD CONSTRAINT secao_mapeamento_estudante_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id);


-- Mapeamento de Estudante
CREATE table public.mapeamento_estudante (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	aluno_codigo varchar(15) not null,
	aluno_nome varchar not null,
	bimestre int4,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT mapeamento_estudante_pk PRIMARY KEY (id)
);

CREATE INDEX mapeamento_estudante_turma_idx ON public.mapeamento_estudante USING btree (turma_id);
ALTER TABLE public.mapeamento_estudante ADD CONSTRAINT mapeamento_estudante_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX mapeamento_estudante_aluno_idx ON public.mapeamento_estudante USING btree (aluno_codigo);

-- mapeamento_estudante_SECAO
CREATE table public.mapeamento_estudante_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	mapeamento_estudante_id int8 not null,
	secao_mapeamento_estudante_id int8 not null,
	concluido bool not null default false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT mapeamento_estudante_secao_pk PRIMARY KEY (id)
);

CREATE INDEX mapeamento_estudante_secao_mapeamento_estudante_idx ON public.mapeamento_estudante_secao USING btree (mapeamento_estudante_id);
ALTER TABLE public.mapeamento_estudante_secao ADD CONSTRAINT mapeamento_estudante_secao_mapeamento_estudante_fk FOREIGN KEY (mapeamento_estudante_id) REFERENCES mapeamento_estudante(id);

CREATE INDEX mapeamento_estudante_secao_secao_idx ON public.mapeamento_estudante_secao USING btree (secao_mapeamento_estudante_id);
ALTER TABLE public.mapeamento_estudante_secao ADD CONSTRAINT mapeamento_estudante_secao_secao_fk FOREIGN KEY (secao_mapeamento_estudante_id) REFERENCES secao_mapeamento_estudante(id);


-- Questão Mapeamento de Estudante
CREATE table public.mapeamento_estudante_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	mapeamento_estudante_secao_id int8 not null,
	questao_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT mapeamento_estudante_questao_pk PRIMARY KEY (id)
);

CREATE INDEX mapeamento_estudante_questao_questao_idx ON public.mapeamento_estudante_questao USING btree (questao_id);
ALTER TABLE public.mapeamento_estudante_questao ADD CONSTRAINT mapeamento_estudante_questao_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);

CREATE INDEX mapeamento_estudante_questao_secao_idx ON public.mapeamento_estudante_questao USING btree (mapeamento_estudante_secao_id);
ALTER TABLE public.mapeamento_estudante_questao ADD CONSTRAINT mapeamento_estudante_questao_secao_fk FOREIGN KEY (mapeamento_estudante_secao_id) REFERENCES mapeamento_estudante_secao(id);

-- Resposta Mapeamento de Estudante
CREATE table public.mapeamento_estudante_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_mapeamento_estudante_id int8 not null,
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
	CONSTRAINT mapeamento_estudante_resposta_pk PRIMARY KEY (id)
);

CREATE INDEX mapeamento_estudante_resposta_questao_idx ON public.mapeamento_estudante_resposta USING btree (questao_mapeamento_estudante_id);
ALTER TABLE public.mapeamento_estudante_resposta ADD CONSTRAINT mapeamento_estudante_resposta_questao_fk FOREIGN KEY (questao_mapeamento_estudante_id) REFERENCES mapeamento_estudante_questao(id);

CREATE INDEX mapeamento_estudante_resposta_resposta_idx ON public.mapeamento_estudante_resposta USING btree (resposta_id);
ALTER TABLE public.mapeamento_estudante_resposta ADD CONSTRAINT mapeamento_estudante_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id);

CREATE INDEX mapeamento_estudante_resposta_arquivo_idx ON public.mapeamento_estudante_resposta USING btree (arquivo_id);
ALTER TABLE public.mapeamento_estudante_resposta ADD CONSTRAINT mapeamento_estudante_resposta_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
