--> Definindo semestre conforme nome do tipo de calendário
update  tipo_calendario tc
	set semestre = case when nome like '% 1%' then 1 else 2 end
where tc.modalidade = 2;
	  