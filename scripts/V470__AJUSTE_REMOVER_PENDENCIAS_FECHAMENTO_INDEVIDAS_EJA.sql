do $$
declare
	idsPendencias bigint[];

begin	  
	select array_agg(distinct (p.id)) into idsPendencias
  	  from pendencia_fechamento pf  
	 inner join pendencia p on p.id = pf.pendencia_id 
	 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id 
	 inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
	 inner join turma t on t.id = ft.turma_id 
	 where p.tipo = 4
	   and t.modalidade_codigo = 3
	   and ftd.disciplina_id in (1060,1061);

	if idsPendencias is not null then 
		delete from pendencia_fechamento where pendencia_id = ANY(idsPendencias);
		delete from pendencia_usuario where pendencia_id = ANY(idsPendencias);
 		delete from pendencia where id = ANY(idsPendencias);
 	end if;
end $$;