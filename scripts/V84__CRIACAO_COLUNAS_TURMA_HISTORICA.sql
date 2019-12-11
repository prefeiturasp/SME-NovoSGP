begin transaction;
drop table abrangencia_turmas;
drop table abrangencia_ues;
drop table abrangencia_dres;
ALTER TABLE public.turma ADD historica bool NOT NULL DEFAULT false;
ALTER TABLE public.turma ADD dt_fim_eol date NULL;
CREATE INDEX turma_historica_idx ON public.turma USING btree (historica, dt_fim_eol);
end transaction;