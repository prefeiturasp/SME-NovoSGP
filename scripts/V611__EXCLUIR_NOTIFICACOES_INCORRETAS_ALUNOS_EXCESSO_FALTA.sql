--Notificação de alunos com excesso, somente CP recebe pelo fluxo atual
update notificacao
set n.excluida = true, 
n.alterado_em = current_timestamp,
n.alterado_por = 'Sistema',
n.alterado_rf = '0' 
where n.titulo like '%Alunos com excesso%' 
and extract (year from n.criado_em) = extract(year from now()) 
and not n.excluida 
and id not in(
select n.id from notificacao n
inner join usuario u on u.id = n.usuario_id 
inner join abrangencia a on a.usuario_id = u.id 
inner join ue ue on ue.id = a.ue_id 
where n.titulo like '%Alunos com excesso%' 
and extract (year from n.criado_em) = extract(year from now()) 
and not a.historico 
and a.perfil = '44E1E074-37D6-E911-ABD6-F81654FE895D' 
and ue.ue_id = n.ue_id
and not n.excluida)