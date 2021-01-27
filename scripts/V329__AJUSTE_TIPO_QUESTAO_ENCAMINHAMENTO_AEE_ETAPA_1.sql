 update questao 
    set tipo = 9
  where ordem = 5 
    and questionario_id in (select id 
                              from secao_encaminhamento_aee 
                             where etapa = 1 
                               and ordem = 2);


  update questao 
     set tipo = 9
   where nome like 'Selecione os tipos de atendimento';