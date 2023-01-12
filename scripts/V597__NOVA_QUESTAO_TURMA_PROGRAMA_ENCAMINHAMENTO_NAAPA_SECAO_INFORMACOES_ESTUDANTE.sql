do $$
declare 
	_questionarioId bigint;	
begin
	_questionarioId := (select id from questionario where nome = 'Questionário Encaminhamento NAAPA Etapa 1 Seção 1');

	--inserindo nova questão
    insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(_questionarioId, 15, 'Turmas de programa', '', false, 19, '', NOW(), 'SISTEMA', '0', 12, 'TURMAS_PROGRAMA');		
		
	--atualizando ordem de questões já existentes (+1)
	update questao set ordem = 16 where questionario_id  = _questionarioId and nome_componente = 'DESCRICAO_ENCAMINHAMENTO';
	update questao set ordem = 17 where questionario_id  = _questionarioId and nome = 'Aplicação do fluxo de alerta';
	update questao set ordem = 18 where questionario_id  = _questionarioId and nome_componente = 'ANEXOS';	
end $$;