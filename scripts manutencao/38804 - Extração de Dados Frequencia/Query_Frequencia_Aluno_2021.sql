select dre.dre_id, ue.ue_id, ue.nome
	, t.turma_id, t.nome
	, al.codigoaluno
	,(select sum(a.quantidade) 
	   from aula a 
	  inner join registro_frequencia rf on rf.aula_id = a.id
	  where not a.excluido 
	    and not rf.excluido 
	    and a.turma_id = t.turma_id 
	  ) as quantidadeRegistros
	  , fa.total_aulas, fa.total_ausencias, fa.total_compensacoes 
  from turma t  
 inner join ue on ue.id = t.ue_id 
 inner join dre on dre.id = ue.dre_id 
 inner join aluno_turma al on al.turmacodigo = t.turma_id
  left join frequencia_aluno fa on 
  	fa.codigo_aluno = al.codigoaluno
  	and fa.turma_id = al.turmacodigo
  	and fa.tipo = 2
 where t.ano_letivo = 2021
   and t.modalidade_codigo in (1,3,5,6)
   and t.ano in ('1', '2', '3', '4', '5', '6', '7', '8', '9')
   and exists (select 1 
	   from aula a 
	  inner join registro_frequencia rf on rf.aula_id = a.id
	  where not a.excluido 
	    and not rf.excluido 
	    and a.turma_id = t.turma_id 
	  ) 
