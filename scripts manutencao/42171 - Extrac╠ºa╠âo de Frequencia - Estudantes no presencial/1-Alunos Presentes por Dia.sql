select AbreviacaoDre, TipoEscola, NomeEscola, CodigoEscola, CodigoTurma, NomeTurma, DataAula
	, count(CodigoAluno) filter (where presente)
from (
	select dre.abreviacao as AbreviacaoDre
		, ue.tipo_escola as TipoEscola
		, ue.nome as NomeEscola
		, ue.ue_id as CodigoEscola
		, t.turma_id as CodigoTurma
		, t.nome as NomeTurma
		, a.data_aula as DataAula
		, rfa.codigo_aluno as CodigoAluno
		, (count(rfa.id) filter (where rfa.valor = 1)) > 0 as presente
	  from turma t
	 inner join ue on ue.id = t.ue_id 
	 inner join dre on dre.id = ue.dre_id 
	 inner join aula a on a.turma_id = t.turma_id 
	 inner join registro_frequencia rf on rf.aula_id = a.id and not rf.excluido 
	 inner join registro_frequencia_aluno rfa 
	  	on rfa.registro_frequencia_id = rf.id 
	  	and not rfa.excluido
	 where t.ano_letivo = 2021
	   and t.modalidade_codigo = 5
	   and dre.id = 2
	group by dre.abreviacao
		, ue.tipo_escola 
		, ue.nome 
		, ue.ue_id
		, t.turma_id 
		, t.nome
		, a.data_aula 
		, rfa.codigo_aluno 
) presencas
group by AbreviacaoDre, TipoEscola, NomeEscola, CodigoEscola, CodigoTurma, NomeTurma, DataAula