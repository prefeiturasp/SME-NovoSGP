select distinct 
	  dre.abreviacao as AbreviacaoDre
	, ue.tipo_escola as TipoEscola
	, ue.nome as NomeEscola
	, ue.ue_id as CodigoEscola
	, t.turma_id as CodigoTurma
	, t.nome as NomeTurma
	, rfa.codigo_aluno as CodigoAluno
	, pe.bimestre 
	, COALESCE(100-((cast((fa.total_ausencias - fa.total_compensacoes) as numeric)/fa.total_aulas)*100), 100) as PercentualFrequencia
  from turma t 
 inner join ue on ue.id = t.ue_id 
 inner join dre on dre.id = ue.dre_id 
 inner join aula a on a.turma_id = t.turma_id 
 inner join periodo_escolar pe 
 	on pe.tipo_calendario_id = a.tipo_calendario_id 
 	and a.data_aula between pe.periodo_inicio and pe.periodo_fim 
 inner join registro_frequencia rf on rf.aula_id = a.id 
 inner join registro_frequencia_aluno rfa on rfa.registro_frequencia_id = rf.id
  left join frequencia_aluno fa
  	on fa.turma_id = t.turma_id 
  	and fa.codigo_aluno = rfa.codigo_aluno 
  	and fa.periodo_escolar_id = pe.id
  	and fa.tipo = 2
 where t.ano_letivo = 2021
   and t.modalidade_codigo = 6
   and t.ano = '3'
order by dre.abreviacao, ue.nome, t.nome, rfa.codigo_aluno
