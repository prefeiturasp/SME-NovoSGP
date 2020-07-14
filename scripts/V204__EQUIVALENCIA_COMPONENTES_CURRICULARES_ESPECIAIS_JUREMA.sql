-- Data de criação: 06/07/2020
-- Descrição: Insere equivalência de componentes curriculares jurema para Libras, Libras compartilhada e língua portuguesa

insert into public.componente_curricular(codigo_jurema, codigo_eol, descricao_eol, criado_em, criado_por , alterado_em, alterado_por , criado_rf , alterado_rf )
select 10, 218, 'Libras', CURRENT_DATE, 'Carga', null, null, 'Carga', null
where not exists(select * from public.componente_curricular where codigo_jurema = 10 and codigo_eol = 218 and descricao_eol = 'Libras');

insert into public.componente_curricular(codigo_jurema, codigo_eol, descricao_eol, criado_em, criado_por , alterado_em, alterado_por , criado_rf , alterado_rf )
select 10, 1116, 'Libras compartilhada', CURRENT_DATE, 'Carga', null, null, 'Carga', null
where not exists(select * from public.componente_curricular where codigo_jurema = 10 and codigo_eol = 1116 and descricao_eol = 'Libras compartilhada');

insert into public.componente_curricular(codigo_jurema, codigo_eol, descricao_eol, criado_em, criado_por , alterado_em, alterado_por , criado_rf , alterado_rf )
select 11, 138, 'Língua Portuguesa', CURRENT_DATE, 'Carga', null, null, 'Carga', null
where not exists(select * from public.componente_curricular where codigo_jurema = 11 and codigo_eol = 138 and descricao_eol = 'Língua Portuguesa');