
select  tcm.ano, tcm.codloc, tcm.cl_alu_codigo, tcm.modal, tcm.descserv
  	, tcm."Unidade_2022(código estadual)", tcm.serie_2022
  	, tcm.unidade_escolar_2022, tcm.dependência_escolar_2022 
  	, r.turma_nome
	, sum(r.total_aulas) as total_aulas
	, sum(r.total_ausencias) as total_ausencias
	, sum(r.total_compensacoes) as total_aulas
	, to_char(((sum(r.total_aulas) - sum(r.total_ausencias) - sum(r.total_compensacoes))::numeric / sum(r.total_aulas) * 100)::numeric, '999D00') as frequencia
  from tcm
  LEFT join relatorio_tcm r on r.codigo_aluno = tcm.cl_alu_codigo::varchar(15)
-- where tcm.cl_alu_codigo = 3030883
  group by tcm.ano, tcm.codloc, tcm.cl_alu_codigo, tcm.modal, tcm.descserv
  	, tcm."Unidade_2022(código estadual)", tcm.serie_2022
  	, tcm.unidade_escolar_2022, tcm.dependência_escolar_2022, r.turma_nome


select t.codigo_aluno, count(distinct t.turma_id) 
  from relatorio_tcm t
  inner join turma tr on tr.turma_id = t.turma_id and tr.tipo_turma = 1
group by t.codigo_aluno
having count(distinct t.turma_id) > 1

select *
from relatorio_tcm t
  inner join turma tr on tr.turma_id = t.turma_id and tr.tipo_turma = 1
where t.codigo_aluno in ('3030883')