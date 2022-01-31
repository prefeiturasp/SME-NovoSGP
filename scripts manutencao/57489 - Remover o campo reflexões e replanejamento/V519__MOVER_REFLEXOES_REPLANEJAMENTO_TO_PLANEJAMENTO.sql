update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202008'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202009'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202010'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202011'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202012'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202101'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202102'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202103'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202104'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202105'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202106'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202107'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202108'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202109'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202110'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202111'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202112'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202201'
and reflexoes_replanejamento <> '';

update diario_bordo 
set planejamento = planejamento || '<br/>' || reflexoes_replanejamento
where to_char(criado_em, 'YYYYMM') = '202202'
and reflexoes_replanejamento <> '';

alter table diario_bordo
drop column reflexoes_replanejamento;