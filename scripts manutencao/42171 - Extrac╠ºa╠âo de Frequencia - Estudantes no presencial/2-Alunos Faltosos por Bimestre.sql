
select dre.abreviacao as AbreviacaoDre
	, ue.tipo_escola as TipoEscola
	, ue.nome as NomeEscola
	, ue.ue_id as CodigoEscola
	, t.turma_id as CodigoTurma
	, t.nome as NomeTurma
	, fa.codigo_aluno as CodigoAluno
	, fa.bimestre 
	, fa.total_ausencias as TotalFaltas
	, fa.total_compensacoes as TotalCompensacoes
	-- select count(*)
  from frequencia_aluno fa
 inner join turma t on t.turma_id = fa.turma_id 
 inner join ue on ue.id = t.ue_id 
 inner join dre on dre.id = ue.dre_id 
 where t.ano_letivo = 2021
   and t.modalidade_codigo = 5
   and fa.total_ausencias > 0
   and fa.tipo = 2
