-- DROP TABLE public.etl_sgp_fechamento_cc;

CREATE TABLE public.etl_sgp_fechamento_cc
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
	conceito_id int8 NULL,
	criado_em_cc timestamp NULL,
	alterado_em_cc timestamp NULL,
	recomendacoes_aluno varchar NULL,
	recomendacoes_familia varchar NULL,
	anotacoes_pedagocidas varchar NULL,
	notacc numeric(5,2) NULL,
	justificativa varchar NULL,
	parecer int8 NULL,
	serie int8 NULL,
	conceito_id_cc int8 NULL
);


-- DROP TABLE public.etl_sgp_fechamento_acerto;

CREATE TABLE public.etl_sgp_fechamento_acerto 
(
	id int8 NULL,
	disciplina_id int8 NULL,
	fechamento_turma_id int8 NULL
);


delete from conselho_classe_nota where migrado = true;
delete from conselho_classe_aluno where migrado = true;
delete from conselho_classe where migrado = true;

delete from fechamento_nota where migrado = true;
delete from fechamento_aluno where migrado = true;
delete from fechamento_turma_disciplina where migrado = true;
delete from fechamento_turma where migrado = true;

truncate table etl_sgp_fechamento_acerto;
truncate table etl_sgp_fechamento_cc;
truncate table etl_sgp_fechamento;

*/