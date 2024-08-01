do $$
declare 
	questionarioId bigint;
begin
	questionarioId := (select id from questionario q 
						where q.tipo = 2 and q.nome like '%Plano AEE%');		
	
	update questao set nome = 'Possui atendimento de AVE (Auxiliar de vida escolar)'
		where id in (select q2.id from questao q2
			where q2.questionario_id = questionarioId
			and q2.id not in (select oqc.questao_complementar_id from opcao_questao_complementar oqc)
			and q2.nome_componente = 'POSSUI_AVE'
			order by q2.ordem);
		
	update questao set nome = 'Possui apoio do estagiário na turma'
		where id in (select q2.id from questao q2
			where q2.questionario_id = questionarioId
			and q2.id not in (select oqc.questao_complementar_id from opcao_questao_complementar oqc)
			and q2.nome_componente = 'POSSUI_ESTAGIARIO_TURMA'
			order by q2.ordem);	
end $$;
		