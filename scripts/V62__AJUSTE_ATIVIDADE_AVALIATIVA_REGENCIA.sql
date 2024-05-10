ALTER TABLE public.atividade_avaliativa ADD COLUMN eh_regencia boolean not null default false;
ALTER TABLE public.atividade_avaliativa ADD COLUMN disciplina_contida_regencia_id varchar(15) null;