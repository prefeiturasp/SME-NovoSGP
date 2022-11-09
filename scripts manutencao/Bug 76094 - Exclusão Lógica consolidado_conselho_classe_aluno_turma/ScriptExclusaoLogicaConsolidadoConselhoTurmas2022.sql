--1
begin transaction;

--2
with vw_consolidado_conselho_classe_aluno_turma_2022 as (
	select distinct(ccc.id) as id
    from consolidado_conselho_classe_aluno_turma ccc
    inner join turma t on t.id = ccc.turma_id 
  where t.ano_letivo = 2022 and not ccc.excluido 
)
update consolidado_conselho_classe_aluno_turma set excluido = true where id in (select id from vw_consolidado_conselho_classe_aluno_turma_2022);

--3
--rollback;
commit;
