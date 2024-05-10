
insert into conselho_classe_parecer_ano (parecer_id, ano_turma, modalidade, inicio_vigencia,criado_por,criado_rf,criado_em)
select 1,3,6,'2014-01-01','SISTEMA','0','2014-01-01'
where 
not exists(
	select 1 from conselho_classe_parecer_ano 
	where parecer_id = 1
	and ano_turma = 3
	and modalidade = 6
	and inicio_vigencia = '2014-01-01'
	and criado_por = 'SISTEMA'
	and criado_rf = '0'
	and criado_em = '2014-01-01'
);

