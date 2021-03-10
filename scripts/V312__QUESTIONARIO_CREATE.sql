DROP TABLE if exists public.opcao_resposta;
DROP TABLE if exists public.questao;
DROP TABLE if exists public.questionario;

CREATE table public.questionario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(200) not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT questionario_pk PRIMARY KEY (id)
);


CREATE table public.questao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questionario_id int8 not null,

	ordem int4 not null,
	nome varchar(200) not null,
	observacao varchar(200) null,
	obrigatorio bool not null default false,
	tipo int4 not null,
	opcionais varchar(100) null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT questao_pk PRIMARY KEY (id)
);

CREATE INDEX questao_questionario_idx ON public.questao USING btree (questionario_id);
ALTER TABLE public.questao ADD CONSTRAINT questao_questionario_fk FOREIGN KEY (questionario_id) REFERENCES questionario(id);


CREATE table public.opcao_resposta (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	questao_id int8 not null,
	questao_complementar_id int8 null,

	ordem int4 not null,
	nome varchar(200) not null,
	
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT opcao_resposta_pk PRIMARY KEY (id)
);

CREATE INDEX opcao_resposta_questao_idx ON public.opcao_resposta USING btree (questao_id);
ALTER TABLE public.opcao_resposta ADD CONSTRAINT opcao_resposta_questao_fk FOREIGN KEY (questao_id) REFERENCES questao(id);

CREATE INDEX opcao_resposta_questao_complementar_idx ON public.opcao_resposta USING btree (questao_complementar_id);
ALTER TABLE public.opcao_resposta ADD CONSTRAINT opcao_resposta_questao_complementar_fk FOREIGN KEY (questao_complementar_id) REFERENCES questao(id);
