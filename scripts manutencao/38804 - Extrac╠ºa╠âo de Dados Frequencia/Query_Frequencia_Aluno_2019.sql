
select dre.dre_id, ue.ue_id, ue.nome
	, t.turma_id, t.nome
	, a.data_aula
	, count(distinct al.codigoaluno) as alunos
	, (count(distinct al.codigoaluno) * count(distinct a.id)) - count(distinct raa.id) as presencas
	, count(distinct raa.id) as ausencias
  from turma t  
 inner join ue on ue.id = t.ue_id 
 inner join dre on dre.id = ue.dre_id 
 inner join tmp_aluno_turma al on al.turmacodigo = t.turma_id
 inner join aula a on a.turma_id = t.turma_id 
 inner join registro_frequencia rf on rf.aula_id = a.id
  left join registro_ausencia_aluno raa on raa.registro_frequencia_id = rf.id
 where t.ano_letivo = 2019
   and t.modalidade_codigo in (1,3,5,6)
   and t.ano in ('1', '2', '3', '4', '5', '6', '7', '8', '9')
   and not a.excluido 
   and not rf.excluido 
group by dre.dre_id, ue.ue_id, ue.nome
	, t.turma_id, t.nome
	, a.data_aula
