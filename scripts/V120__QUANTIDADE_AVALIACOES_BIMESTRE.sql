ALTER TABLE if exists public.tipo_avaliacao ADD if not exists avaliacoes_necessarias_bimestre int4 NOT NULL DEFAULT 0;

update tipo_avaliacao set avaliacoes_necessarias_bimestre = 1 where descricao='Avaliação bimestral'