update notificacao set tipo = 14 --Inatividade Atendimento NAAPA
where tipo = 11 --NAAPA
and titulo like '%Encaminhamento NAAPA sem atendimento recente%'
and not excluida;

