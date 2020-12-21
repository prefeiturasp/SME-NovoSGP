-- drop table tmp_fechamentos_duplicados
create table tmp_fechamentos_duplicados (
  turma_id int8 not null,
  quantidade int8 not null,
  fechamento_turma_id int8 not null
);

commit;

-- select * from tmp_fechamentos_duplicados;
-- delete from tmp_fechamentos_duplicados;
insert into tmp_fechamentos_duplicados (turma_id, quantidade, fechamento_turma_id)  
select turma_id, count(id), min(id)
  from fechamento_turma
 where periodo_escolar_id is null
   and extract(year from criado_em ) = 2020
 group by turma_id 
having count(id) > 1
order by 3;

-- Exclusao de notas
delete from fechamento_nota where fechamento_aluno_id in (
  select fa.id from fechamento_aluno fa
  inner join fechamento_turma_disciplina ft on ft.id = fa.fechamento_turma_disciplina_id
  inner join tmp_fechamentos_duplicados fd on fd.fechamento_turma_id = ft.fechamento_turma_id
);

-- Exclusão de fechamento aluno
delete from fechamento_aluno where fechamento_turma_disciplina_id in (
  select ft.id from fechamento_turma_disciplina ft 
  inner join tmp_fechamentos_duplicados fd on fd.fechamento_turma_id = ft.fechamento_turma_id
);

-- Exclusão de fechamento disciplina
delete from fechamento_turma_disciplina where fechamento_turma_id in (
  select fd.fechamento_turma_id from tmp_fechamentos_duplicados fd 
);

-- Exclusão de fechamento turma
delete from fechamento_turma where id in (
  select fd.fechamento_turma_id from tmp_fechamentos_duplicados fd 
);
