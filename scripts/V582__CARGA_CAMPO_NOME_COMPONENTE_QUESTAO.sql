do $$
declare 
	questionarioId bigint;
	
begin
	/* ********************  */
	/* Encaminhamento NAAPA  */
	/* ********************  */
	select questionario_id 
		into questionarioId
	from secao_encaminhamento_naapa sen 
	where etapa = 1 and ordem = 1;

	update secao_encaminhamento_naapa set nome_componente = 'INFORMACOES_ESTUDANTE';

	update questao set nome_componente = 'DATA_ENTRADA_QUEIXA' where questionario_id = questionarioId and ordem = 0;
	update questao set nome_componente = 'PRIORIDADE' where questionario_id = questionarioId and ordem = 1;
	update questao set nome_componente = 'PORTA_ENTRADA' where questionario_id = questionarioId and ordem = 2;
	update questao set nome_componente = 'NIS' where questionario_id = questionarioId and ordem = 3;
	update questao set nome_componente = 'CNS' where questionario_id = questionarioId and ordem = 4;
	update questao set nome_componente = 'CONTATO_RESPONSAVEIS' where questionario_id = questionarioId and ordem = 5;
	update questao set nome_componente = 'ENDERECO_RESIDENCIAL' where questionario_id = questionarioId and ordem = 6;
	update questao set nome_componente = 'NOME_MAE' where questionario_id = questionarioId and ordem = 7;
	update questao set nome_componente = 'GENERO' where questionario_id = questionarioId and ordem = 8;
	update questao set nome_componente = 'GRUPO_ETNICO' where questionario_id = questionarioId and ordem = 9;
	update questao set nome_componente = 'ESTUDANTE_IMIGRANTE' where questionario_id = questionarioId and ordem = 10;
	update questao set nome_componente = 'RESPONSAVEL_IMIGRANTE' where questionario_id = questionarioId and ordem = 11;
	update questao set nome_componente = 'UBS' where questionario_id = questionarioId and ordem = 12;
	update questao set nome_componente = 'CRAS' where questionario_id = questionarioId and ordem = 13;
	update questao set nome_componente = 'ATIVIDADES_CONTRATURNO' where questionario_id = questionarioId and ordem = 14;
	update questao set nome_componente = 'DESCRICAO_ENCAMINHAMENTO' where questionario_id = questionarioId and ordem = 15;
	update questao set nome_componente = 'ANEXOS' where questionario_id = questionarioId and ordem = 16;


	/* ****************** */
	/* Encaminhamento AEE */
	/* ****************** */
	-- INFORMACOES_ESCOLARES
	select questionario_id 
		into questionarioId
	from secao_encaminhamento_AEE 
	where etapa = 1 and ordem = 1;

	update secao_encaminhamento_aee set nome_componente = 'INFORMACOES_ESCOLARES' where etapa = 1 and ordem = 1;

	update questao set nome_componente = 'INFORMACOES_ESCOLARES' where questionario_id = questionarioId and ordem = 0;
	update questao set nome_componente = 'JUSTIFICATIVA_AUSENCIAS' where questionario_id = questionarioId and ordem = 1;
	update questao set nome_componente = 'CLASSE_ESPECIALIZADA' where questionario_id = questionarioId and ordem = 2;
	update questao set nome_componente = 'ULTIMO_PERIODO_CLASSE_ESPECIALIZADA' where questionario_id = questionarioId and ordem = 3;

	-- DESCRICAO_ENCAMINHAMENTO
	select questionario_id 
		into questionarioId
	from secao_encaminhamento_AEE 
	where etapa = 1 and ordem = 2;

	update secao_encaminhamento_aee set nome_componente = 'DESCRICAO_ENCAMINHAMENTO' where etapa = 1 and ordem = 2;

	update questao set nome_componente = 'UPLOAD' where questionario_id = questionarioId and ordem = 0;
	update questao set nome_componente = 'MOTIVO_ENCAMINHAMENTO' where questionario_id = questionarioId and ordem = 1 and nome like '%qual(is) motivo(s)%';
	update questao set nome_componente = 'PORQUE' where questionario_id = questionarioId and ordem = 1 and nome like '%Por qu%';
	update questao set nome_componente = 'DETALHAMENTO_ATENDIMENTO_CLINICO' where questionario_id = questionarioId and ordem = 1 and tipo = 8;
	update questao set nome_componente = 'DESCRICAO_TIPO_ATENDIMENTO' where questionario_id = questionarioId and ordem = 1 and nome like '%Descreva o tipo de atendimento%';
	update questao set nome_componente = 'SELECAO_TIPO_ATENDIMENTO' where questionario_id = questionarioId and ordem = 1 and tipo = 9;
	update questao set nome_componente = 'DETALHAMENTO_ATIVIDADES' where questionario_id = questionarioId and ordem = 2 and tipo = 2;
	update questao set nome_componente = 'DIAGNOSTICO_LAUDO' where questionario_id = questionarioId and ordem = 2 and tipo = 3;
	update questao set nome_componente = 'ATIVIDADES_FAVORITAS' where questionario_id = questionarioId and ordem = 3;
	update questao set nome_componente = 'ATENCAO_ESTUDANTE' where questionario_id = questionarioId and ordem = 4;
	update questao set nome_componente = 'ATIVIDADES_DIFICEIS' where questionario_id = questionarioId and ordem = 5;
	update questao set nome_componente = 'ESTRATEGIAS_REALIZADAS' where questionario_id = questionarioId and ordem = 6;
	update questao set nome_componente = 'ATENDIMENTO_CLINICO' where questionario_id = questionarioId and ordem = 7;
	update questao set nome_componente = 'OUTRO_ATENDIMENTO' where questionario_id = questionarioId and ordem = 8;
	update questao set nome_componente = 'INFORMACOES_RELEVANTES' where questionario_id = questionarioId and ordem = 9;
	update questao set nome_componente = 'DOCUMENTOS_RELEVANTES' where questionario_id = questionarioId and ordem = 10;
	update questao set nome_componente = 'OBSERVACOES_ADICIONAIS' where questionario_id = questionarioId and ordem = 11;


	-- PARECER_COORDENACAO
	select questionario_id 
		into questionarioId
	from secao_encaminhamento_AEE 
	where etapa = 2 and ordem = 1;

	update secao_encaminhamento_aee set nome_componente = 'PARECER_COORDENACAO' where etapa = 2 and ordem = 1;

	update questao set nome_componente = 'MEDIACOES_PEDAGOGICAS' where questionario_id = questionarioId and ordem = 1 and nome like '%junto ao professor%';
	update questao set nome_componente = 'PROFISSIONAIS_PARTICIPANTES' where questionario_id = questionarioId and ordem = 1 and nome like '%Quais profissionais participaram%';
	update questao set nome_componente = 'MOTIVO_NAO_ENVOLVIMENTO_OUTROS' where questionario_id = questionarioId and ordem = 1 and nome like '%Justifique%';
	update questao set nome_componente = 'OBSERVACOES_ADICIONAIS' where questionario_id = questionarioId and ordem = 3;


	-- PARECER_AEE
	select questionario_id 
		into questionarioId
	from secao_encaminhamento_AEE 
	where etapa = 3 and ordem = 1;

	update secao_encaminhamento_aee set nome_componente = 'PARECER_AEE' where etapa = 3 and ordem = 1;

	update questao set nome_componente = 'BARREIRAS_IDENTIFICADAS' where questionario_id = questionarioId and ordem = 1 and tipo = 5;
	update questao set nome_componente = 'BARREIRAS_ARQUITETONICAS' where questionario_id = questionarioId and ordem = 1 and nome like '%Barreiras arquite%';
	update questao set nome_componente = 'CRITERIOS_ELEGIVEIS' where questionario_id = questionarioId and ordem = 1 and nome like '%Justifique a partir do estudo%';
	update questao set nome_componente = 'SUGESTOES_PRATICAS_PEDAGOGICAS' where questionario_id = questionarioId and ordem = 1 and nome like '%barreiras no contexto escolar do estudante%';
	update questao set nome_componente = 'SUGESTOES_FAMILIA' where questionario_id = questionarioId and ordem = 2 and nome like '%unidade educacional para orientar%';
	update questao set nome_componente = 'NECESSITA_ATENDIMENTO' where questionario_id = questionarioId and ordem = 2 and tipo = 3;
	update questao set nome_componente = 'BARREIRAS_COMUNICACOES' where questionario_id = questionarioId and ordem = 2 and nome like '%Barreiras nas comunica%';
	update questao set nome_componente = 'BARREIRAS_ATITUDINAIS' where questionario_id = questionarioId and ordem = 3;

	/* ***********/
	/* Plano AEE */
	/* ***********/
	select id 
		into questionarioId
	from questionario 
	where nome like '%Plano AEE%';

	update questao set nome_componente = 'BIMESTRE_VIGENCIA' where questionario_id = questionarioId and ordem = 1 and tipo = 12;
	update questao set nome_componente = 'JUSTIFICATIVA' where questionario_id = questionarioId and ordem = 1 and nome like '%Justifique%';
	update questao set nome_componente = 'ORGANIZACAO_AEE' where questionario_id = questionarioId and ordem = 2;
	update questao set nome_componente = 'DIAS_FREQUENCIA' where questionario_id = questionarioId and ordem = 3;
	update questao set nome_componente = 'FORMA_ATENDIMENTO' where questionario_id = questionarioId and ordem = 4;
	update questao set nome_componente = 'OBJETIVOS_AEE' where questionario_id = questionarioId and ordem = 5;
	update questao set nome_componente = 'ORIENTACOES_DESENVOLVIMENTO' where questionario_id = questionarioId and ordem = 6;
	update questao set nome_componente = 'RECURSOS_SALA_REGULAR' where questionario_id = questionarioId and ordem = 7;
	update questao set nome_componente = 'RECURSOS_SALA_MULTIFUNCIONAIS' where questionario_id = questionarioId and ordem = 8;
	update questao set nome_componente = 'MOBILIZACAO_RH_UE' where questionario_id = questionarioId and ordem = 9;
	update questao set nome_componente = 'MOBILIZACAO_RH_EXTERNO' where questionario_id = questionarioId and ordem = 10;

end $$

