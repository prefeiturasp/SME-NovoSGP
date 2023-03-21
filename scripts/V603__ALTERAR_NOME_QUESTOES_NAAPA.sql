UPDATE public.questao
	SET nome_componente='FILIACAO_1',nome='Filiação 1'
	WHERE nome='Nome da mãe';

UPDATE public.questao
	SET nome_componente='ESTUDANTE_MIGRANTE',nome='Criança/Estudante é migrante (autodenominação)'
	WHERE nome='Criança/Estudante é imigrante (autodenominação)';

UPDATE public.questao
	SET nome_componente='RESPONSAVEL_MIGRANTE',nome='Responsável/Cuidador é migrante'
	WHERE nome='Responsável/Cuidador é imigrante';