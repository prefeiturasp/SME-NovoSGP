CREATE OR REPLACE FUNCTION public.f_ata_bimestral_obter_notas_turmas_alunos(p_ano_letivo integer DEFAULT 2021, p_modalidade_codigo integer DEFAULT 0, p_semestre integer DEFAULT 0, p_tipos_turma integer[] DEFAULT NULL::integer[], p_alunos_codigos character varying[] DEFAULT NULL::character varying(15)[], p_bimestre integer default 0)
 RETURNS TABLE(conselhoclassealunoid bigint, codigoturma character varying, tipoturma integer, turmacomplementarid bigint, codigoaluno character varying, codigocomponentecurricular bigint, aprovado boolean, bimestre integer, periodoinicio timestamp without time zone, periodofim timestamp without time zone, notaid bigint, conceitoid bigint, conceito character varying, nota numeric)
 LANGUAGE sql
AS $function$
  
select
	distinct *
from
	(
	select 
		cca.id as ConselhoClasseAlunoId,
		t.turma_id CodigoTurma,
		t.tipo_turma as TipoTurma,
		ccatc.turma_id TurmaComplementarId,
		fa.aluno_codigo CodigoAluno,
		fn.disciplina_id CodigoComponenteCurricular,
		coalesce(ccp.aprovado, false) as Aprovado,
		coalesce(pe.bimestre, 0) Bimestre,
		pe.periodo_inicio PeriodoInicio,
		pe.periodo_fim PeriodoFim,
		fn.id NotaId,
		coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId,
		coalesce(cvc.valor, cvf.valor) as Conceito,
		coalesce(ccn.nota, fn.nota) as Nota
	from
		fechamento_turma ft
	left join periodo_escolar pe on
		pe.id = ft.periodo_escolar_id
	inner join turma t on
		t.id = ft.turma_id 
    inner join fechamento_turma_disciplina ftd on
		ftd.fechamento_turma_id = ft.id
	inner join fechamento_aluno fa on
		fa.fechamento_turma_disciplina_id = ftd.id
	inner join fechamento_nota fn on
		fn.fechamento_aluno_id = fa.id
	left join conceito_valores cvf on
		fn.conceito_id = cvf.id
	left join conselho_classe cc on
		cc.fechamento_turma_id = ft.id
	left join conselho_classe_aluno cca on
		cca.conselho_classe_id = cc.id
		and cca.aluno_codigo = fa.aluno_codigo
	left join conselho_classe_aluno_turma_complementar ccatc on
		cca.id = ccatc.conselho_classe_aluno_id
	left join conselho_classe_parecer ccp on
		cca.conselho_classe_parecer_id = ccp.id
	left join conselho_classe_nota ccn on
		ccn.conselho_classe_aluno_id = cca.id
		and ccn.componente_curricular_codigo = fn.disciplina_id 
                         left join conceito_valores cvc on
		ccn.conceito_id = cvc.id		
	where
		fa.aluno_codigo = any(p_alunos_codigos)
		and (p_ano_letivo = 0 or (p_ano_letivo <> 0 and t.ano_letivo = p_ano_letivo))
		and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and t.modalidade_codigo = p_modalidade_codigo))
		and (p_semestre = 0 or (p_semestre <> 0 and t.semestre = p_semestre))
		and (p_tipos_turma is null or (array_length(p_tipos_turma, 1) > 0 and t.tipo_turma = any(p_tipos_turma)))
		and pe.bimestre = p_bimestre
