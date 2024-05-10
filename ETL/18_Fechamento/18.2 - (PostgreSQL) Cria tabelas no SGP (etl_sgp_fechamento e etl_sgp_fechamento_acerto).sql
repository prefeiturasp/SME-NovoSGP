-- DROP TABLE public.etl_sgp_fechamento;

CREATE TABLE public.etl_sgp_fechamento
(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	periodo_escolar_id int8 NULL,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	disciplina_id int8 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	nota numeric(5,2) NULL,
	sintese_id int8 NULL,
	conceito_id int8 NULL
);


-- DROP TABLE public.etl_sgp_fechamento_acerto;

CREATE TABLE public.etl_sgp_fechamento_acerto 
(
	id int8 NULL,
	disciplina_id int8 NULL,
	fechamento_turma_id int8 NULL
);
