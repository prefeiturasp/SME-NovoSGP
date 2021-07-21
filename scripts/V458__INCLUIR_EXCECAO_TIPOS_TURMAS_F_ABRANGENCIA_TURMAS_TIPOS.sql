drop function f_abrangencia_turmas_tipos;

CREATE OR REPLACE FUNCTION public.f_abrangencia_turmas_tipos(p_login character varying, 
	p_perfil_id uuid, 
	p_historico boolean, 
	p_modalidade_codigo integer DEFAULT 0, 
	p_turma_semestre integer DEFAULT 0, 
	p_ue_codigo character varying DEFAULT NULL::character varying, 
	p_ano_letivo integer DEFAULT 0, 
	p_tipos_turma integer[] DEFAULT NULL::integer[], 
	p_anos_desconsiderar_turma_infantil text[] default null::text[])
 RETURNS SETOF v_estrutura_abrangencia_turmas_tipos
 LANGUAGE sql
AS $function$

select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma,
    act.nome_filtro
from v_abrangencia_nivel_dre a
    inner join v_abrangencia_cadeia_turmas act on a.dre_id = act.dre_id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and act.turma_historica = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
    and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
    )
union
select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma,
    act.nome_filtro
from v_abrangencia_nivel_ue a
    inner join v_abrangencia_cadeia_turmas act on a.ue_id = act.ue_id
    inner join ue on act.ue_id = ue.id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and act.turma_historica = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
    and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
    )
union
select distinct act.turma_ano,
    act.turma_ano_letivo,
    act.turma_codigo,
    act.modalidade_codigo,
    act.turma_nome,
    act.turma_semestre,
    act.qt_duracao_aula,
    act.tipo_turno,
    act.ensino_especial,
    act.turma_id,
    act.tipo_turma,
    act.nome_filtro
from v_abrangencia_nivel_turma a
    inner join v_abrangencia_cadeia_turmas act on a.turma_id = act.turma_id
    inner join ue on act.ue_id = ue.id
where a.login = p_login
    and a.perfil_id = p_perfil_id
    and a.historico = p_historico
    and (
        p_modalidade_codigo = 0
        or (
            p_modalidade_codigo <> 0
            and act.modalidade_codigo = p_modalidade_codigo
        )
    )
    and (
        p_turma_semestre = 0
        or (
            p_turma_semestre <> 0
            and act.turma_semestre = p_turma_semestre
        )
    )
    and (
        p_ue_codigo is null
        or (
            p_ue_codigo is not null
            and act.ue_codigo = p_ue_codigo
        )
    )
    and (
        p_ano_letivo = 0
        or (
            p_ano_letivo <> 0
            and act.turma_ano_letivo = p_ano_letivo
        )
    )
    and (
        p_tipos_turma is null
        or (
            array_length(p_tipos_turma, 1) > 0
            and act.tipo_turma = ANY(p_tipos_turma)
        )
    )
    and(
    	p_anos_desconsiderar_turma_infantil is null
    	or act.modalidade_codigo <> 1
    	or(
    		array_length(p_anos_desconsiderar_turma_infantil, 1) > 0 
    		and act.modalidade_codigo = 1
    		and not act.turma_ano = ANY(p_anos_desconsiderar_turma_infantil)
    	)
    )
$function$
;
