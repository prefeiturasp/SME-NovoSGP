update aula 
   set recorrencia_aula = p.recorrencia_aula
 from (select a.recorrencia_aula, a.id from aula a where a.recorrencia_aula <> 1 and a.aula_pai_id is null) p
 where aula_pai_id = p.id
