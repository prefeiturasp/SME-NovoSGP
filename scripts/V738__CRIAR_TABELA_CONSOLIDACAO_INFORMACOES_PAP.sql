CREATE TABLE IF NOT EXISTS consolidacao_informacoes_pap (
    id SERIAL PRIMARY KEY,
    tipo_pap VARCHAR(30) NOT NULL,
	dre_codigo VARCHAR(15) NOT NULL,
	ue_codigo VARCHAR(15) NOT NULL,
	dre_nome VARCHAR(100) NOT NULL,
	ue_nome VARCHAR(200) NOT NULL,
    quantidade_turmas INT NOT NULL,
    quantidade_estudantes INT NOT NULL,
    quantidade_estudantes_com_frequencia_inferior_limite INT NOT NULL,
    quantidade_estudantes_dificuldade_top_1 INT NOT NULL,
    quantidade_estudantes_dificuldade_top_2 INT NOT NULL,
    nome_dificuldade_top_1 VARCHAR(200) NOT NULL,
    nome_dificuldade_top_2 VARCHAR(200) NOT NULL,
    outras_dificuldades_aprendizagem INT NOT NULL
);