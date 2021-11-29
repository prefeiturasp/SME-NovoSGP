--Plano AEE para validação da coordenação
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_plano_aee paee 
inner join pendencia p on p.id = paee.pendencia_id 
inner join plano_aee pa on pa.id  = paee.plano_aee_id 
inner join turma t on t.id = pa.turma_id  
where p.tipo = 18 and UPPER(descricao) like UPPER('%para acessar o plano e registrar o seu parecer%') 
) x
where x.pId = pe.id;

--Plano AEE para atribuição de responsável
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_plano_aee paee 
inner join pendencia p on p.id = paee.pendencia_id 
inner join plano_aee pa on pa.id  = paee.plano_aee_id 
inner join turma t on t.id = pa.turma_id  
where p.tipo = 18 and UPPER(descricao) like UPPER('%e atribuir um PAAI para que ele registre o parecer%')
) x
where x.pId = pe.id;

--Encaminhamento AEE para análise da Coordenação
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_encaminhamento_aee pea  
inner join pendencia p on p.id = pea.pendencia_id 
inner join encaminhamento_aee ea on ea.id  = pea.encaminhamento_aee_id 
inner join turma t on t.id = ea.turma_id  
where p.tipo = 18 and UPPER(descricao) like UPPER('%Esta pendência será resolvida automaticamente quando o parecer da coordenação%')
) x
where x.pId = pe.id;

--Encaminhamento AEE para análise do PAEE
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_encaminhamento_aee pea  
inner join pendencia p on p.id = pea.pendencia_id 
inner join encaminhamento_aee ea on ea.id  = pea.encaminhamento_aee_id 
inner join turma t on t.id = ea.turma_id  
where p.tipo = 18 and UPPER(descricao) like UPPER('%Esta pendência será resolvida automaticamente quando o parecer do AEE for registrado no sistema%')
) x
where x.pId = pe.id;


--Encaminhamento AEE para atribuição de responsável (CEFAI)
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_encaminhamento_aee pea  
inner join pendencia p on p.id = pea.pendencia_id 
inner join encaminhamento_aee ea on ea.id  = pea.encaminhamento_aee_id 
inner join turma t on t.id = ea.turma_id 
where p.tipo = 18 and UPPER(descricao) like UPPER('%Esta pendência será resolvida automaticamente quando o PAAI for atribuído no encaminhamento%')
) x
where x.pId = pe.id;

--Encaminhamento AEE para atribuição de responsável (CP)
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_encaminhamento_aee pea  
inner join pendencia p on p.id = pea.pendencia_id 
inner join encaminhamento_aee ea on ea.id  = pea.encaminhamento_aee_id 
inner join turma t on t.id = ea.turma_id 
where p.tipo = 18 and UPPER(descricao) like UPPER('%Esta pendência será resolvida automaticamente quando o PAEE for atribuído no encaminhamento%')
) x
where x.pId = pe.id;

--Aulas criadas em dias não letivos
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_aula pa
inner join pendencia p on p.id = pa.pendencia_id 
inner join aula a on a.id  = pa.aula_id 
inner join turma t on t.turma_id = a.turma_id 
where p.tipo = 11
) x
where x.pId = pe.id;

--Calendário com dias letivos abaixo do mínimo
update pendencia pe 
	set ue_id = x.ueId
from (
select pcu.ue_id as ueId, p.id as pId from pendencia_calendario_ue pcu
inner join pendencia p on p.id = pcu.pendencia_id 
where p.tipo = 12
) x
where x.pId = pe.id;

--Cadastro de eventos pendentes
update pendencia pe 
	set ue_id = x.ueId
from (
select pcu.ue_id as ueId, p.id as pId from pendencia_calendario_ue pcu
inner join pendencia p on p.id = pcu.pendencia_id 
where p.tipo = 13
) x
where x.pId = pe.id;

--Componente sem nenhuma avaliação no bimestre
update pendencia pe 
	set ue_id = x.ueId
from (
select t.ue_id as ueId, p.id as pId from pendencia_professor pp
inner join pendencia p on p.id = pp.pendencia_id
inner join turma t on t.id = pp.turma_id 
where p.tipo = 15
) x
where x.pId = pe.id;