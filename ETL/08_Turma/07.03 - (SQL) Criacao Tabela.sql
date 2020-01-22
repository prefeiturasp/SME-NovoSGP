USE [Manutencao]

INSERT INTO ETL_TURMAS_ANO
SELECT 
		 CodTurma				as turma_id,
		 CodEscola		    	as cd_escola,
		 NomeTurma				as nome,
		 Ano					as ano,
		 CodDre					as cd_unidade_educacao,
		 AnoLetivo				as ano_letivo,
		 CodModalidade			as modalidade_codigo,
		 Semestre				as semestre,
		 DuracaoTurno			as qt_duracao_aula,
		 TipoTurno				as tipo_turno,
		 data_atualizacao,
		 dt_fim_eol 
	FROM ETL_TURMAS
