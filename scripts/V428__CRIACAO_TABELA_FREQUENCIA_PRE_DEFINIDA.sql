CREATE TABLE
IF NOT EXISTS public.frequencia_pre_definida
(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	
	componente_curricular_id int8 NOT NULL,	
	turma_id int8 NOT NULL,
    codigo_aluno varchar
(15) NOT NULL,
	tipo_frequencia int4 NOT NULL,
	CONSTRAINT frequencia_pre_definida_pk PRIMARY KEY
(id)
);

CREATE INDEX
if not exists frequencia_pre_definida_turma_id_ix ON public.frequencia_pre_definida USING btree
(turma_id);
CREATE INDEX
if not exists frequencia_pre_definida_componente_curricular_id_ix ON public.frequencia_pre_definida USING btree
(componente_curricular_id);
CREATE INDEX
if not exists frequencia_pre_definida_codigo_aluno_idx ON public.frequencia_pre_definida USING btree
(codigo_aluno);


ALTER TABLE public.frequencia_pre_definida ADD CONSTRAINT frequencia_pre_definida_turma_fk FOREIGN KEY (turma_id) REFERENCES public.turma(id);	