insert into parametros_sistema (nome,tipo,descricao,valor,ano,ativo,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf) 
select 'DiasAusenciaGerarPendenciaDeRegistroIndividual',48,'Dias de aus�ncia para gera��o de pend�ncias por registro individual',
15,2021,true,'2021-01-24 21:18:20','Carga inicial',null,null,'Carga inicial',null
where not exists(select * from parametros_sistema where tipo = 48 and ativo);