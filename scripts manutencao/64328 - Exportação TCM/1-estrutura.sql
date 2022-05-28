drop table if exists relatorio_tcm;
create table relatorio_tcm (
	codigo_aluno  varchar(15) not null,
	turma_id  varchar(15) null,
	bimestre int null,
	total_aulas int null,
	total_ausencias int null,
	total_compensacoes int null,
	constraint relatorio_tcm_pk primary key (codigo_aluno, turma_id, bimestre)
)