union all
	select
		cca.id as ConselhoClasseAlunoId,
		t.turma_id CodigoTurma,
		t.tipo_turma as TipoTurma,
		ccatc.turma_id TurmaComplementarId,
		cca.aluno_codigo CodigoAluno,
		ccn.componente_curricular_codigo CodigoComponenteCurricular,
		coalesce(ccp.aprovado, false) as Aprovado,
		coalesce(pe.bimestre, 0) Bimestre,
		pe.periodo_inicio PeriodoInicio,
		pe.periodo_fim PeriodoFim,
		ccn.id NotaId,
		coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId,
		coalesce(cvc.valor, cvf.valor) as Conceito,
		coalesce(ccn.nota, fn.nota) as Nota
	from
		fechamento_turma ft
	left join periodo_escolar pe on
		pe.id = ft.periodo_escolar_id
	inner join conselho_classe cc on
		cc.fechamento_turma_id = ft.id
	inner join conselho_classe_aluno cca on
		cca.conselho_classe_id = cc.id
	inner join conselho_classe_nota ccn on
		ccn.conselho_classe_aluno_id = cca.id
	left join conselho_classe_aluno_turma_complementar ccatc on
		cca.id = ccatc.conselho_classe_aluno_id
	inner join turma t on
		t.id = ccatc.turma_id
	left join conselho_classe_parecer ccp on
		cca.conselho_classe_parecer_id = ccp.id
	left join conceito_valores cvc on
		ccn.conceito_id = cvc.id
	left join fechamento_turma_disciplina ftd on
		ftd.fechamento_turma_id = ft.id
	left join fechamento_aluno fa on
		fa.fechamento_turma_disciplina_id = ftd.id
		and cca.aluno_codigo = fa.aluno_codigo
	left join fechamento_nota fn on
		fn.fechamento_aluno_id = fa.id
		and ccn.componente_curricular_codigo = fn.disciplina_id
	left join conceito_valores cvf on
		fn.conceito_id = cvf.id
	where
		cca.aluno_codigo = any(p_alunos_codigos)
		and (p_ano_letivo = 0 or (p_ano_letivo <> 0 and t.ano_letivo = p_ano_letivo))
		and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and t.modalidade_codigo = p_modalidade_codigo))
		and (p_semestre = 0 or (p_semestre <> 0 and t.semestre = p_semestre))
		and (p_tipos_turma is null or (array_length(p_tipos_turma, 1) > 0 and t.tipo_turma = any(p_tipos_turma)))
		and pe.bimestre = p_bimestre
union all
	select
		cca.id as ConselhoClasseAlunoId,
		t.turma_id CodigoTurma,
		t.tipo_turma as TipoTurma,
		null as TurmaComplementarId,
		cca.aluno_codigo CodigoAluno,
		ccn.componente_curricular_codigo CodigoComponenteCurricular,
		coalesce(ccp.aprovado, false) as Aprovado,
		coalesce(pe.bimestre, 0) as bimestre,
		pe.periodo_inicio PeriodoInicio,
		pe.periodo_fim PeriodoFim,
		ccn.id NotaId,
		coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId,
		coalesce(cvc.valor, cvf.valor) as Conceito,
		coalesce(ccn.nota, fn.nota) as Nota
	from
		fechamento_turma ft
	left join periodo_escolar pe on
		pe.id = ft.periodo_escolar_id
	inner join turma t on
		t.id = ft.turma_id
	inner join conselho_classe cc on
		cc.fechamento_turma_id = ft.id
	inner join conselho_classe_aluno cca on
		cca.conselho_classe_id = cc.id
	inner join conselho_classe_nota ccn on
		ccn.conselho_classe_aluno_id = cca.id
	left join conselho_classe_parecer ccp
 on
		cca.conselho_classe_parecer_id = ccp.id
	left join conceito_valores cvc on
		ccn.conceito_id = cvc.id
	left join fechamento_turma_disciplina ftd on
		ftd.fechamento_turma_id = ft.id
	left join fechamento_aluno fa on
		fa.fechamento_turma_disciplina_id = ftd.id
		and cca.aluno_codigo = fa.aluno_codigo
	left join fechamento_nota fn on
		fn.fechamento_aluno_id = fa.id
		and ccn.componente_curricular_codigo = fn.disciplina_id
	left join conceito_valores cvf on
		fn.conceito_id = cvf.id
	where
		cca.aluno_codigo = any(p_alunos_codigos)
		and (p_ano_letivo = 0 or (p_ano_letivo <> 0 and t.ano_letivo = p_ano_letivo))
		and (p_modalidade_codigo = 0 or (p_modalidade_codigo <> 0 and t.modalidade_codigo = p_modalidade_codigo))
		and (p_semestre = 0 or (p_semestre <> 0 and t.semestre = p_semestre))
		and (p_tipos_turma is null or (array_length(p_tipos_turma, 1) > 0 and t.tipo_turma = any(p_tipos_turma))
		and pe.bimestre = p_bimestre)) x		
		$function$;
