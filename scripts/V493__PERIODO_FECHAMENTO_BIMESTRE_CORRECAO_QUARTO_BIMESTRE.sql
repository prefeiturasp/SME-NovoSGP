update periodo_fechamento_bimestre set
    periodo_escolar_id = 56, 
    inicio_fechamento = '2021-11-29',
    final_fechamento = '2021-12-23'
where id in (
    select max_id from (
        select count(id), max(id) as max_id, periodo_fechamento_id, periodo_escolar_id 
        from periodo_fechamento_bimestre 
        where periodo_escolar_id in (53, 54, 55, 56)
        group by periodo_fechamento_id, periodo_escolar_id 
        having count(id) > 1
    ) tb
)