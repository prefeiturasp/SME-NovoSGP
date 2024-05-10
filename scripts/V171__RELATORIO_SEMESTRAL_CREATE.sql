DROP TABLE IF EXISTS public.relatorio_semestral_pap_aluno_secao;
DROP TABLE IF EXISTS public.relatorio_semestral_pap_aluno; 
DROP TABLE IF EXISTS public.relatorio_semestral_turma_pap; 
DROP TABLE IF EXISTS public.secao_relatorio_semestral_pap;

CREATE TABLE public.secao_relatorio_semestral_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(50) NOT NULL,
	descricao varchar NOT NULL,
	obrigatorio bool NOT NULL DEFAULT FALSE,
	inicio_vigencia timestamp not null,
	fim_vigencia timestamp null,
	ordem int4 not null,
	
	CONSTRAINT secao_relatorio_semestral_pap_pk PRIMARY KEY (id)
);

insert into public.secao_relatorio_semestral_pap(nome, descricao, obrigatorio, inicio_vigencia, ordem) values 
  	('Histórico do Estudante', 'Trajetória do estudante, reprovações, histórico de faltas, acompanhamento das aprendizagens', true, '2014-01-01', 10),
  	('Dificuldades', 'Dificuldades apresentadas inicialmente ', true, '2014-01-01', 20),
  	('Encaminhamentos', 'Encaminhamentos realizados', true, '2014-01-01', 30),
  	('Avanços', 'Avanços observados', true, '2014-01-01', 40),
  	('Outros', 'Outras observações', false, '2014-01-01', 50);


CREATE TABLE public.relatorio_semestral_turma_pap (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	semestre int4 not null,

	CONSTRAINT relatorio_semestral_turma_pap_pk PRIMARY KEY (id)
);
CREATE INDEX relatorio_semestral_turma_pap_idx ON public.relatorio_semestral_turma_pap USING btree (turma_id);
ALTER TABLE public.relatorio_semestral_turma_pap ADD CONSTRAINT relatorio_semestral_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);


CREATE TABLE public.relatorio_semestral_pap_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	relatorio_semestral_turma_pap_id int8 not null,
	aluno_codigo varchar(15) not null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT relatorio_semestral_pap_aluno_pk PRIMARY KEY (id)
);

CREATE INDEX relatorio_semestral_pap_aluno_aluno_idx ON public.relatorio_semestral_pap_aluno USING btree (aluno_codigo);
CREATE INDEX relatorio_semestral_pap_aluno_relatorio_idx ON public.relatorio_semestral_pap_aluno USING btree (relatorio_semestral_turma_pap_id);
ALTER TABLE public.relatorio_semestral_pap_aluno ADD CONSTRAINT relatorio_semestral_pap_aluno_relatorio_fk FOREIGN KEY (relatorio_semestral_turma_pap_id) REFERENCES relatorio_semestral_turma_pap(id);


CREATE TABLE public.relatorio_semestral_pap_aluno_secao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	relatorio_semestral_pap_aluno_id int8 not null,
	secao_relatorio_semestral_pap_id int8 not null,
	valor varchar not null,
	CONSTRAINT relatorio_semestral_pap_aluno_secao_pk PRIMARY KEY (id)
);
CREATE INDEX relatorio_semestral_pap_aluno_secao_aluno_idx ON public.relatorio_semestral_pap_aluno_secao USING btree (relatorio_semestral_pap_aluno_id);
ALTER TABLE public.relatorio_semestral_pap_aluno_secao ADD CONSTRAINT relatorio_semestral_pap_aluno_secao_aluno_fk FOREIGN KEY (relatorio_semestral_pap_aluno_id) REFERENCES relatorio_semestral_pap_aluno(id);
CREATE INDEX relatorio_semestral_pap_aluno_secao_secao_idx ON public.relatorio_semestral_pap_aluno_secao USING btree (secao_relatorio_semestral_pap_id);
ALTER TABLE public.relatorio_semestral_pap_aluno_secao ADD CONSTRAINT relatorio_semestral_pap_aluno_secao_secao_fk FOREIGN KEY (secao_relatorio_semestral_pap_id) REFERENCES secao_relatorio_semestral_pap(id);
