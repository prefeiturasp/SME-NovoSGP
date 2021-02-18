CREATE VIEW [dbo].[ix_vw_consulta1] WITH SCHEMABINDING
AS
select pcc.cd_componente_curricular,
       cc.cd_componente_curricular as Codigo,  
       pcc.dc_componente_curricular, cc.dc_componente_curricular as Descricao,
       esc.tp_escola as TipoEscola, dtt.qt_hora_duracao as TurnoTurma,
       serie_ensino.sg_resumida_serie as AnoTurma
from dbo.turma_escola (nolock) te
inner join dbo.escola (nolock) esc ON te.cd_escola = esc.cd_escola
--Serie Ensino
inner join dbo.serie_turma_escola (nolock) ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
inner join dbo.serie_turma_grade (nolock) ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola
inner join dbo.escola_grade (nolock) ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
inner join dbo.grade (nolock) ON escola_grade.cd_grade = grade.cd_grade
inner join dbo.grade_componente_curricular (nolock) gcc on gcc.cd_grade = grade.cd_grade
inner join dbo.componente_curricular (nolock) cc on cc.cd_componente_curricular = gcc.cd_componente_curricular
and cc.dt_cancelamento is null
inner join dbo.serie_ensino (nolock)
ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino
-- Programa
inner join dbo.tipo_programa (nolock) tp on te.cd_tipo_programa = tp.cd_tipo_programa
inner join dbo.turma_escola_grade_programa (nolock) tegp on tegp.cd_turma_escola = te.cd_turma_escola
--inner join dbo.escola_grade (nolock) teg on teg.cd_escola_grade = tegp.cd_escola_grade
inner join dbo.grade (nolock) pg on pg.cd_grade = teg.cd_grade
inner join dbo.grade_componente_curricular (nolock) pgcc on pgcc.cd_grade = teg.cd_grade
inner join dbo.componente_curricular (nolock) pcc on pgcc.cd_componente_curricular = pcc.cd_componente_curricular
and pcc.dt_cancelamento is null
--Atribuição
inner join dbo.atribuicao_aula (nolock) aa
on (gcc.cd_grade = aa.cd_grade and gcc.cd_componente_curricular = aa.cd_componente_curricular
and aa.cd_serie_grade = serie_turma_grade.cd_serie_grade)
or
(pgcc.cd_grade = aa.cd_grade and pgcc.cd_componente_curricular = aa.cd_componente_curricular)
and aa.dt_cancelamento is null and aa.dt_disponibilizacao_aulas is null 
--and aa.an_atribuicao = year(getdate())
inner join dbo.v_cargo_base_cotic (nolock) vcbc on aa.cd_cargo_base_servidor = vcbc.cd_cargo_base_servidor
inner join dbo.v_servidor_cotic (nolock) vsc on vcbc.cd_servidor = vsc.cd_servidor
--          inner join funcao_atividade_cargo_servidor facs on aa.cd_cargo_base_servidor = facs.cd_cargo_base_servidor
--     and facs.dt_cancelamento is null and facs.dt_fim_funcao_atividade is null
inner join dbo.duracao_tipo_turno dtt on te.cd_tipo_turno = dtt.cd_tipo_turno and te.cd_duracao = dtt.cd_duracao
--	where te.cd_turma_escola = @codigoTurma
--	  and te.st_turma_escola in ('O', 'A', 'C')
--	  and vsc.cd_registro_funcional = @login