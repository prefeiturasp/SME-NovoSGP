CREATE TABLE IF NOT EXISTS public.painel_educacional_consolidacao_sondagem_escrita_ue (
    codigo_dre VARCHAR(15) NOT NULL,
    codigo_ue VARCHAR(15) NOT NULL,
    pre_silabico INT NOT NULL,
    silabico_sem_valor INT NOT NULL,
    silabico_com_valor INT NOT NULL,
    silabico_alfabetico INT NOT NULL,
    alfabetico INT NOT NULL,
    sem_preenchimento INT NOT NULL,
    ano_letivo INT NOT NULL,
    serie_ano INT NOT NULL,
    quantidade_aluno INT NOT NULL,
    bimestre INT NOT NULL
);
