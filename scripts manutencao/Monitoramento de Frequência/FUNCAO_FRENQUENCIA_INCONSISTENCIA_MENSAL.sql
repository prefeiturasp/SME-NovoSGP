do $$
declare
	turma_db record;
begin	
	raise notice 'Iniciando validação frenquência geral mensal';
	raise notice 'Criando tabela';
	DROP TABLE IF EXISTS public.consolidacao_frequencia_aluno_mensal_inconsistencia;
	CREATE TABLE public.consolidacao_frequencia_aluno_mensal_inconsistencia (
		id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
		codigo_aluno VARCHAR(15) NOT NULL,
		turma_id VARCHAR(15) NOT NULL,
		bimestre int4 NOT null,
		CONSTRAINT consolidacao_frequencia_aluno_mensal_inconsistencia_pk PRIMARY KEY (id)
	);
	raise notice 'Iniciando carga das inconsistências';
	for turma_db in 
		select t.turma_id from turma t where ano_letivo = extract('Year' from now()) 
  	loop
  		for valorMes in 1..12 
  		loop
  			INSERT INTO consolidacao_frequencia_aluno_mensal_inconsistencia (codigo_aluno, turma_id, bimestre)
  			select diaria.codigo_aluno, tab_mes.turma_id, bimestre
			from 
			(select t.turma_id, aluno_codigo, mes, quantidade_aulas, quantidade_ausencias, quantidade_compensacoes 
			 from consolidacao_frequencia_aluno_mensal cfam
			 inner join turma t on t.id = cfam.turma_id  
			 where t.turma_id = turma_db.turma_id and cfam.mes = valorMes) tab_mes 
			 inner join
			 (select rfa.codigo_aluno, a.turma_id, pe.bimestre,  
			  count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 1) totalCompareceu,
			  count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 2) totalFaltou,
			  count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 3) totalRemoto,
			  count(1) totalAula
		      from registro_frequencia_aluno rfa
			  inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id 
			  inner join aula a on a.id = rf.aula_id 
			  inner join tipo_calendario tc on tc.id = a.tipo_calendario_id 
			  inner join periodo_escolar pe on pe.id = tc.periodo 
			  where a.turma_id = turma_db.turma_id and Extract('Month' from a.data_aula) = valorMes
			  group by rfa.codigo_aluno, a.turma_id,  pe.bimestre) diaria on 
				diaria.codigo_aluno = tab_mes.aluno_codigo and diaria.turma_id = tab_mes.turma_id 
			  where totalFaltou <> tab_mes.quantidade_ausencias and totalAula <> tab_mes.quantidade_aulas; 
  		end loop;
  	end loop;
    raise notice 'Finalizando carga das inconsistências';
    commit;  
end $$ 