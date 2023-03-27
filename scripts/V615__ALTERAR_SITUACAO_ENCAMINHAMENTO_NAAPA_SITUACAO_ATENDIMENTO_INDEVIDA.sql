
update encaminhamento_naapa 
set situacao = 3,
alterado_em = current_timestamp,
alterado_por = 'Sistema',
alterado_rf = '0' where id in (
select distinct en.id from encaminhamento_naapa en 
left join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = en.id 
left join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id 
where not en.excluido 
and en.situacao = 2  
and sen.nome_componente = 'QUESTOES_ITINERACIA'
and ens.concluido 
and not ens.excluido) 