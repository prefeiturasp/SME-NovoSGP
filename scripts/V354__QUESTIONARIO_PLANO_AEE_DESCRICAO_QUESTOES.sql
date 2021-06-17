update questao 
	set nome = 'Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala regular (Seleção de materiais, equipamentos e mobiliário)'
	, observacao = ''
 where questionario_id in (select id from questionario q where tipo = 2) and ordem = 7;

update questao 
	set nome = 'Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala de recursos multifuncionais (Seleção de materiais, equipamentos e mobiliário)'
	, observacao = ''
 where questionario_id in (select id from questionario q where tipo = 2) and ordem = 8;

update questao 
	set nome = 'Mobilização dos Recursos Humanos da U.E. ou parcerias na unidade educacional'
	, observacao = ''
 where questionario_id in (select id from questionario q where tipo = 2) and ordem = 9;

update questao 
	set nome = 'Mobilização dos Recursos Humanos com profissionais externos à unidade educacional'
	, observacao = ''
 where questionario_id in (select id from questionario q where tipo = 2) and ordem = 10;
