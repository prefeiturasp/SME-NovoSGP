
--Plano AEE para validação da coordenação
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 7, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 18 and UPPER(descricao) like UPPER('%para acessar o plano e registrar o seu parecer%');

--Plano AEE para atribuição de responsável
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 8, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 18 and UPPER(descricao) like UPPER('%e atribuir um PAAI para que ele registre o parecer%');

--Encaminhamento AEE para análise da Coordenação
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 7, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 18 and UPPER(descricao) like UPPER('%Esta pendência será resolvida automaticamente quando o parecer da coordenação%');

--Encaminhamento AEE para atribuição de responsável
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 8, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 18 and UPPER(descricao) like UPPER('%Esta pendência será resolvida automaticamente quando o PAAI for atribuído no encaminhamento%');

--Aulas criadas em dias não letivos
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 7, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 11;

--Calendário com dias letivos abaixo do mínimo

-- 4 Perfis:

--CP
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 7, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 12 order by id;

--AD
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 4, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 12 order by id;

--DIRETOR
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 9, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 12 order by id;

--ADMUE
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 5, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 12 order by id;

-- Cadastro de eventos pendentes
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 5, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 13;

--Componente sem nenhuma avaliação no bimestre
insert into pendencia_perfil (perfil_codigo, pendencia_id, criado_em, criado_por, criado_rf)
select 7, id, criado_em, criado_por,criado_rf from pendencia 
where tipo = 15;