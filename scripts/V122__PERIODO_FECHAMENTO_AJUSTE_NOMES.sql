ALTER TABLE if exists public.fechamento_bimestre RENAME TO periodo_fechamento_bimestre;

ALTER TABLE if exists public.fechamento RENAME TO periodo_fechamento;

ALTER TABLE if exists public.periodo_fechamento_bimestre RENAME COLUMN fechamento_id TO periodo_fechamento_id;
