CREATE INDEX registro_frequencia_aluno_rf_id_idx ON public.registro_frequencia_aluno USING btree (registro_frequencia_id);
ALTER TABLE turma ALTER COLUMN ano TYPE varchar(1);
CREATE INDEX turma_ano_idx ON public.turma USING btree (ano);