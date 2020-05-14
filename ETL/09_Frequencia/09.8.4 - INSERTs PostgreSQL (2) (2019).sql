insert into registro_ausencia_aluno
   (codigo_aluno, numero_aula, registro_frequencia_id, migrado, excluido, 
	criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select codigo_aluno, 1, registro_frequencia_id, migrado, excluido, 
	   criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
  from registro_ausencia_aluno
 where numero_aula = 2
   and migrado = true
   and excluido = false
   and date_part('year',criado_em) = 2019;