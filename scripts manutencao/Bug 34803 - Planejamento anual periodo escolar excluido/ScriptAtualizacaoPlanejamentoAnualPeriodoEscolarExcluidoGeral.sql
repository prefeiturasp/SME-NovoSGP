select *
	from planejamento_anual pa
		inner join planejamento_anual_periodo_escolar pape
			on pa.id = pape.planejamento_anual_id
		inner join planejamento_anual_componente pac
			on pape.id = pac.planejamento_anual_periodo_escolar_id
where not pa.excluido and
	  not pac.excluido and
	  pape.excluido and
	  not exists (select 1
	 			  	from planejamento_anual_periodo_escolar pape2
	 			  where pape2.planejamento_anual_id = pa.id and
	 					not pape2.excluido and
	 					pape2.id <> pape.id);

begin transaction;	
--rollback;
--commit;
	 				
update planejamento_anual_periodo_escolar
set excluido = false
where id in (select distinct pape.id
				from planejamento_anual pa
					inner join planejamento_anual_periodo_escolar pape
						on pa.id = pape.planejamento_anual_id
					inner join planejamento_anual_componente pac
						on pape.id = pac.planejamento_anual_periodo_escolar_id
			 where not pa.excluido and
				   not pac.excluido and
				   pape.excluido and
				   not exists (select 1
				 			      from planejamento_anual_periodo_escolar pape2
				 			   where pape2.planejamento_anual_id = pa.id and
				 					 not pape2.excluido and
				 					 pape2.id <> pape.id));
 