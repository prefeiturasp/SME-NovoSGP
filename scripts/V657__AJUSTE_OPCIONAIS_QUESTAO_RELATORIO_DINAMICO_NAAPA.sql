--> Ajustando as informações opcionais da questão de 'Data de entrada da queixa' 
update questao 
	set opcionais = '{"desabilitarDataFutura":true}' 
where nome = 'Data de entrada da queixa' 
and opcionais <> '{"desabilitarDataFutura":true}';

--> Definir obrigatoriedade de campos
update questao set obrigatorio = false
where id in(
SELECT questao.id
     FROM questionario q
     JOIN questao on q.id = questao.questionario_id
     WHERE q.tipo = 7 and questao.obrigatorio);