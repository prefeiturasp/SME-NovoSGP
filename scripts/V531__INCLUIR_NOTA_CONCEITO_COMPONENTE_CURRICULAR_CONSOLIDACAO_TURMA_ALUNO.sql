alter table consolidado_conselho_classe_aluno_turma drop column if exists nota;
alter table consolidado_conselho_classe_aluno_turma add column nota numeric(11, 2) NULL;

alter table consolidado_conselho_classe_aluno_turma drop column if exists conceito_id;
alter table consolidado_conselho_classe_aluno_turma add column conceito_id int8 NULL;

alter table consolidado_conselho_classe_aluno_turma drop column if exists componente_curricular_id;
alter table consolidado_conselho_classe_aluno_turma add column componente_curricular_id int8 NULL;