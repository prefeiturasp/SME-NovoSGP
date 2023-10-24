--> Adicionando campo semestre em tipo_calendario
ALTER TABLE public.tipo_calendario ADD column IF NOT exists SEMESTRE int4 NULL;

--> Definindo semestre conforme nome do tipo de calendário
update  tipo_calendario tc
	set semestre = case when nome like '% 1%' then 1 else 2 end
where tc.modalidade = 2 and not excluido;
	  
--> Inserindo parâmetro CelpDiasLetivos
INSERT INTO public.parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, criado_rf)
select 'CelpDiasLetivos', 109, 'Dias letivos minimo permitido para CELP', '100', 2023, true, now(), 'Sistema', 'Sistema' where not exists (select 1 from parametros_sistema where nome = 'CelpDiasLetivos')