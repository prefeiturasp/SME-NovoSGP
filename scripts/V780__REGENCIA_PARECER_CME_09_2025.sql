-- fix: 147048 alteracao-regencia REG CLASSE PARECER CME

INSERT INTO public.componente_curricular
    (id, componente_curricular_pai_id, grupo_matriz_id, area_conhecimento_id, descricao,
     eh_regencia, eh_compartilhada, eh_territorio, eh_base_nacional,
     permite_registro_frequencia, permite_lancamento_nota)
VALUES
    (1875, NULL, 1, NULL, 'REG CLASSE PARECER CME Nº 09/2025',
     true, false, false, false, true, true)
ON CONFLICT (id) DO UPDATE SET eh_regencia = true;
