CREATE TABLE IF NOT EXISTS consolidacao_informacoes_pap (
    id SERIAL PRIMARY KEY,
    tipo_pap VARCHAR(30) NOT NULL,
    quantidade_turmas INT NOT NULL,
    quantidade_estudantes INT NOT NULL,
    quantidade_estudantes_com_menos_75_por_cento_frequencia INT NOT NULL,
    dificuldade_aprendizagem_1 INT NOT NULL,
    dificuldade_aprendizagem_2 INT NOT NULL,
    outras_dificuldades_aprendizagem INT NOT NULL
);