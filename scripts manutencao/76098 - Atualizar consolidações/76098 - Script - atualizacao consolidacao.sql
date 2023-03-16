--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--> Obter divergências entre fechamentos turma disciplina e consolidação
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
select distinct ftd.id fechamento_turma_disciplina_id, ftd.disciplina_id, p.bimestre,ft.turma_id, ftd.situacao situacaoFechamento, c.Id consolidacaoId, c.status situacaoConsolidacao  
into tmp_fix76098
from fechamento_turma_disciplina ftd
	 join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
	 join periodo_escolar p on p.id = ft.periodo_escolar_id 
	 join turma t on t.id = ft.turma_id
	 join consolidado_fechamento_componente_turma c on c.bimestre = p.bimestre and c.componente_curricular_id = ftd.disciplina_id and c.turma_id = ft.turma_id 
where t.ano_letivo = 2022   
      and ftd.situacao <> c.status

select * from tmp_fix76098

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--> 2 - Atualizar consolidação
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
begin transaction;

select c.id, c.status as situacaoConsolidacaoAntes, f.situacaoFechamento, 'ANTES' as Resultado from tmp_fix76098 f join consolidado_fechamento_componente_turma c on f.consolidacaoId = c.id;

update consolidado_fechamento_componente_turma con set status = fc.situacaoFechamento
from ( select f.situacaoFechamento,f.consolidacaoId from tmp_fix76098 f join consolidado_fechamento_componente_turma c on f.consolidacaoId = c.id) fc
where fc.consolidacaoId = con.id;

select c.id, c.status as situacaoConsolidacaoDepois, f.situacaoFechamento, 'DEPOIS' as Resultado from tmp_fix76098 f join consolidado_fechamento_componente_turma c on f.consolidacaoId = c.id;

rollback;

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--> Remover temporária
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--drop table tmp_fix76098


