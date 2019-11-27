ALTER TABLE public.atividade_avaliativa ALTER COLUMN descricao_avaliacao DROP NOT NULL;
ALTER TABLE public.atividade_avaliativa DROP COLUMN IF EXISTS componente_curricular_id;
ALTER TABLE public.atividade_avaliativa ADD COLUMN disciplina_id varchar(15);