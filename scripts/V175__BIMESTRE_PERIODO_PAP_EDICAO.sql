ALTER TABLE public.recuperacao_paralela_periodo add if not exists bimestre_edicao int4 NOT NULL DEFAULT 0;

UPDATE public.recuperacao_paralela_periodo 	SET bimestre_edicao = 2 ,alterado_por='Sistema',alterado_em = now(),alterado_rf='0' WHERE nome = 'Acompanhamento 1º Semestre';
	
UPDATE public.recuperacao_paralela_periodo 	SET bimestre_edicao = 4 ,alterado_por='Sistema',alterado_em = now(),alterado_rf='0' WHERE nome = 'Acompanhamento 2º Semestre';
