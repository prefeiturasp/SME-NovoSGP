update plano_aula
set descricao = descricao || ' <br/> ' || desenvolvimento_aula;

alter table plano_aula
drop column desenvolvimento_aula;