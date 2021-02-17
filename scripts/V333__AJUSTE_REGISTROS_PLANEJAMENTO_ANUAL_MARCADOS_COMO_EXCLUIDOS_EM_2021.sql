do $$

declare lista_vinculos_plano_anual_excluido record;

begin
	for lista_vinculos_plano_anual_excluido in
		select pa.id planejamento_anual_id,
			   pape.id planejamento_anual_periodo_escolar_id,
			   pac.id planejamento_anual_componente_id,
			   paoa.id planejamento_anual_objetivos_aprendizagem_id
			from planejamento_anual pa 
				inner join turma t
					on pa.turma_id = t.id
				inner join planejamento_anual_periodo_escolar pape
					on pa.id = pape.planejamento_anual_id 
				inner join planejamento_anual_componente pac
					on pape.id = pac.planejamento_anual_periodo_escolar_id
				inner join planejamento_anual_objetivos_aprendizagem paoa
					on pac.id = paoa.planejamento_anual_componente_id 				
		where t.ano_letivo = 2021 and
			  pa.excluido and
			  not pape.excluido and
			  not pac.excluido and
			  not paoa.excluido
			  
	loop
	
		update planejamento_anual_periodo_escolar
		set excluido = true
		where id = lista_vinculos_plano_anual_excluido.planejamento_anual_periodo_escolar_id;
	
		update planejamento_anual_componente 
		set excluido = true
		where id = lista_vinculos_plano_anual_excluido.planejamento_anual_componente_id;
	
		update planejamento_anual_objetivos_aprendizagem 
		set excluido = true
		where id = lista_vinculos_plano_anual_excluido.planejamento_anual_objetivos_aprendizagem_id;
	
	end loop;
end $$;