update ocorrencia set ue_id = ocorrencia_db.ue_id 
from(
select oco.id as ocorrencia_id, tu.ue_id  from ocorrencia oco inner join turma tu on tu.id = oco.turma_id 
) ocorrencia_db
where id = ocorrencia_db.ocorrencia_id;

ALTER TABLE ocorrencia ALTER COLUMN ue_id SET NOT NULL;
ALTER TABLE ocorrencia ALTER COLUMN turma_id DROP NOT NULL;
