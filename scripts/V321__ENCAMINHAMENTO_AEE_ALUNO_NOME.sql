alter table encaminhamento_aee drop if exists aluno_nome;
alter table encaminhamento_aee drop if exists aluno_numero;

alter table encaminhamento_aee add aluno_nome varchar;
alter table encaminhamento_aee add aluno_numero int4 not null default 0;
