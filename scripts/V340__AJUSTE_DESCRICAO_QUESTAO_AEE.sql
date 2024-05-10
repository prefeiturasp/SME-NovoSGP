update questao q
  set nome = 'O estudante/criança necessita do Atendimento Educacional Especializado?'
  where questionario_id = (select questionario_id from secao_encaminhamento_aee sea where etapa = 3)
    and ordem = 2
    and tipo = 3;