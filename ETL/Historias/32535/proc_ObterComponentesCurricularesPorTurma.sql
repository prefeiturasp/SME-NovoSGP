USE [se1426]
GO

/****** Object:  UserDefinedFunction [dbo].[proc_ObterComponentesCurricularesPorTurma]    Script Date: 08/02/2021 19:23:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[proc_ObterComponentesCurricularesPorTurma]
(	
	@p_cd_turma_escola AS INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT DISTINCT iif(pcc.cd_componente_curricular is NOT NULL, pcc.cd_componente_curricular,
                    cc.cd_componente_curricular) as Codigo,
					iif(pcc.dc_componente_curricular is NOT NULL, pcc.dc_componente_curricular,
						cc.dc_componente_curricular) as Descricao,
						serie_ensino.sg_resumida_serie   as AnoTurma
	FROM 
		turma_escola te (NOLOCK)
	INNER JOIN 
		escola esc (NOLOCK) 
		ON te.cd_escola = esc.cd_escola
	INNER JOIN 
		v_cadastro_unidade_educacao (NOLOCK) ue 
		ON ue.cd_unidade_educacao = esc.cd_escola
	INNER JOIN 
		unidade_administrativa dre (NOLOCK) 
		ON dre.tp_unidade_administrativa = 24 AND ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
	--Serie Ensino
	LEFT JOIN 
		serie_turma_escola (NOLOCK) 
		ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
	LEFT JOIN 
		serie_turma_grade (NOLOCK) 
		ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola and serie_turma_grade.dt_fim is NULL
	LEFT JOIN 
		escola_grade (NOLOCK) 
		ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
	LEFT JOIN 
		grade (NOLOCK) 
		ON escola_grade.cd_grade = grade.cd_grade
	LEFT JOIN 
		grade_componente_curricular gcc (NOLOCK) 
		ON gcc.cd_grade = grade.cd_grade
	LEFT JOIN 
		componente_curricular cc (NOLOCK) 
		ON cc.cd_componente_curricular = gcc.cd_componente_curricular and cc.dt_cancelamento is NULL
	LEFT JOIN 
		serie_ensino
		ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino
	-- Programa
	LEFT JOIN 
		tipo_programa tp (NOLOCK) 
		ON te.cd_tipo_programa = tp.cd_tipo_programa
	LEFT JOIN 
		turma_escola_grade_programa tegp (NOLOCK) 
		ON tegp.cd_turma_escola = te.cd_turma_escola
	LEFT JOIN 
		escola_grade teg (NOLOCK) 
		ON teg.cd_escola_grade = tegp.cd_escola_grade
	LEFT JOIN 
		grade pg (NOLOCK) 
		ON pg.cd_grade = teg.cd_grade
	LEFT JOIN 
		grade_componente_curricular pgcc 
		ON pgcc.cd_grade = teg.cd_grade
	LEFT JOIN 
		componente_curricular pcc 
		ON pgcc.cd_componente_curricular = pcc.cd_componente_curricular and pcc.dt_cancelamento is NULL
	WHERE 
		te.cd_turma_escola = @p_cd_turma_escola
		AND te.st_turma_escola in ('O', 'A', 'C')
)
GO


