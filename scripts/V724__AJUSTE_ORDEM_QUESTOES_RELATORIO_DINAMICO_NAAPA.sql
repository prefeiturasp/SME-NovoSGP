DO $$
DECLARE
    naapasUpdate record;
BEGIN
    FOR naapasUpdate IN
        with secao_questionario as (
			select s.nome_componente as nome_componente_secao,
				   q.id, q.tipo	       
			from questionario q
			inner join secao_encaminhamento_naapa s on s.questionario_id = q.id
			where q.tipo in (5,7)
			)
		select distinct sen.nome_componente, q2.nome_componente, q2.ordem as ordem_preenchimento, 
		q3.nome_componente, q3.ordem as ordem_relatorio, q3.id as questao_relatorio_id  
		from secao_encaminhamento_naapa sen 
		inner join secao_questionario q on q.nome_componente_secao = sen.nome_componente and q.tipo = 5
		inner join secao_questionario qq on qq.nome_componente_secao = sen.nome_componente and qq.tipo = 7
		inner join questao q2 on q2.questionario_id = q.id 
		inner join questao q3 on q3.questionario_id = qq.id and q3.nome_componente = q2.nome_componente 
		where q2.ordem <> q3.ordem 
		order by sen.nome_componente, q2.nome_componente, q3.nome_componente  
    LOOP
		update questao set ordem = naapasUpdate.ordem_preenchimento where id = naapasUpdate.questao_relatorio_id;		   
	END LOOP;
END $$;
