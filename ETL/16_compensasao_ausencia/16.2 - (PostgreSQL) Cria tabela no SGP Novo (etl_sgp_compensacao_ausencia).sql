-- DROP TABLE public.etl_sgp_compensacao_ausencia;

CREATE TABLE public.etl_sgp_compensacao_ausencia 
(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	bimestre int4 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	turma_id int8 NOT NULL,
	nome varchar NOT NULL,
	descricao varchar NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	ano_letivo int4 NOT NULL,
	codigo_aluno varchar(100) NOT NULL,
	qtd_faltas_compensadas int4 NOT NULL,
	CONSTRAINT etl_sgp_compensacao_ausencia_pk PRIMARY KEY (id)
);