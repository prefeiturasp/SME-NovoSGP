alter table frequencia_aluno drop column if exists total_compensacoes;
alter table frequencia_aluno add column total_compensacoes int4 not null default 0;
