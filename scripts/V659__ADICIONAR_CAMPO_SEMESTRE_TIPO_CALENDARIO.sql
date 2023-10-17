--> Adicionando campo semestre em tipo_calendario
ALTER TABLE public.tipo_calendario ADD column IF NOT exists SEMESTRE int4 NULL;

--> Definindo semestre conforme nome do tipo de calendário
update  tipo_calendario tc
	set semestre = case when nome like '%1º%' then 1 else 2 end
where tc.modalidade = 2 
      and not excluido 
	  and semestre is null;