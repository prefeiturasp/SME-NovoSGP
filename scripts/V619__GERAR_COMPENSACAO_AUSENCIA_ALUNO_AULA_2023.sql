do $$
declare
	_turmas_com_compensacao record;
	_ausencias record;
begin	
	
	for _turmas_com_compensacao in 
		select  ca.turma_id, 
			   t.turma_id 
			   turma_codigo, 
			   ca.bimestre,
			   ca.disciplina_id,
			   caa.id as compensacao_ausencia_aluno_id,
			   caa.codigo_aluno,
			   caa.qtd_faltas_compensadas
		from compensacao_ausencia ca
			join compensacao_ausencia_aluno caa on caa.compensacao_ausencia_id = ca.id
			left join compensacao_ausencia_aluno_aula caaa on caaa.compensacao_ausencia_aluno_id = caa.id and not caaa.excluido 
			join turma t on t.id = ca.turma_id
		where ca.ano_letivo = 2023
			  and not ca.excluido
			  and not caa.excluido 
			  and caaa.id is null
  	loop
 		
	  	for _ausencias in 
			select rfa.id registro_frequencia_aluno_Id, 
				   a.data_aula, 
				   rfa.numero_aula
			from aula a
				inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id
											and a.data_aula between pe.periodo_inicio and pe.periodo_fim
				inner join registro_frequencia rf on rf.aula_id = a.id and not rf.excluido			
				inner join registro_frequencia_aluno rfa on rfa.registro_frequencia_id = rf.id and not rfa.excluido
				left join compensacao_ausencia_aluno_aula caaa	on caaa.registro_frequencia_aluno_id = rfa.id and not caaa.excluido			
			where not a.excluido
				and a.turma_id = _turmas_com_compensacao.turma_codigo
				and a.disciplina_id = _turmas_com_compensacao.disciplina_id
				and pe.bimestre = _turmas_com_compensacao.bimestre
				and rfa.codigo_aluno = _turmas_com_compensacao.codigo_aluno
				and rfa.valor = 2
				and caaa.id is null	
			order by a.data_aula, rfa.numero_aula 	
			limit _turmas_com_compensacao.qtd_faltas_compensadas			  
			
		  	loop
		  		insert into compensacao_ausencia_aluno_aula (compensacao_ausencia_aluno_id,registro_frequencia_aluno_id, numero_aula, data_aula, criado_por, criado_em, criado_rf) 
				values (_turmas_com_compensacao.compensacao_ausencia_aluno_id,
					    _ausencias.registro_frequencia_aluno_Id,
					    _ausencias.numero_aula,
					    _ausencias.data_aula,
					    'SISTEMA', 
					    current_timestamp, 
					    'SISTEMA');
			   RAISE NOTICE 'Inserindo compensacao_ausencia_aluno_id %', _turmas_com_compensacao.compensacao_ausencia_aluno_id;
		  	end loop;	  								 
  	end loop;
end $$