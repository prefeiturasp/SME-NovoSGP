 update questao 
    set nome = 'Diante das dificuldades apresentadas acima, quais estratégias pedagógicas foram feitas em sala de aula antes do encaminhamento ao AEE?' 
  where ordem = 6
    and questionario_id in (select id 
                              from secao_encaminhamento_aee 
                             where etapa = 1 
                               and ordem = 2)