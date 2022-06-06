with turmas as (
	select t.id as turma_id, t.nome as turma_nome, t.ano as turma_ano, t.ano_letivo , t.turma_id as turma_codigo
	  from turma t 
	 where t.ano_letivo = 2015
	 
	   and t.ano in ('1', '2', '3', '4', '5')
	   and t.modalidade_codigo = 5
)

select t.turma_codigo, t.turma_nome, t.turma_ano, t.ano_letivo
	, rb.aluno_codigo 
	, max(coalesce(cvc.descricao, cvf.descricao)) filter (where rb.componente_curricular_id = 138) as "Conceito Lingua Portuguesa"
	, to_char(max(coalesce(rb.nota_conselho, rb.nota_fechamento)) filter (where rb.componente_curricular_id = 138), '99D0') as "Nota Lingua Portuguesa"
	, sum(total_aulas) as total_aulas
	, sum(total_ausencias) as total_ausencias
	, sum(total_compensacoes) as total_compensacoes
	, to_char(((sum(total_aulas) - sum(total_ausencias) - sum(total_compensacoes))::numeric / sum(total_aulas) * 100)::numeric, '999D00') as frequencia
  from turmas t
  left join relatorio_bid rb on rb.turma_id = t.turma_id
  left join conceito_valores cvf on cvf.id = rb.conceito_fechamento_id 
  left join conceito_valores cvc on cvc.id = rb.conceito_conselho_id 
  left join relatorio_bid_frequencia rbf on rbf.turma_id = t.turma_id and rbf.aluno_codigo = rb.aluno_codigo 
group by t.turma_codigo, t.turma_nome, t.turma_ano, t.ano_letivo, rb.aluno_codigo 

