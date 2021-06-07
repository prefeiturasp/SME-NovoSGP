drop table aluno_turma
create table aluno_turma (
  codigoaluno varchar(max),
  nomealuno varchar(max),
  turmacodigo varchar(max),
  codigosituacaomatricula int
)

select distinct * from aluno_turma



select distinct * from (

insert into aluno_turma 
	SELECT aluno.cd_aluno CodigoAluno,
		aluno.nm_aluno NomeAluno,
		mte.cd_turma_escola as TurmaCodigo,
		mte.cd_situacao_aluno CodigoSituacaoMatricula
	FROM
		v_aluno_cotic aluno
	INNER JOIN v_matricula_cotic matr ON
		aluno.cd_aluno = matr.cd_aluno
	INNER JOIN matricula_turma_escola mte ON
		matr.cd_matricula = mte.cd_matricula
	inner join turma_escola te on 
		te.cd_turma_escola = mte.cd_turma_escola
	WHERE 1=1
	--	and mte.cd_turma_escola = '2299295'
		and te.an_letivo = 2019
		and mte.cd_situacao_aluno in (1, 6, 10, 13, 5)
	)
	union all

	
insert into aluno_turma 
	SELECT aluno.cd_aluno CodigoAluno,
		aluno.nm_aluno NomeAluno,
		mte.cd_turma_escola as TurmaCodigo,
		mte.cd_situacao_aluno CodigoSituacaoMatricula
	FROM
		v_aluno_cotic aluno
	INNER JOIN v_historico_matricula_cotic matr ON
		aluno.cd_aluno = matr.cd_aluno
	INNER JOIN historico_matricula_turma_escola mte ON
		matr.cd_matricula = mte.cd_matricula
	inner join turma_escola te on 
		te.cd_turma_escola = mte.cd_turma_escola
	WHERE
		mte.nr_chamada_aluno <> 0
		and mte.nr_chamada_aluno is not null
		and mte.dt_situacao_aluno = (
			select
				max(mte2.dt_situacao_aluno)
			from
				v_historico_matricula_cotic matr2
			INNER JOIN historico_matricula_turma_escola mte2 ON
				matr2.cd_matricula = mte2.cd_matricula
			where
				matr2.cd_aluno = matr.cd_aluno
				and (matr2.st_matricula in (1, 5, 6, 10, 13)) )
		AND NOT EXISTS(
			SELECT
				1
			FROM
				v_matricula_cotic matr3
			INNER JOIN matricula_turma_escola mte3 ON
				matr3.cd_matricula = mte3.cd_matricula
			WHERE
				mte.cd_matricula = mte3.cd_matricula)
		and te.an_letivo = 2019
) t