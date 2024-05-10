do $$
declare 
	tipoCalendarioId int8;

begin
	-- Infantil
	select min(id) into tipoCalendarioId 
	  from tipo_calendario tc 
	 where ano_letivo = 2021 
	   and modalidade = 3;
	  
	update aula set tipo_calendario_id = tipoCalendarioId where id in (
		select a.id
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 1
		   and a.tipo_calendario_id <> tipoCalendarioId
	);

	-- Fundamental/Medio
	select min(id) into tipoCalendarioId 
	  from tipo_calendario tc 
	 where ano_letivo = 2021 
	   and modalidade = 1;

	update aula set tipo_calendario_id = tipoCalendarioId where id in (
		select a.id
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo in (5,6)
		   and a.tipo_calendario_id <> tipoCalendarioId
	);

	-- Exclusão de aulas EJA 
	delete from notificacao_aula where aula_id in (
		select a.id 
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 3
		   and a.data_aula between '2021-07-09' and '2021-12-31');

	delete from registro_frequencia where aula_id in (
		select a.id 
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 3
		   and a.data_aula between '2021-07-09' and '2021-12-31');
		  
	delete from plano_aula where aula_id in (
		select a.id 
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 3
		   and a.data_aula between '2021-07-09' and '2021-12-31');
		  
	delete from anotacao_frequencia_aluno where aula_id in (
		select a.id 
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 3
		   and a.data_aula between '2021-07-09' and '2021-12-31');

	delete from pendencia_aula where aula_id in (
		select a.id 
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 3
		   and a.data_aula between '2021-07-09' and '2021-12-31');
	  
	delete from aula where id in (
		select a.id 
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 3
		   and a.data_aula between '2021-07-09' and '2021-12-31');

	-- EJA 1o. Semestre
	select min(id) into tipoCalendarioId 
	  from tipo_calendario tc 
	 where ano_letivo = 2021 
	   and modalidade = 2
	   and exists(select 1 from periodo_escolar pe where pe.tipo_calendario_id = tc.id and periodo_inicio < '2021-05-01');
	  
	update aula set tipo_calendario_id = tipoCalendarioId where id in (
		select a.id
		  from turma t
		 inner join aula a on a.turma_id = t.turma_id and not a.excluido 
		 where t.ano_letivo = 2021
		   and t.modalidade_codigo = 3
		   and a.tipo_calendario_id <> tipoCalendarioId
	);

end $$
