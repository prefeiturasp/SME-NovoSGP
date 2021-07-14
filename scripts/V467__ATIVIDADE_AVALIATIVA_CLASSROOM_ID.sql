alter table atividade_avaliativa 
	add atividade_classroom_id int8 null;
	
CREATE INDEX atividade_avaliativa_classroom_idx ON public.atividade_avaliativa USING btree (atividade_classroom_id);