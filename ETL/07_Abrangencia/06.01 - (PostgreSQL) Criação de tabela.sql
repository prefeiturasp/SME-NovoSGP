-- ################################################################# --
--
-- Criação da tabela 'public.etl_abrangencia'
-- 
-- ################################################################# --

CREATE TABLE public.etl_abrangencia (
	usuario_rf varchar(7) NULL,
	dre_id_eol varchar(6) NULL,
	ue_id_eol varchar(6) NULL,
	turma_id_eol int4 NULL,
	dt_fim_vinculo date NULL
);
