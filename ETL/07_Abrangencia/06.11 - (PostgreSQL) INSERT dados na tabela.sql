-- passo 11:
-- IMPORT dos dados na tabela 'public.abrangencia'
-- tendo como origem a tabela 'etl_abrangencia'
-- usar SELECT com INSERT 


insert into abrangencia
(usuario_id,  dre_id,  ue_id,  turma_id,  perfil,  historico,  dt_fim_vinculo)
select cast(id as int8) as usuario_id, 
       cast(dre_id_eol as int8) dre_id, 
       cast(ue_id_eol as int8) as ue_id, 
       turma_id_sgp as turma_id, 
       cast(perfil as uuid) as perfil, 
       cast(historico as boolean) as historico, 
       dt_fim_vinculo 
from etl_abrangencia

