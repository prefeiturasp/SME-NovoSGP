update parametros_sistema
   set ano = 2020   
 where ano is null 
   and ativo = true
   and tipo not in (22, 25, 26, 28, 29, 100);