update objetivo_aprendizagem_aula
	set componente_curricular_id = pc.componente_curricular_id
  from objetivo_aprendizagem_aula oa 
 inner join plano_aula pa on oa.plano_aula_id = pa.id
 inner join aula a on a.id = pa.aula_id
 inner join turma t on t.turma_id = a.turma_id
 inner join planejamento_anual pla on pla.turma_id = t.id and pla.componente_curricular_id = a.disciplina_id::bigint
 inner join planejamento_anual_periodo_escolar pe on pe.planejamento_anual_id = pla.id
 inner join periodo_escolar p on p.id = pe.periodo_escolar_id and a.data_aula between p.periodo_inicio and p.periodo_fim
 inner join planejamento_anual_componente pc on pc.planejamento_anual_periodo_escolar_id = pe.id
 inner join planejamento_anual_objetivos_aprendizagem po on po.planejamento_anual_componente_id = pc.id and po.objetivo_aprendizagem_id = oa.objetivo_aprendizagem_id
 where objetivo_aprendizagem_aula.componente_curricular_id is null
   and objetivo_aprendizagem_aula.id = oa.id

