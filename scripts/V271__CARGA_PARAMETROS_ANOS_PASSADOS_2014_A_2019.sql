-- Inserir registros 2014
insert into parametros_sistema
      (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select nome, tipo, descricao, valor, 2014, ativo, now(), criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
  from parametros_sistema
 where ano = 2020
   and tipo not in (select tipo 
                      from parametros_sistema 
                     where ano = 2014);

-- Inserir registros 2015
insert into parametros_sistema
      (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select nome, tipo, descricao, valor, 2015, ativo, now(), criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
  from parametros_sistema
 where ano = 2020
   and tipo not in (select tipo 
                      from parametros_sistema 
                     where ano = 2015);

-- Inserir registros 2016
insert into parametros_sistema
      (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select nome, tipo, descricao, valor, 2016, ativo, now(), criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
  from parametros_sistema
 where ano = 2020
   and tipo not in (select tipo 
                      from parametros_sistema 
                     where ano = 2016);

-- Inserir registros 2017
insert into parametros_sistema
      (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select nome, tipo, descricao, valor, 2017, ativo, now(), criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
  from parametros_sistema
 where ano = 2020
   and tipo not in (select tipo 
                      from parametros_sistema 
                     where ano = 2017);

-- Inserir registros 2018
insert into parametros_sistema
      (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select nome, tipo, descricao, valor, 2018, ativo, now(), criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
  from parametros_sistema
 where ano = 2020
   and tipo not in (select tipo 
                      from parametros_sistema 
                     where ano = 2018);

-- Inserir registros 2019
insert into parametros_sistema
      (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select nome, tipo, descricao, valor, 2019, ativo, now(), criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
  from parametros_sistema
 where ano = 2020
   and tipo not in (select tipo 
                      from parametros_sistema 
                     where ano = 2019);