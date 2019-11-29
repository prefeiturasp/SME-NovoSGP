begin transaction;
ALTER TABLE public.dre ADD data_atualizacao date NOT NULL;
ALTER TABLE public.ue ADD data_atualizacao date NOT NULL;
ALTER TABLE public.turma ADD data_atualizacao date NOT NULL;
end transaction;