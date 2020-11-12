do $$
declare planoanual record;
		p_anual_periodo record;
		existe_periodo_escolar_id boolean;

begin

for planoanual in
	select pa.id,pa.escola_id,pa.turma_id,pa.ano,bimestre,pa.descricao,pa.criado_em,pa.criado_por,pa.alterado_em,
	pa.alterado_por,pa.criado_rf,pa.alterado_rf,pa.migrado,pa.componente_curricular_eol_id,pa.objetivos_opcionais
	from plano_anual pa
	left join objetivo_aprendizagem_plano o on o.plano_id = pa.id
	inner join componente_curricular cc on cc.id = pa.componente_curricular_eol_id::bigint
	where pa.ano = 2020	
	and o.id is null
loop
-- ---------------------------------
		for p_anual_periodo in
			select ppe.id,ppe.periodo_escolar_id,ppe.planejamento_anual_id,ppe.criado_em,ppe.criado_por,ppe.alterado_em,ppe.alterado_por,ppe.criado_rf,ppe.alterado_rf
			from planejamento_anual pa
			inner join turma t on t.id = pa.turma_id
			inner join planejamento_anual_periodo_escolar ppe on ppe.planejamento_anual_id = pa.id
			inner join periodo_escolar pe on pe.id = ppe.periodo_escolar_id
			where t.turma_id = planoanual.turma_id::varchar(15)
			and pe.bimestre = planoanual.bimestre			
		loop
		-- ----------------------
				select 1 into existe_periodo_escolar_id 
				from planejamento_anual_componente where planejamento_anual_periodo_escolar_id = p_anual_periodo.id;
				
				if not existe_periodo_escolar_id then
				    --delete from planejamento_anual_objetivos_aprendizagem where planejamento_anual_componente_id 
				    --in (select id from planejamento_anual_componente where planejamento_anual_periodo_escolar_id = p_anual_periodo.id);
				    --delete from planejamento_anual_componente where planejamento_anual_periodo_escolar_id = p_anual_periodo.id;
				    insert into planejamento_anual_componente (planejamento_anual_periodo_escolar_id, componente_curricular_id, descricao, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
					values (p_anual_periodo.id, planoanual.componente_curricular_eol_id, planoanual.descricao, planoanual.criado_em, planoanual.criado_por, planoanual.criado_rf, planoanual.alterado_em, planoanual.alterado_por, planoanual.alterado_rf);					
				end if;							
		-- ----------------------
		end loop;
-- ---------------------------------
end loop;

end $$;