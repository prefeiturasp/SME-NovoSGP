CREATE TABLE if NOT EXISTS public.atribuicao_cj (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	componente_curricular_id int8 NOT NULL,
	dre_id varchar(15) NOT NULL,
	ue_id varchar(15) NOT NULL,
    professor_rf varchar(10) NOT NULL,
    turma_id varchar(15) NOT NULL,
    modalidade int not null ,	
	substituir boolean not null,
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    CONSTRAINT atribuicao_cj_pk PRIMARY KEY (id)
);

CREATE INDEX atribuicao_cj_componente_cur_idx ON public.atribuicao_cj USING btree (componente_curricular_id);
CREATE INDEX atribuicao_cj_ue_id_idx ON public.atribuicao_cj USING btree (ue_id);
CREATE INDEX atribuicao_cj_professor_rf_idx ON public.atribuicao_cj USING btree (professor_rf);
CREATE INDEX atribuicao_cj_modalidade_idx ON public.atribuicao_cj USING btree (modalidade);
CREATE INDEX atribuicao_cj_turma_id_idx ON public.atribuicao_cj USING btree (turma_id);