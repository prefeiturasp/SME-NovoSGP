insert into parametros_sistema (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2014, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2014) union
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2015, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2015) union
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2016, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2016) union
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2017, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2017) union
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2018, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2018) union
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2019, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2019) union
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2020, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2020) union
select 'TempoValidadeProcessoExecutandoEmSegundos', 57, 'Parâmetro para definição do tempo de validade de um registro de processo executando em segundos', 300, 2021, true, current_date, 'Sistema', null::timestamp, null, 'Sistema', null
where not exists (select 1 from parametros_sistema where tipo = 57 and ano = 2021);