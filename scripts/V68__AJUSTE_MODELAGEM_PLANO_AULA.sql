ALTER TABLE public.plano_aula ALTER COLUMN descricao TYPE varchar;
ALTER TABLE public.plano_aula ALTER COLUMN desenvolvimento_aula TYPE varchar;
ALTER TABLE public.plano_aula ALTER COLUMN recuperacao_aula TYPE varchar;
ALTER TABLE public.plano_aula ALTER COLUMN licao_casa TYPE varchar;

ALTER TABLE public.plano_aula ADD migrado bool NOT NULL DEFAULT false;
ALTER TABLE public.plano_aula ADD excluido bool NOT NULL DEFAULT false;

ALTER TABLE public.objetivo_aprendizagem_aula ADD excluido bool NOT NULL DEFAULT false;
ALTER TABLE public.registro_frequencia ADD excluido bool NOT NULL DEFAULT false;
