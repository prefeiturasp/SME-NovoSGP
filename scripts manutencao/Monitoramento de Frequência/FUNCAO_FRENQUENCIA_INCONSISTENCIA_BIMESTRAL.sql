do $$
declare
	turma_db record;
begin	
	raise notice 'Iniciando validação frenquência bimestral';
	raise notice 'Criando tabela';
	DROP TABLE IF EXISTS public.frequencia_aluno_inconsistencia;
	CREATE TABLE public.frequencia_aluno_inconsistencia (
		id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
		codigo_aluno VARCHAR(15) NOT NULL,
		turma_id VARCHAR(15) NOT NULL,
		bimestre int4 NOT null,
		CONSTRAINT frequencia_aluno_inconsistencia_pk PRIMARY KEY (id)
	);
	raise notice 'Iniciando carga das inconsistências';
	for turma_db in 
		select t.turma_id from turma t where ano_letivo = extract('Year' from now()) 
  	loop
  		for valorBimestre in 1..4
  		loop
  			INSERT INTO frequencia_aluno_inconsistencia (codigo_aluno, turma_id, bimestre)
  			select tab.codigo_aluno, tab.turma_id, tab.bimestre
			from 
			(select codigo_aluno, periodo_escolar_id, turma_id, 
			 sum(total_aulas) total_aulas, 
		     sum(total_ausencias) total_ausencias, 
			 sum(total_presencas) total_presencas, 
			 sum(total_remotos) total_remotos 
			 from frequencia_aluno fa
			 inner join periodo_escolar pe on pe.id = fa.periodo_escolar_id  
			 where not fa.excluido 
			 and tipo = 2 
			 and pe.bimestre = valorBimestre
			 and turma_id = turma_db.turma_id 
			 group by codigo_aluno, periodo_escolar_id, turma_id) fa 
			 inner join
			(select rfa.codigo_aluno, pe.bimestre, turma_id, pe.id periodo_id, 
			 count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 1) totalCompareceu,
			 count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 2) totalFaltou,
			 count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 3) totalRemoto,
			 count(1) totalAula
			 from registro_frequencia_aluno rfa
			 inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id 
			 inner join aula a on a.id = rf.aula_id 
			 inner join periodo_escolar pe on pe.tipo_calendario_id  = a.tipo_calendario_id  
			 where not rfa.excluido 
             and not rf.excluido 
             and not a.excluido
			 and pe.periodo_inicio <= a.data_aula 
			 and pe.periodo_fim >= a.data_aula 
			 and turma_id = turma_db.turma_id 
			 and pe.bimestre = valorBimestre
			 group by rfa.codigo_aluno, pe.bimestre, turma_id, pe.id) tab 
			 on tab.periodo_id = fa.periodo_escolar_id 
			 and tab.turma_id = fa.turma_id 
			 and tab.codigo_aluno = fa.codigo_aluno 
			 where totalFaltou <> fa.total_ausencias 
			 --or totalCompareceu <> fa.total_presencas //Na base de hom2 esse valor não bate
			 or totalRemoto <> fa.total_remotos 
			 or totalAula <> fa.total_aulas;
  		end loop;
  	end loop;
    raise notice 'Finalizando carga das inconsistências';
    commit;  
end $$ 