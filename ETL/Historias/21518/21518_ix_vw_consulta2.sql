CREATE VIEW [dbo].[ix_vw_consulta2] WITH SCHEMABINDING
AS
select coalesce(stg.cd_turma_escola, tegp.cd_turma_escola) CodigosTurmas,
atb.cd_unidade_educacao                             CodigosUes,
dre.cd_unidade_educacao                             CodigosDres
from atribuicao_aula (nolock) atb
-- escolas
inner join escola (nolock) esc on atb.cd_unidade_educacao = esc.cd_escola
inner join v_cadastro_unidade_educacao (nolock) vue on vue.cd_unidade_educacao = esc.cd_escola
inner join (select v_ua.cd_unidade_educacao, v_ua.nm_unidade_educacao, v_ua.nm_exibicao_unidade
from unidade_administrativa (nolock) ua
inner join v_cadastro_unidade_educacao (nolock) v_ua
where tp_unidade_administrativa = 24) dre
on dre.cd_unidade_educacao = vue.cd_unidade_administrativa_referencia
--Servidor
inner join v_cargo_base_cotic (nolock) cbs on atb.cd_cargo_base_servidor = cbs.cd_cargo_base_servidor
and cbs.dt_cancelamento is null
inner join v_servidor_cotic (nolock) vsc on cbs.cd_servidor = vsc.cd_servidor
left join cargo_sobreposto_servidor (nolock) css on cbs.cd_cargo_base_servidor = css.cd_cargo_base_servidor
and css.dt_cancelamento is null and (css.dt_fim_cargo_sobreposto is null or
-- SerieGrade
left join serie_turma_grade (nolock) stg on atb.cd_serie_grade = stg.cd_serie_grade and stg.dt_fim is null
left join turma_escola (nolock) tur_reg on stg.cd_turma_escola = tur_reg.cd_turma_escola
and tur_reg.st_turma_escola in ('A', 'O') and tur_reg.an_letivo = year(getdate())
left join serie_turma_escola (nolock) ste
on tur_reg.cd_turma_escola = ste.cd_turma_escola and ste.dt_fim is null
left join serie_ensino (nolock) se on se.cd_serie_ensino = ste.cd_serie_ensino
left join etapa_ensino (nolock) ee on ee.cd_etapa_ensino = se.cd_etapa_ensino
-- ProgramaGrade
left join turma_escola_grade_programa (nolock) tegp
on tegp.cd_turma_escola_grade_programa = atb.cd_turma_escola_grade_programa and
tegp.dt_fim is null
left join turma_escola (nolock) tur_pro on tegp.cd_turma_escola = tur_pro.cd_turma_escola
--and tur_pro.st_turma_escola in ('A', 'O') and tur_pro.an_letivo = year(getdate())
where atb.dt_cancelamento is null
--and atb.an_atribuicao = year(getdate())
and dt_disponibilizacao_aulas is null
--and esc.tp_escola in (@tiposEscola1,@tiposEscola2,@tiposEscola3,@tiposEscola4,@tiposEscola5,@tiposEscola6,@tiposEscola7,@tiposEscola8,@tiposEscola9)
--and (tur_reg.cd_turma_escola is not null or tur_pro.cd_turma_escola is not null)
--and ((tur_reg.cd_turma_escola is not null and ee.cd_etapa_ensino in (@etapas1,@etapas2,@etapas3,@etapas4,@etapas5,@etapas6,@etapas7,@etapas8,@etapas9,@etapas10,@etapas11,@etapas12,@etapas13))
or tur_pro.cd_turma_escola is not null)
