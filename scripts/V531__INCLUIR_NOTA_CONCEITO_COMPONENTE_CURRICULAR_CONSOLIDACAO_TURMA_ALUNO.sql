CREATE TABLE IF NOT EXISTS public.consolidado_conselho_classe_aluno_turma_nota (
id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
consolidadoConselhoClasseAlunoTurmaId int8,
bimestre int4 NOT NULL,
nota numeric(11, 2) NULL,
conceito_id int8 NULL,
componente_curricular_id int8 NULL,
CONSTRAINT consolidado_conselho_classe_aluno_turma_nota_pk PRIMARY KEY (id)
);



ALTER TABLE IF EXISTS public.consolidado_conselho_classe_aluno_turma_nota 
ADD CONSTRAINT consolidado_conselho_classe_aluno_turma_nota 
FOREIGN KEY (consolidadoConselhoClasseAlunoTurmaId) 
REFERENCES public.consolidado_conselho_classe_aluno_turma(id);