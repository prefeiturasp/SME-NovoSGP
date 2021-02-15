update questao qs 
   set obrigatorio = false
 where qs.ordem = 3
   and qs.questionario_id in (select id from questionario q where q.tipo = 2); 
