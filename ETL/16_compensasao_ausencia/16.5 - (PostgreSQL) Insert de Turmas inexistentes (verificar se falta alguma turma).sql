
-- Executar o SELECT abaixo.
-- Se retornar alguma turma não cadastrada na tabela de turmas, será necessário importá-la(s) do legado
select distinct turma_id
  from etl_sgp_compensacao_ausencia as etl
 where cast(etl.turma_id as varchar) not in (select turma_id from turma)


-- No teste em homologação, foram identificadas as turmas abaixo.
-- Como eram de 2014 e o ambiente era de homologação, foram excluídas (somente 44 registros)
delete
  from etl_sgp_compensacao_ausencia
 where turma_id IN ('1417561','1417635','1417640','1417644','1417659','1417667','1417699','1417721','1417763','1417901',
                    '1417917','1417960','1418105','1418120','1418127','1710059','1874344','1890509','1981254','1981265')