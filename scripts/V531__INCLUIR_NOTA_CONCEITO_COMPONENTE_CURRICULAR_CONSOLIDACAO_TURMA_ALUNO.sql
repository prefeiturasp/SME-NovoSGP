DROP TABLE IF EXISTS public.consolidado_conselho_classe_aluno_turma_nota;

CREATE TABLE IF NOT EXISTS public.consolidado_conselho_classe_aluno_turma_nota (
id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
consolidado_conselho_classe_aluno_turma_id int8 NOT NULL,
bimestre int4 NULL,
nota numeric(11, 2) NULL,
conceito_id int8 NULL,
componente_curricular_id int8 NOT NULL,
CONSTRAINT consolidado_conselho_classe_aluno_turma_nota_pk PRIMARY KEY (id)
);

ALTER TABLE consolidado_conselho_classe_aluno_turma_nota DROP CONSTRAINT IF EXISTS consolidado_conselho_classe_aluno_turma_nota;
ALTER TABLE public.consolidado_conselho_classe_aluno_turma_nota 
ADD CONSTRAINT consolidado_conselho_classe_aluno_turma_nota 
FOREIGN KEY (consolidado_conselho_classe_aluno_turma_id) 
REFERENCES public.consolidado_conselho_classe_aluno_turma(id);

ALTER TABLE consolidado_conselho_classe_aluno_turma_nota DROP CONSTRAINT IF EXISTS consolidado_conselho_classe_aluno_turma_nota_componente_Curricular;
ALTER TABLE public.consolidado_conselho_classe_aluno_turma_nota 
ADD CONSTRAINT consolidado_conselho_classe_aluno_turma_nota_componente_Curricular 
FOREIGN KEY (componente_curricular_id) 
REFERENCES public.componente_curricular(id);

CREATE INDEX IF NOT EXISTS componente_curricular_idx ON public.consolidado_conselho_classe_aluno_turma_nota USING btree (componente_curricular_id);
CREATE INDEX IF NOT EXISTS consolidado_conselho_classe_aluno_turma_idx ON public.consolidado_conselho_classe_aluno_turma_nota USING btree (consolidado_conselho_classe_aluno_turma_id);


--> Removendo as linhas que são de bimestres e mantendo somente final
do $$
declare 
	temBimestre int4:=0;
begin	
	SELECT 1 into temBimestre FROM information_schema.columns WHERE table_name='consolidado_conselho_classe_aluno_turma' and column_name='bimestre';    

	if temBimestre = 1 then
		delete from consolidado_conselho_classe_aluno_turma where bimestre <> 0;
	end if;	
end $$;	

--> Removendo a coluna bimestre
alter table consolidado_conselho_classe_aluno_turma drop column if exists bimestre;