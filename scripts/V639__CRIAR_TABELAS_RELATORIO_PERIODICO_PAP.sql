CREATE table IF NOT EXISTS public.relatorio_periodico_pap_turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	periodo_relatorio_pap_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT relatorio_periodico_pap_turma_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists relatorio_periodico_pap_turma_idx ON public.relatorio_periodico_pap_turma USING btree (turma_id);
ALTER TABLE public.relatorio_periodico_pap_turma DROP CONSTRAINT if exists relatorio_periodico_pap_turma_fk;
ALTER TABLE public.relatorio_periodico_pap_turma ADD CONSTRAINT relatorio_periodico_pap_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX if not exists relatorio_periodico_pap_turma_periodo_pap_idx ON public.relatorio_periodico_pap_turma USING btree (periodo_relatorio_pap_id);
ALTER TABLE public.relatorio_periodico_pap_turma DROP CONSTRAINT if exists relatorio_periodico_pap_turma_periodo_pap_turma_fk;
ALTER TABLE public.relatorio_periodico_pap_turma ADD CONSTRAINT relatorio_periodico_pap_turma_periodo_pap_turma_fk FOREIGN KEY (periodo_relatorio_pap_id) REFERENCES periodo_relatorio_pap(id);


CREATE table IF NOT EXISTS public.relatorio_periodico_pap_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aluno_codigo varchar(15) not null,
	aluno_nome varchar not null,
	relatorio_periodico_pap_turma_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT relatorio_periodico_pap_aluno_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists  relatorio_periodico_pap_aluno_idx ON public.relatorio_periodico_pap_aluno USING btree (aluno_codigo);

CREATE INDEX if not exists  relatorio_periodico_pap_aluno_relatorio_turma_idx ON public.relatorio_periodico_pap_aluno USING btree (relatorio_periodico_pap_turma_id);
ALTER TABLE public.relatorio_periodico_pap_aluno DROP CONSTRAINT if exists relatorio_periodico_pap_aluno_relatorio_turma_fk;
ALTER TABLE public.relatorio_periodico_pap_aluno ADD CONSTRAINT relatorio_periodico_pap_aluno_relatorio_turma_fk FOREIGN KEY (relatorio_periodico_pap_turma_id) REFERENCES relatorio_periodico_pap_turma(id);


CREATE table IF NOT EXISTS public.relatorio_periodico_pap_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	relatorio_periodico_pap_aluno_id int8 not null,
	secao_relatorio_periodico_pap_id int8 not null,
	concluido bool not null default false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT relatorio_periodico_pap_secao_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists relatorio_periodico_pap_secao_relatorio_aluno_idx ON public.relatorio_periodico_pap_secao USING btree (relatorio_periodico_pap_aluno_id);
ALTER TABLE public.relatorio_periodico_pap_secao DROP CONSTRAINT if exists relatorio_periodico_pap_secao_relatorio_aluno_fk;
ALTER TABLE public.relatorio_periodico_pap_secao ADD CONSTRAINT relatorio_periodico_pap_secao_relatorio_aluno_fk FOREIGN KEY (relatorio_periodico_pap_aluno_id) REFERENCES relatorio_periodico_pap_aluno(id);

CREATE INDEX if not exists relatorio_periodico_pap_secao_secao_idx ON public.relatorio_periodico_pap_secao USING btree (secao_relatorio_periodico_pap_id);
ALTER TABLE public.relatorio_periodico_pap_secao DROP CONSTRAINT if exists relatorio_periodico_pap_secao_secao_fk;
ALTER TABLE public.relatorio_periodico_pap_secao ADD CONSTRAINT relatorio_periodico_pap_secao_secao_fk FOREIGN KEY (secao_relatorio_periodico_pap_id) REFERENCES secao_relatorio_periodico_pap(id);

CREATE table IF NOT EXISTS public.relatorio_periodico_pap_questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	relatorio_periodico_pap_secao_id int8 not null,
	questao_id int8 not null,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT relatorio_periodico_pap_questao_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists  relatorio_periodico_pap_questao_questao_idx ON public.relatorio_periodico_pap_questao USING btree (questao_id);
ALTER TABLE public.relatorio_periodico_pap_questao DROP CONSTRAINT if exists relatorio_periodico_pap_questao_questao_fk;
ALTER TABLE public.relatorio_periodico_pap_questao ADD CONSTRAINT relatorio_periodico_pap_questao_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);

CREATE INDEX if not exists relatorio_periodico_pap_questao_secao_idx ON public.relatorio_periodico_pap_questao USING btree (relatorio_periodico_pap_secao_id);
ALTER TABLE public.relatorio_periodico_pap_questao DROP CONSTRAINT if exists relatorio_periodico_pap_questao_secao_fk;
ALTER TABLE public.relatorio_periodico_pap_questao ADD CONSTRAINT relatorio_periodico_pap_questao_secao_fk FOREIGN KEY (relatorio_periodico_pap_secao_id) REFERENCES relatorio_periodico_pap_secao(id);


CREATE table IF NOT EXISTS public.relatorio_periodico_pap_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	relatorio_periodico_pap_questao_id int8 not null,
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
	CONSTRAINT relatorio_periodico_pap_resposta_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists  relatorio_periodico_pap_resposta_questao_idx ON public.relatorio_periodico_pap_resposta USING btree (relatorio_periodico_pap_questao_id);
ALTER TABLE public.relatorio_periodico_pap_resposta DROP CONSTRAINT if exists relatorio_periodico_pap_resposta_questao_fk;
ALTER TABLE public.relatorio_periodico_pap_resposta ADD CONSTRAINT relatorio_periodico_pap_resposta_questao_fk FOREIGN KEY (relatorio_periodico_pap_questao_id) REFERENCES relatorio_periodico_pap_questao(id);

CREATE INDEX if not exists relatorio_periodico_pap_resposta_resposta_idx ON public.relatorio_periodico_pap_resposta USING btree (resposta_id);
ALTER TABLE public.relatorio_periodico_pap_resposta DROP CONSTRAINT if exists relatorio_periodico_pap_resposta_resposta_fk;
ALTER TABLE public.relatorio_periodico_pap_resposta ADD CONSTRAINT relatorio_periodico_pap_resposta_resposta_fk FOREIGN KEY (resposta_id) REFERENCES opcao_resposta(id);

CREATE INDEX if not exists  relatorio_periodico_pap_resposta_arquivo_idx ON public.relatorio_periodico_pap_resposta USING btree (arquivo_id);
ALTER TABLE public.relatorio_periodico_pap_resposta DROP CONSTRAINT if exists relatorio_periodico_pap_resposta_arquivo_fk;
ALTER TABLE public.relatorio_periodico_pap_resposta ADD CONSTRAINT relatorio_periodico_pap_resposta_arquivo_fk FOREIGN KEY (arquivo_id) REFERENCES arquivo(id);
