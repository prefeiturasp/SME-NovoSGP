drop function if exists public.f_obternotasfechamentoouconselhoalunos;

CREATE OR REPLACE FUNCTION public.f_obternotasfechamentoouconselhoalunos(p_ano_letivo integer, p_dreid bigint DEFAULT 0, p_ueid bigint DEFAULT 0, p_modalidade_codigo integer DEFAULT 0, p_semestre integer DEFAULT 0, p_bimestre integer DEFAULT 0)
 RETURNS TABLE(turmaanonome character varying, ano character varying, bimestre integer, componentecurricularcodigo bigint, conselhoclassenotaid bigint, conceitoid bigint, nota numeric, alunocodigo character varying, conceito character varying, prioridade integer)
 LANGUAGE sql
AS $function$
set statement_timeout = '10min';
select * from (
	select t.nome as TurmaAnoNome,
	t.ano as Ano,
	pe.bimestre, 
	fn.disciplina_id as ComponenteCurricularCodigo, 
	null as ConselhoClasseNotaId, 
	fn.conceito_id as ConceitoId, 
	fn.nota as Nota, 
	fa.aluno_codigo as AlunoCodigo,
	cv.valor as Conceito,
	2 as prioridade
from
fechamento_turma ft
inner join periodo_escolar pe on
	pe.id = ft.periodo_escolar_id
inner join turma t on
	t.id = ft.turma_id
inner join ue on
	ue.id = t.ue_id
inner join fechamento_turma_disciplina ftd on
	ftd.fechamento_turma_id = ft.id
	and not ftd.excluido
inner join fechamento_aluno fa on
	fa.fechamento_turma_disciplina_id = ftd.id
	and not fa.excluido
inner join fechamento_nota fn on
	fn.fechamento_aluno_id = fa.id
	and not fn.excluido
inner join componente_curricular cc on cc.id = fn.disciplina_id
	and cc.permite_lancamento_nota 
left join conceito_valores cv on fn.conceito_id = cv.id
where t.ano_letivo = p_ano_letivo
	 and (p_ueid = 0 or (p_ueid <> 0 and ue.id = p_ueid))
	 and (p_dreid = 0 or (p_dreid <> 0 and ue.dre_id = p_dreid))
	 and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and t.modalidade_codigo = p_modalidade_codigo))
	 and (p_bimestre = 0 or (p_bimestre <> 0 and pe.bimestre = p_bimestre))
	 and (p_semestre = 0 or (p_semestre <> 0 and t.semestre = p_semestre))

union all

select t.nome as TurmaAnoNome,
	t.ano as Ano,
	pe.bimestre, 
	ccn.componente_curricular_codigo as ComponenteCurricularCodigo, 
	ccn.id as ConselhoClasseNotaId, 
	ccn.conceito_id as ConceitoId, 
	ccn.nota as Nota, 
	cca.aluno_codigo as AlunoCodigo,
	cv.valor as Conceito,
	1 as prioridade
from
fechamento_turma ft
left join periodo_escolar pe on
	pe.id = ft.periodo_escolar_id
inner join turma t on
	t.id = ft.turma_id
inner join ue on
	ue.id = t.ue_id
inner join conselho_classe cc on
	cc.fechamento_turma_id = ft.id
inner join conselho_classe_aluno cca on
	cca.conselho_classe_id = cc.id
inner join conselho_classe_nota ccn on
	ccn.conselho_classe_aluno_id = cca.id
inner join componente_curricular comp on comp.id = ccn.componente_curricular_codigo
	and comp.permite_lancamento_nota 
left join conceito_valores cv on ccn.conceito_id = cv.id
where t.ano_letivo = p_ano_letivo
	and (p_ueid = 0 or (p_ueid <> 0 and ue.id = p_ueid))
	and (p_dreid = 0 or (p_dreid <> 0 and ue.dre_id = p_dreid))
	and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and t.modalidade_codigo = p_modalidade_codigo))
	and (p_bimestre = 0 or (p_bimestre <> 0 and pe.bimestre = p_bimestre))
	and (p_semestre = 0 or (p_semestre <> 0 and t.semestre = p_semestre))
) x

$function$
;