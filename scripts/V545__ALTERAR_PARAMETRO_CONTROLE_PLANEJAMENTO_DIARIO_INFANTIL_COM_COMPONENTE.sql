update public.parametros_sistema 
   set nome = 'SeprarDiarioBordoPorComponente'
 where tipo = 85;
                
delete from public.parametros_sistema 
 where tipo = 86;