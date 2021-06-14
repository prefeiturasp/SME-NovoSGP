update questao 
   set observacao = ''
 where questionario_id in (select id from questionario where tipo = 2) and ordem = 1 and tipo = 12;