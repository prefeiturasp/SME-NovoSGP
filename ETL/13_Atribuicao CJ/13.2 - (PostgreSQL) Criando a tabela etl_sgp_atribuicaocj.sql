
-- Drop table

-- DROP TABLE public.etl_sgp_atribuicaocj;

CREATE TABLE public.etl_sgp_atribuicaocj 
(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	disciplina_id int8 NOT NULL,
	dre_id varchar(15) NOT NULL,
	ue_id varchar(15) NOT NULL,
	professor_rf varchar(10) NOT NULL,
	turma_id varchar(15) NOT NULL,
	modalidade int4 NOT NULL,
	CONSTRAINT etl_sgp_atribuicaocj_pk PRIMARY KEY (id)
);

CREATE INDEX etl_sgp_atribuicaocj_disciplina_id_idx ON public.etl_sgp_atribuicaocj USING btree (disciplina_id);
CREATE INDEX etl_sgp_atribuicaocj_dre_id_idx ON public.etl_sgp_atribuicaocj USING btree (dre_id);
CREATE INDEX etl_sgp_atribuicaocj_modalidade_idx ON public.etl_sgp_atribuicaocj USING btree (modalidade);
CREATE INDEX etl_sgp_atribuicaocj_professor_rf_idx ON public.etl_sgp_atribuicaocj USING btree (professor_rf);
CREATE INDEX etl_sgp_atribuicaocj_turma_id_idx ON public.etl_sgp_atribuicaocj USING btree (turma_id);
CREATE INDEX etl_sgp_atribuicaocj_ue_id_idx ON public.etl_sgp_atribuicaocj USING btree (ue_id);
