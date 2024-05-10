insert into registro_ausencia_aluno
   (codigo_aluno, numero_aula, registro_frequencia_id, migrado, excluido, 
	criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select codigo_aluno, 1, registro_frequencia_id, migrado, excluido, 
	   criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
  from registro_ausencia_aluno
 where numero_aula = 4
   and migrado = true
   and excluido = false;
   
insert into registro_ausencia_aluno
   (codigo_aluno, numero_aula, registro_frequencia_id, migrado, excluido, 
	criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select codigo_aluno, 2, registro_frequencia_id, migrado, excluido, 
	   criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
  from registro_ausencia_aluno
 where numero_aula = 4
   and migrado = true
   and excluido = false;

insert into registro_ausencia_aluno
   (codigo_aluno, numero_aula, registro_frequencia_id, migrado, excluido, 
	criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select codigo_aluno, 3, registro_frequencia_id, migrado, excluido, 
	   criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
  from registro_ausencia_aluno
 where numero_aula = 4
   and migrado = true
   and excluido = false;