CREATE TABLE IF NOT EXISTS public.proficiencia_ideb (
    id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
    codigo_escola VARCHAR(15) NOT NULL,
    serie int8 NOT NULL,
    componente_curricular VARCHAR(15) NOT NULL,
    proficiencia NUMERIC(11,2) NOT NULL,
    ano_letivo int8 NOT NULL,
    boletim VARCHAR(500),
    criado_em TIMESTAMP NOT NULL,
    criado_por VARCHAR(200) NOT NULL,
    alterado_em TIMESTAMP NOT NULL,
    alterado_por VARCHAR(200),
    criado_rf VARCHAR(200) NOT NULL,
    alterado_rf VARCHAR(200),
    CONSTRAINT proficiencia_ideb_pkey PRIMARY KEY (id)
);