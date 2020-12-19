CREATE TEMP TABLE totalPlanosDuplicados AS
select t.turma_id, t.nome, pa.componente_curricular_id,pe.bimestre, count(distinct pa.id) total, MIN(pa.criado_em) criado_em from planejamento_anual pa
inner join planejamento_anual_periodo_escolar pape on pa. id = pape.planejamento_anual_id
inner join planejamento_anual_componente pac on pape.id = pac.planejamento_anual_periodo_escolar_id
inner join periodo_escolar pe on pe.id = pape.periodo_escolar_id
inner join turma t on t.id = pa.turma_id
where not pa.excluido and not pape.excluido and not pac.excluido 
group by t.turma_id, t.nome, pa.componente_curricular_id,pe.bimestre
having count(distinct pa.id) > 1
order by t.nome, pe.bimestre

CREATE TEMP TABLE planosDuplicados AS
select pa.id from planejamento_anual pa
inner join planejamento_anual_periodo_escolar pape on pa.id = pape.planejamento_anual_id
inner join planejamento_anual_componente pac on pape.id = pac.planejamento_anual_periodo_escolar_id
inner join periodo_escolar pe on pe.id = pape.periodo_escolar_id
inner join turma t on t.id = pa.turma_id
inner join totalPlanosDuplicados tpd on t.turma_id = tpd.turma_id and pa.componente_curricular_id = tpd.componente_curricular_id and pe.bimestre = tpd.bimestre
where not pa.excluido and not pape.excluido and not pac.excluido and pa.criado_em > tpd.criado_em
order by t.nome, pe.bimestre

CREATE TEMP TABLE planosPeriodoDuplicados AS
select id from planejamento_anual_periodo_escolar pe
where pe.planejamento_anual_id in (select id from planosDuplicados)

CREATE TEMP TABLE planosComponenteDuplicados AS
select id from planejamento_anual_componente pc
where pc.planejamento_anual_periodo_escolar_id  in (select id from planosPeriodoDuplicados)

--delete from planejamento_anual_componente 
--where id in (select id from planosComponenteDuplicados)

--delete from planejamento_anual_periodo_escolar 
--where id in (select id from planosPeriodoDuplicados)

--delete from planejamento_anual 
--where id in (select id from planosDuplicados)