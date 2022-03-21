USE Manutencao
GO


-- Essa rotina é necessária devido à problemas de acesso na base da Prodam
-- As tabelas são copiadas para a base de Manutenção e, no final do ETL, são truncadas

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_serie_turma_grade]
GO

INSERT   INTO  [Manutencao].[dbo].[ETL_SGP_Fechamento_serie_turma_grade]
SELECT * FROM  [db_educacao.rede.sp].[se1426].[dbo].[serie_turma_grade]
GO

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_escola_grade]
GO

INSERT   INTO  [Manutencao].[dbo].[ETL_SGP_Fechamento_escola_grade]
SELECT * FROM  [db_educacao.rede.sp].[se1426].[dbo].[escola_grade]
GO

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_grade_componente_curricular]
GO

INSERT   INTO  [Manutencao].[dbo].[ETL_SGP_Fechamento_grade_componente_curricular]
SELECT * FROM  [db_educacao.rede.sp].[se1426].[dbo].[grade_componente_curricular] 
GO

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_turma_escola]
GO

INSERT   INTO  [Manutencao].[dbo].[ETL_SGP_Fechamento_turma_escola]
SELECT * FROM [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[turma_escola]
GO

