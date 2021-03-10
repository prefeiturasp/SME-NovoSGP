drop table if exists resposta_encaminhamento_aee;
drop table if exists questao_encaminhamento_aee;
drop table if exists encaminhamento_aee_secao;
drop table if exists encaminhamento_aee;
drop table if exists secao_encaminhamento_aee;

-- SECAO ENCAMINHAMENTO AEE
CREATE table public.secao_encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questionario_id int8 not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT secao_encaminhamento_aee_pk PRIMARY KEY (id)
);

CREATE INDEX secao_encaminhamento_aee_questionario_idx ON public.secao_encaminhamento_aee USING btree (questionario_id);
ALTER TABLE public.secao_encaminhamento_aee ADD CONSTRAINT secao_encaminhamento_aee_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id);


-- ENCAMINHAMENTO AEE
CREATE table public.encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	aluno_codigo varchar(15) not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_aee_pk PRIMARY KEY (id)
);

CREATE INDEX encaminhamento_aee_turma_idx ON public.encaminhamento_aee USING btree (turma_id);
ALTER TABLE public.encaminhamento_aee ADD CONSTRAINT encaminhamento_aee_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX encaminhamento_aee_aluno_idx ON public.encaminhamento_aee USING btree (aluno_codigo);


-- ENCAMINHAMENTO_AEE_SECAO
CREATE table public.encaminhamento_aee_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_aee_id int8 not null,
	secao_encaminhamento_id int8 not null,
	concluido bool not null default false,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT encaminhamento_aee_secao_pk PRIMARY KEY (id)
);

CREATE INDEX encaminhamento_aee_secao_encaminhamento_idx ON public.encaminhamento_aee_secao USING btree (encaminhamento_aee_id);
ALTER TABLE public.encaminhamento_aee_secao ADD CONSTRAINT encaminhamento_aee_secao_encaminhamento_fk FOREIGN KEY (encaminhamento_aee_id) REFERENCES encaminhamento_aee(id);

CREATE INDEX encaminhamento_aee_secao_secao_idx ON public.encaminhamento_aee_secao USING btree (secao_encaminhamento_id);
ALTER TABLE public.encaminhamento_aee_secao ADD CONSTRAINT encaminhamento_aee_secao_secao_fk FOREIGN KEY (secao_encaminhamento_id) REFERENCES secao_encaminhamento_aee(id);


-- QUESTAO_ENCAMINHAMENTO_AEE
CREATE table public.questao_encaminhamento_aee (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_aee_secao_id int8 not null,
	questao_id int8 not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT questao_encaminhamento_aee_pk PRIMARY KEY (id)
);

CREATE INDEX questao_encaminhamento_aee_secao_idx ON public.questao_encaminhamento_aee USING btree (encaminhamento_aee_secao_id);
ALTER TABLE public.questao_encaminhamento_aee ADD CONSTRAINT questao_encaminhamento_aee_encaminhamento_fk FOREIGN KEY (encaminhamento_aee_secao_id) REFERENCES encaminhamento_aee_secao(id);

CREATE INDEX questao_encaminhamento_aee_questao_idx ON public.questao_encaminhamento_aee USING btree (questao_id);
ALTER TABLE public.questao_encaminhamento_aee ADD CONSTRAINT questao_encaminhamento_aee_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);


-- REPOSTA_ENCAMINHAMENTO_AEE
CREATE table public.resposta_encaminhamento_aee (
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
	CONSTRAINT resposta_encaminhamento_aee_pk PRIMARY KEY (id)
);

CREATE INDEX resposta_encaminhamento_aee_questao_idx ON public.resposta_encaminhamento_aee USING btree (questao_encaminhamento_id);
ALTER TABLE public.resposta_encaminhamento_aee ADD CONSTRAINT resposta_encaminhamento_aee_questao_fk FOREIGN KEY (questao_encaminhamento_id) REFERENCES questao_encaminhamento_aee(id);

CREATE INDEX resposta_encaminhamento_aee_resposta_idx ON public.resposta_encaminhamento_aee USING btree (resposta_id);
ALTER TABLE public.resposta_encaminhamento_aee ADD CONSTRAINT resposta_encaminhamento_aee_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id);

CREATE INDEX resposta_encaminhamento_aee_arquivo_idx ON public.resposta_encaminhamento_aee USING btree (arquivo_id);
ALTER TABLE public.resposta_encaminhamento_aee ADD CONSTRAINT resposta_encaminhamento_aee_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
