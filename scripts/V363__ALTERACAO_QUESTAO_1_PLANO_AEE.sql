update questao set tipo = 12, nome = 'Periodo Escolar', somente_leitura = true
where questionario_id in (select id from questionario q where tipo = 2) and tipo = 10;

