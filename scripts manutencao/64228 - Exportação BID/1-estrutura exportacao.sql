DROP TABLE if exists relatorio_bid;
CREATE TABLE relatorio_bid
(
	componente_curricular_id int8 not null,
	ano_letivo int not null,
	turma_id int8 not null,
	turma_nome VARCHAR(20) not null,
	turma_ano char(1) not null,
	aluno_codigo  varchar(15) not null,
	conceito_fechamento_id int8 null,
	nota_fechamento numeric(11,2) null,
	conceito_conselho_id int8 null,
	nota_conselho numeric(11,2) null,
	constraint relatorio_bid_pk primary key (ano_letivo, componente_curricular_id, turma_id, aluno_codigo)
)

DROP TABLE if exists relatorio_bid_frequencia;
create table relatorio_bid_frequencia (
	ano_letivo int not null,
	turma_id int8 not null,
	turma_nome VARCHAR(20) not null,
	turma_ano char(1) not null,
	aluno_codigo  varchar(15) not null,
	bimestre int not null,
	total_aulas int not null,
	total_ausencias int not null,
	total_compensacoes int not null,
	constraint relatorio_bid_frequencia_pk primary key (turma_id, aluno_codigo, bimestre)
)

