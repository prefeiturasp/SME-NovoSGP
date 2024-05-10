CREATE TABLE public.conselho_classe (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_id int8 not null,
	
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT conselho_classe_pk PRIMARY KEY (id)
);
ALTER TABLE public.conselho_classe ADD CONSTRAINT conselho_classe_fechamento_fk FOREIGN KEY (fechamento_turma_id) REFERENCES fechamento_turma(id);
CREATE INDEX conselho_classe_fechamento_idx ON public.conselho_classe USING btree (fechamento_turma_id);


CREATE TABLE public.conselho_classe_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	conselho_classe_id int8 not null,
	aluno_codigo varchar(15) not null,
	recomendacoes_aluno varchar null,
	recomendacoes_familia varchar null,
	anotacoes_pedagogicas varchar null,
	
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT conselho_classe_aluno_pk PRIMARY KEY (id)
);
ALTER TABLE public.conselho_classe_aluno ADD CONSTRAINT conselho_classe_aluno_conselho_fk FOREIGN KEY (conselho_classe_id) REFERENCES conselho_classe(id);
CREATE INDEX conselho_classe_aluno_conselho_idx ON public.conselho_classe_aluno USING btree (conselho_classe_id);


CREATE TABLE public.conselho_classe_nota (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	conselho_classe_aluno_id int8 not null,
	componente_curricular_codigo int8 not null,
	nota numeric(5,2) null,
	conceito_id int8 null,
	justificativa varchar not null,
	
	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT conselho_classe_nota_pk PRIMARY KEY (id)
);
ALTER TABLE public.conselho_classe_nota ADD CONSTRAINT conselho_classe_nota_aluno_fk FOREIGN KEY (conselho_classe_aluno_id) REFERENCES conselho_classe_aluno(id);
CREATE INDEX conselho_classe_nota_aluno_idx ON public.conselho_classe_nota USING btree (conselho_classe_aluno_id);
ALTER TABLE public.conselho_classe_nota ADD CONSTRAINT conselho_classe_nota_conceito_fk FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id);
