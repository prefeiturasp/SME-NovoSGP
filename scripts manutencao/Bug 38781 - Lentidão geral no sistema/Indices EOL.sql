-- v_cargo_base_cotic
CREATE INDEX v_cargo_base_cotic_cd_servidor_IDX ON v_cargo_base_cotic (cd_servidor) GO
CREATE INDEX v_cargo_base_cotic_cd_cargo_base_servidor_IDX ON v_cargo_base_cotic (cd_cargo_base_servidor) GO

-- v_servidor_cotic 
CREATE INDEX v_servidor_cotic_cd_servidor_IDX ON v_servidor_cotic (cd_servidor) GO
CREATE INDEX v_servidor_cotic_cd_registro_funcional_IDX ON v_servidor_cotic (cd_registro_funcional) GO

-- atribuicao_aula
CREATE INDEX atribuicao_aula_cd_componente_curricular_IDX ON atribuicao_aula (cd_componente_curricular) GO
CREATE INDEX atribuicao_aula_dt_cancelamento_IDX ON atribuicao_aula (dt_cancelamento) GO
CREATE INDEX atribuicao_aula_dt_disponibilizacao_aulas_IDX ON atribuicao_aula (dt_disponibilizacao_aulas) GO
CREATE INDEX atribuicao_aula_cd_grade_IDX ON atribuicao_aula (cd_grade) GO

-- turma_escola
CREATE INDEX turma_escola_cd_escola_IDX ON turma_escola (cd_escola) GO

-- serie_turma_grade
CREATE INDEX serie_turma_grade_cd_escola_grade_IDX ON serie_turma_grade (cd_escola_grade) GO

-- escola_grade
CREATE INDEX escola_grade_cd_grade_IDX ON escola_grade (cd_grade) GO

-- componente_curricular
CREATE INDEX componente_curricular_dt_cancelamento_IDX ON componente_curricular (dt_cancelamento) GO

-- turma_escola_grade_programa
CREATE INDEX turma_escola_grade_programa_cd_turma_escola_IDX ON turma_escola_grade_programa (cd_turma_escola) GO
CREATE INDEX turma_escola_grade_programa_cd_escola_grade_IDX ON turma_escola_grade_programa (cd_escola_grade) GO

