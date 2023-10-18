--> Adicionando campo semestre em tipo_calendario
ALTER TABLE public.tipo_calendario ADD column IF NOT exists SEMESTRE int4 NULL;

--> Definindo semestre conforme nome do tipo de calendário
update  tipo_calendario tc
	set semestre = case when nome like '%1º%' then 1 else 2 end
where tc.modalidade = 2 
      and not excluido 
	  and semestre is null;
	  
--> Inserindo parâmetro
INSERT INTO public.parametros_sistema
(id, nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
VALUES(1, 'CelpDiasLetivos', 1, 'Dias letivos minimo permitido para CELP', '100', 2023, true, now(), 'Sistema', NULL, NULL, 'Sistema', NULL);