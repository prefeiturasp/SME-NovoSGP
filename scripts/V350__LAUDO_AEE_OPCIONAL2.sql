update questao q
	set obrigatorio = true
where q.questionario_id in (select questionario_id from secao_encaminhamento_aee sea where etapa = 1 and ordem = 2)
  and q.ordem = 2
  and q.tipo = 3;

update questao q
	set obrigatorio = false
where q.questionario_id in (select questionario_id from secao_encaminhamento_aee sea where etapa = 1 and ordem = 2)
  and q.ordem = 0
  and q.tipo = 6;
