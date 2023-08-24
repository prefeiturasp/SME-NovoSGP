create table if not exists plano_aee_turma_aluno (
	id int8 not null generated always as identity,
	plano_aee_id int8 not null,
	turma_id int8 not null,
	aluno_codigo varchar(15) not null,
	criado_em timestamp not null,
	criado_por varchar(200) not null,
	alterado_em timestamp null,
	alterado_por varchar(200) null,
	criado_rf varchar(200) not null,
	alterado_rf varchar(200) null,
	constraint plano_aee_turma_aluno_pk primary key (id),
	constraint plano_aee_turma_aluno_plano_aee_fk foreign key (plano_aee_id) references plano_aee(id),
	constraint plano_aee_turma_aluno_turma_fk foreign key (turma_id) references turma(id)
);

create table if not exists encaminhamento_aee_turma_aluno (
	id int8 not null generated always as identity,
	encaminhamento_aee_id int8 not null,
	turma_id int8 not null,
	aluno_codigo varchar(15) not null,
	criado_em timestamp not null,
	criado_por varchar(200) not null,
	alterado_em timestamp null,
	alterado_por varchar(200) null,
	criado_rf varchar(200) not null,
	alterado_rf varchar(200) null,
	constraint encaminhamento_aee_turma_aluno_pk primary key (id),
	constraint encaminhamento_aee_turma_aluno_encaminhamento_aee_fk foreign key (encaminhamento_aee_id) references encaminhamento_aee(id),
	constraint encaminhamento_aee_turma_aluno_turma_fk foreign key (turma_id) references turma(id)
);