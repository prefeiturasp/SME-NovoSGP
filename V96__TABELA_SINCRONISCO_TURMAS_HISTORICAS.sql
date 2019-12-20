CREATE TABLE public.sincronismo_turma_historica (
	ultimo_processamento date NOT NULL,
	CONSTRAINT sincronismo_turma_historica_pk PRIMARY KEY (ultimo_processamento)
);
