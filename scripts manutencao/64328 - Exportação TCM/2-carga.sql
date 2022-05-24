with frequencias as (
select fa.codigo_aluno
	, fa.turma_id
	, t.nome as turma_nome
	, fa.bimestre
	, fa.total_aulas 
	, fa.total_ausencias 
	, fa.total_compensacoes 
	, row_number() over (
		partition by fa.codigo_aluno
				, fa.turma_id
				, fa.bimestre order by fa.id desc) as ordem
  from tcm
inner join frequencia_aluno fa on fa.codigo_aluno = tcm.cl_alu_codigo::varchar(15) 
inner join turma t on t.turma_id = fa.turma_id and t.ano_letivo = 2022 and t.tipo_turma = 1
)
insert into relatorio_tcm (
	  turma_id
	, turma_nome
	, codigo_aluno
	, bimestre
	, total_aulas
	, total_ausencias
	, total_compensacoes)
  select distinct turma_id
	, fa.turma_nome
  	, fa.codigo_aluno
	, fa.bimestre
  	, fa.total_aulas
  	, fa.total_ausencias
  	, fa.total_compensacoes 
  from frequencias fa
  where ordem = 1

	on conflict (turma_id, codigo_aluno, bimestre)
	do UPDATE SET 
	     total_aulas = excluded.total_aulas
	    ,total_ausencias = excluded.total_ausencias
	    ,total_compensacoes = excluded.total_compensacoes;
