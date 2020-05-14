ALTER TABLE public.evento_fechamento DROP CONSTRAINT if exists evento_fechamento_fechamento_fk;

select f_cria_fk_se_nao_existir('evento_fechamento', 'evento_fechamento_fk', 'FOREIGN KEY (fechamento_id) REFERENCES fechamento_bimestre (id)');

