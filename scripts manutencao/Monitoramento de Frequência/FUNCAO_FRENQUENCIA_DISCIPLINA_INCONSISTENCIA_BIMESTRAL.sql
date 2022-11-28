do $$
declare
	turma_db record;
	periodo_db record;
begin	
	raise notice 'Iniciando validação frenquência disciplina mensal';
	raise notice 'Criando tabela';
	DROP TABLE IF EXISTS public.frequencia_aluno_disciplina_inconsistencia;
	CREATE TABLE public.frequencia_aluno_disciplina_inconsistencia (
		id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
		codigo_aluno VARCHAR(15) NOT NULL,
		turma_id VARCHAR(15) NOT NULL,
		bimestre int4 NOT null,
		CONSTRAINT frequencia_aluno_disciplina_inconsistencia_pk PRIMARY KEY (id)
	);
	raise notice 'Iniciando carga das inconsistências';
	for turma_db in 
		select t.turma_id from turma t where ano_letivo = extract('Year' from now()) 
  	loop
  		for periodo_db in 
			 select distinct pe.id perido_id 			 
			 from periodo_escolar pe 
			 inner join aula a on pe.tipo_calendario_id  = a.tipo_calendario_id  
			 where turma_id = turma_db.turma_id 
  		loop
			INSERT INTO frequencia_aluno_disciplina_inconsistencia (codigo_aluno, turma_id, bimestre)
  			select diaria.codigo_aluno, diaria.turma_id, diaria.bimestre
			from 
			(select rfa.codigo_aluno, pe.bimestre, turma_id, 
			 count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 1) totalCompareceu,
			 count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 2) totalFaltou,
			 count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) filter (where rfa.valor = 3) totalRemoto,
			 count(distinct(rfa.aula_id*rfa.numero_aula)) totalAula,
			 a.disciplina_id 
			 from registro_frequencia_aluno rfa
			 inner join aula a on a.id = rfa.aula_id 
			 inner join periodo_escolar pe on pe.tipo_calendario_id  = a.tipo_calendario_id  
			 where not rfa.excluido 
             and not a.excluido
			 and pe.periodo_inicio <= a.data_aula 
			 and pe.periodo_fim >= a.data_aula 
			 and turma_id = turma_db.turma_id
			 and pe.id = periodo_db.perido_id
			 group by rfa.codigo_aluno, pe.bimestre, turma_id, pe.id, disciplina_id) diaria 
			 left join
			 (select codigo_aluno, periodo_escolar_id, turma_id, disciplina_id, 
			 sum(total_aulas) total_aulas, 
		     sum(total_ausencias) total_ausencias, 
			 sum(total_presencas) total_presencas, 
			 sum(total_remotos) total_remotos 
			 from frequencia_aluno  
			 where not excluido 
			 and tipo = 1 
			 and periodo_escolar_id = periodo_db.perido_id
			 and turma_id = turma_db.turma_id
			 group by codigo_aluno, periodo_escolar_id, turma_id, disciplina_id) consolidado 
			 on diaria.turma_id = consolidado.turma_id 
			 and diaria.codigo_aluno = consolidado.codigo_aluno 
			 and diaria.disciplina_id = consolidado.disciplina_id
			 where totalFaltou <> total_ausencias 
			 or totalCompareceu <> total_presencas 
			 or totalRemoto <> total_remotos 
			 or totalAula <> total_aulas;
  		end loop;
  	end loop;
    raise notice 'Finalizando carga das inconsistências';
    commit;  
end $$ 