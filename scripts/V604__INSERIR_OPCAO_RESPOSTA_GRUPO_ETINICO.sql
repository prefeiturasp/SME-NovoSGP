insert
	into
	public.opcao_resposta (questao_id,
	ordem,
	nome,
	excluido,
	criado_em,
	criado_por,
	criado_rf)
values ((select id from questao where nome_componente = 'GRUPO_ETNICO'),
7,
'Preta',
false,
now(),
'SISTEMA',
'0');