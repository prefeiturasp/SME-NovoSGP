CREATE TABLE IF NOT EXISTS public.frequencia_aluno (
	id bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno varchar(100) NOT NULL,
	tipo int4 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	periodo_inicio timestamp NOT NULL,
	periodo_fim timestamp NOT NULL,
	bimestre int4 NOT NULL,
	total_aulas int4 NOT NULL,
	total_ausencias int4 NOT null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido boolean not null default false,
	migrado boolean not null default false
);

select f_cria_constraint_se_nao_existir('frequencia_aluno','frequencia_aluno_un','UNIQUE (codigo_aluno, tipo, disciplina_id, periodo_inicio, periodo_fim)')