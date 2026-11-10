-- Correção das seções (abas), questões e respostas duplicadas no Encaminhamento NAAPA.

-- A tela (ObterRespostasEncaminhamento) soma as respostas de todas as cópias, o objetivo é deixar uma cópia,
-- com o conteúdo que a tela já exibe hoje, sem repetição, priorizando o que o usuário alterou por último.

-- Tudo é feito através da exclusão lógica, nada é apagado. O conteúdo original das colunas de auditoria é mantido
-- e recebe o sufixo '_V782'.

drop table if exists pg_temp.tmp_v782_secao_excluir;
drop table if exists pg_temp.tmp_v782_questao_excluir;
drop table if exists pg_temp.tmp_v782_questao_origem;
drop table if exists pg_temp.tmp_v782_destino;

create or replace function pg_temp.v782_preenchida(p_resposta_id bigint, p_arquivo_id bigint, p_texto varchar)
returns boolean language sql immutable as $$
    select p_resposta_id is not null
        or p_arquivo_id is not null
        or coalesce(btrim(p_texto), '') not in ('', '""', '[]', '{}', 'null');
$$;

create or replace function pg_temp.v782_atividade_usuario(p_criado_em timestamp, p_criado_rf varchar,
                                                           p_alterado_em timestamp, p_alterado_rf varchar)
returns timestamp language sql immutable as $$
    select greatest(case when upper(coalesce(p_criado_rf, '0')) not in ('0', 'SISTEMA') then p_criado_em end,
                    case when upper(coalesce(p_alterado_rf, '0')) not in ('0', 'SISTEMA') then p_alterado_em end);
$$;

-- 1) Seções a excluir (avalia só as combinações encaminhamento + seção que estão repetidas)
create temp table tmp_v782_secao_excluir as
with repetida as (
    select ens.encaminhamento_naapa_id, ens.secao_encaminhamento_id
      from encaminhamento_naapa_secao ens
      join encaminhamento_naapa en on en.id = ens.encaminhamento_naapa_id and not en.excluido
      join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id
     where not ens.excluido
       and coalesce(sen.nome_componente, '') not in ('QUESTOES_ITINERANCIA', 'QUESTOES_ITINERACIA')
     group by ens.encaminhamento_naapa_id, ens.secao_encaminhamento_id
    having count(*) > 1
),
atividade as (
    select ens.id,
           ens.encaminhamento_naapa_id,
           ens.secao_encaminhamento_id,
           greatest(pg_temp.v782_atividade_usuario(ens.criado_em, ens.criado_rf, ens.alterado_em, ens.alterado_rf),
                    max(pg_temp.v782_atividade_usuario(enq.criado_em, enq.criado_rf, enq.alterado_em, enq.alterado_rf)),
                    max(pg_temp.v782_atividade_usuario(enr.criado_em, enr.criado_rf, enr.alterado_em, enr.alterado_rf))) as atividade_usuario,
           greatest(coalesce(ens.alterado_em, ens.criado_em),
                    max(coalesce(enq.alterado_em, enq.criado_em)),
                    max(coalesce(enr.alterado_em, enr.criado_em))) as atividade_geral
      from repetida r
      join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = r.encaminhamento_naapa_id
                                         and ens.secao_encaminhamento_id = r.secao_encaminhamento_id
                                         and not ens.excluido
      left join encaminhamento_naapa_questao enq on enq.encaminhamento_naapa_secao_id = ens.id and not enq.excluido
      left join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id and not enr.excluido
     group by ens.id
),
ranqueada as (
    select id,
           row_number() over (partition by encaminhamento_naapa_id, secao_encaminhamento_id
                              order by atividade_usuario desc nulls last, atividade_geral desc, id desc) as posicao
      from atividade
)
select id from ranqueada where posicao > 1;

analyze tmp_v782_secao_excluir;

-- 2) Questões repetidas a excluir, dentro das seções que ficam (inclusive cópias de itinerância)
create temp table tmp_v782_questao_excluir as
with repetida as (
    select enq.encaminhamento_naapa_secao_id, enq.questao_id
      from encaminhamento_naapa_questao enq
      join encaminhamento_naapa_secao ens on ens.id = enq.encaminhamento_naapa_secao_id and not ens.excluido
      join encaminhamento_naapa en on en.id = ens.encaminhamento_naapa_id and not en.excluido
     where not enq.excluido
       and not exists (select 1 from tmp_v782_secao_excluir s where s.id = ens.id)
     group by enq.encaminhamento_naapa_secao_id, enq.questao_id
    having count(*) > 1
),
atividade as (
    select enq.id,
           enq.encaminhamento_naapa_secao_id,
           enq.questao_id,
           coalesce(bool_or(enr.id is not null and pg_temp.v782_preenchida(enr.resposta_id, enr.arquivo_id, enr.texto)), false) as possui_resposta,
           greatest(pg_temp.v782_atividade_usuario(enq.criado_em, enq.criado_rf, enq.alterado_em, enq.alterado_rf),
                    max(pg_temp.v782_atividade_usuario(enr.criado_em, enr.criado_rf, enr.alterado_em, enr.alterado_rf))) as atividade_usuario,
           greatest(coalesce(enq.alterado_em, enq.criado_em),
                    max(coalesce(enr.alterado_em, enr.criado_em))) as atividade_geral
      from repetida r
      join encaminhamento_naapa_questao enq on enq.encaminhamento_naapa_secao_id = r.encaminhamento_naapa_secao_id
                                           and enq.questao_id = r.questao_id
                                           and not enq.excluido
      left join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id and not enr.excluido
     group by enq.id
),
ranqueada as (
    select id,
           row_number() over (partition by encaminhamento_naapa_secao_id, questao_id
                              order by possui_resposta desc, atividade_usuario desc nulls last, atividade_geral desc, id desc) as posicao
      from atividade
)
select id from ranqueada where posicao > 1;

analyze tmp_v782_questao_excluir;

-- Todas as questões ativas que vão ser excluídas (das seções do passo 1 e as do passo 2)
create temp table tmp_v782_questao_origem as
select enq.id,
       enq.questao_id,
       ens.id as secao_id,
       ens.encaminhamento_naapa_id,
       ens.secao_encaminhamento_id,
       true as secao_excluida
  from tmp_v782_secao_excluir s
  join encaminhamento_naapa_secao ens on ens.id = s.id
  join encaminhamento_naapa_questao enq on enq.encaminhamento_naapa_secao_id = ens.id and not enq.excluido
union all
select enq.id,
       enq.questao_id,
       ens.id,
       ens.encaminhamento_naapa_id,
       ens.secao_encaminhamento_id,
       false
  from tmp_v782_questao_excluir qe
  join encaminhamento_naapa_questao enq on enq.id = qe.id
  join encaminhamento_naapa_secao ens on ens.id = enq.encaminhamento_naapa_secao_id;

analyze tmp_v782_questao_origem;

-- 3a) Cria na seção mantida a questão que só existe (com resposta preenchida) nas cópias excluídas
insert into encaminhamento_naapa_questao (encaminhamento_naapa_secao_id, questao_id, excluido, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select distinct on (sm.id, o.questao_id)
       sm.id, o.questao_id, false, qo.criado_em,
       left(coalesce(qo.criado_por, ''), 195) || '_V782', left(coalesce(qo.criado_rf, ''), 195) || '_V782',
       qo.alterado_em, left(qo.alterado_por, 195) || '_V782', left(qo.alterado_rf, 195) || '_V782'
  from tmp_v782_questao_origem o
  join encaminhamento_naapa_questao qo on qo.id = o.id
  join encaminhamento_naapa_secao sm on sm.encaminhamento_naapa_id = o.encaminhamento_naapa_id
                                    and sm.secao_encaminhamento_id = o.secao_encaminhamento_id
                                    and not sm.excluido
                                    and not exists (select 1 from tmp_v782_secao_excluir s where s.id = sm.id)
 where o.secao_excluida
   and exists (select 1
                 from encaminhamento_naapa_resposta r
                where r.questao_encaminhamento_id = o.id
                  and not r.excluido
                  and pg_temp.v782_preenchida(r.resposta_id, r.arquivo_id, r.texto))
   and not exists (select 1
                     from encaminhamento_naapa_questao qm
                    where qm.encaminhamento_naapa_secao_id = sm.id
                      and qm.questao_id = o.questao_id
                      and not qm.excluido
                      and not exists (select 1 from tmp_v782_questao_excluir qe where qe.id = qm.id))
 order by sm.id, o.questao_id, coalesce(qo.alterado_em, qo.criado_em) desc, o.id desc;

-- Destino de cada questão excluída: a questão que fica para a mesma pergunta.
--   passo 2 (seção fica)     -> destino na mesma seção (protege as cópias legítimas da itinerância);
--   passo 1 (seção excluída) -> destino na seção mantida do mesmo encaminhamento e mesma seção configurada.
create temp table tmp_v782_destino as
select o.id as questao_origem_id,
       qm.id as questao_destino_id,
       q.tipo
  from tmp_v782_questao_origem o
  join questao q on q.id = o.questao_id
  join encaminhamento_naapa_secao sm on sm.encaminhamento_naapa_id = o.encaminhamento_naapa_id
                                    and sm.secao_encaminhamento_id = o.secao_encaminhamento_id
                                    and not sm.excluido
                                    and (sm.id = o.secao_id or o.secao_excluida)
                                    and not exists (select 1 from tmp_v782_secao_excluir s where s.id = sm.id)
  join encaminhamento_naapa_questao qm on qm.encaminhamento_naapa_secao_id = sm.id
                                      and qm.questao_id = o.questao_id
                                      and not qm.excluido
                                      and not exists (select 1 from tmp_v782_questao_excluir qe where qe.id = qm.id);

analyze tmp_v782_destino;

-- 3b) Questão que fica sem resposta preenchida recebe as respostas preenchidas da cópia mais recente
with origem_ranqueada as (
    select d.questao_destino_id,
           d.questao_origem_id,
           row_number() over (partition by d.questao_destino_id
                              order by max(coalesce(enr.alterado_em, enr.criado_em)) desc, d.questao_origem_id desc) as posicao
      from tmp_v782_destino d
      join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = d.questao_origem_id
                                            and not enr.excluido
                                            and pg_temp.v782_preenchida(enr.resposta_id, enr.arquivo_id, enr.texto)
     where not exists (select 1
                         from encaminhamento_naapa_resposta atual
                        where atual.questao_encaminhamento_id = d.questao_destino_id
                          and not atual.excluido
                          and pg_temp.v782_preenchida(atual.resposta_id, atual.arquivo_id, atual.texto))
     group by d.questao_destino_id, d.questao_origem_id
)
insert into encaminhamento_naapa_resposta (questao_encaminhamento_id, resposta_id, arquivo_id, texto, excluido, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select o.questao_destino_id, enr.resposta_id, enr.arquivo_id, enr.texto, false, enr.criado_em,
       left(coalesce(enr.criado_por, ''), 195) || '_V782', left(coalesce(enr.criado_rf, ''), 195) || '_V782',
       enr.alterado_em, left(enr.alterado_por, 195) || '_V782', left(enr.alterado_rf, 195) || '_V782'
  from origem_ranqueada o
  join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = o.questao_origem_id
                                        and not enr.excluido
                                        and pg_temp.v782_preenchida(enr.resposta_id, enr.arquivo_id, enr.texto)
 where o.posicao = 1;

-- 3c) Múltipla escolha / upload: acrescenta as opções e arquivos que a questão que fica não tem
--     (5 Checkbox, 6 Upload, 9 ComboMultiplaEscolha, 22 ComboMultiplaEscolhaMes, 25 ComboMultiplaEscolhaDinamico)
with candidata as (
    select distinct on (d.questao_destino_id, enr.resposta_id, enr.arquivo_id)
           d.questao_destino_id, enr.resposta_id, enr.arquivo_id, enr.texto,
           enr.criado_em, enr.criado_por, enr.criado_rf, enr.alterado_em, enr.alterado_por, enr.alterado_rf,
           coalesce(enr.alterado_em, enr.criado_em) as atividade
      from tmp_v782_destino d
      join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = d.questao_origem_id and not enr.excluido
     where d.tipo in (5, 6, 9, 22, 25)
       and (enr.resposta_id is not null or enr.arquivo_id is not null)
       and not exists (select 1
                         from encaminhamento_naapa_resposta atual
                        where atual.questao_encaminhamento_id = d.questao_destino_id
                          and not atual.excluido
                          and atual.resposta_id is not distinct from enr.resposta_id
                          and atual.arquivo_id is not distinct from enr.arquivo_id)
     order by d.questao_destino_id, enr.resposta_id, enr.arquivo_id, coalesce(enr.alterado_em, enr.criado_em) desc, enr.id desc
)
insert into encaminhamento_naapa_resposta (questao_encaminhamento_id, resposta_id, arquivo_id, texto, excluido, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select c.questao_destino_id, c.resposta_id, c.arquivo_id, c.texto, false, c.criado_em,
       left(coalesce(c.criado_por, ''), 195) || '_V782', left(coalesce(c.criado_rf, ''), 195) || '_V782',
       c.alterado_em, left(c.alterado_por, 195) || '_V782', left(c.alterado_rf, 195) || '_V782'
  from candidata c;

-- 3d) Endereço (15) e Turmas de programa (19) também são atualizados pela rotina do EOL, que grava só na primeira
--     cópia que encontra. Se uma cópia excluída tem valor mais novo que o da questão que fica, ele é copiado;
--     o passo 4b deixa só o mais recente.
with mais_recente_origem as (
    select distinct on (d.questao_destino_id)
           d.questao_destino_id, enr.resposta_id, enr.arquivo_id, enr.texto,
           enr.criado_em, enr.criado_por, enr.criado_rf, enr.alterado_em, enr.alterado_por, enr.alterado_rf,
           coalesce(enr.alterado_em, enr.criado_em) as atividade
      from tmp_v782_destino d
      join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = d.questao_origem_id
                                            and not enr.excluido
                                            and pg_temp.v782_preenchida(enr.resposta_id, enr.arquivo_id, enr.texto)
     where d.tipo in (15, 19)
     order by d.questao_destino_id, coalesce(enr.alterado_em, enr.criado_em) desc, enr.id desc
)
insert into encaminhamento_naapa_resposta (questao_encaminhamento_id, resposta_id, arquivo_id, texto, excluido, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
select m.questao_destino_id, m.resposta_id, m.arquivo_id, m.texto, false, m.criado_em,
       left(coalesce(m.criado_por, ''), 195) || '_V782', left(coalesce(m.criado_rf, ''), 195) || '_V782',
       m.alterado_em, left(m.alterado_por, 195) || '_V782', left(m.alterado_rf, 195) || '_V782'
  from mais_recente_origem m
 where not exists (select 1
                     from encaminhamento_naapa_resposta atual
                    where atual.questao_encaminhamento_id = m.questao_destino_id
                      and not atual.excluido
                      and pg_temp.v782_preenchida(atual.resposta_id, atual.arquivo_id, atual.texto)
                      and coalesce(atual.alterado_em, atual.criado_em) >= m.atividade);

-- Exclusão das respostas, questões e seções das cópias
update encaminhamento_naapa_resposta enr
   set excluido = true,
       alterado_por = left(coalesce(enr.alterado_por, ''), 195) || '_V782',
       alterado_rf = left(coalesce(enr.alterado_rf, ''), 195) || '_V782'
  from tmp_v782_questao_origem o
 where enr.questao_encaminhamento_id = o.id
   and not enr.excluido;

update encaminhamento_naapa_questao enq
   set excluido = true,
       alterado_por = left(coalesce(enq.alterado_por, ''), 195) || '_V782',
       alterado_rf = left(coalesce(enq.alterado_rf, ''), 195) || '_V782'
  from tmp_v782_questao_origem o
 where enq.id = o.id
   and not enq.excluido;

update encaminhamento_naapa_secao ens
   set excluido = true,
       alterado_por = left(coalesce(ens.alterado_por, ''), 195) || '_V782',
       alterado_rf = left(coalesce(ens.alterado_rf, ''), 195) || '_V782'
  from tmp_v782_secao_excluir s
 where ens.id = s.id
   and not ens.excluido;

-- 4) Limpeza dentro das questões que continuam ativas e têm mais de uma resposta ativa
with questao_multipla as (
    select enr.questao_encaminhamento_id
      from encaminhamento_naapa_resposta enr
     where not enr.excluido
     group by enr.questao_encaminhamento_id
    having count(*) > 1
),
resposta as (
    select enr.id,
           enr.questao_encaminhamento_id,
           enr.resposta_id,
           enr.arquivo_id,
           coalesce(enr.alterado_em, enr.criado_em) as atividade,
           q.tipo,
           pg_temp.v782_preenchida(enr.resposta_id, enr.arquivo_id, enr.texto) as preenchida
      from questao_multipla qmu
      join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = qmu.questao_encaminhamento_id
                                            and not enr.excluido
      join encaminhamento_naapa_questao enq on enq.id = enr.questao_encaminhamento_id and not enq.excluido
      join questao q on q.id = enq.questao_id
      join encaminhamento_naapa_secao ens on ens.id = enq.encaminhamento_naapa_secao_id and not ens.excluido
      join encaminhamento_naapa en on en.id = ens.encaminhamento_naapa_id and not en.excluido
),
avaliada as (
    select id,
           tipo,
           resposta_id,
           arquivo_id,
           preenchida,
           bool_or(preenchida) over (partition by questao_encaminhamento_id) as questao_tem_preenchida,
           row_number() over (partition by questao_encaminhamento_id
                              order by preenchida desc, atividade desc, id desc) as posicao_questao,
           row_number() over (partition by questao_encaminhamento_id, resposta_id, arquivo_id
                              order by atividade desc, id desc) as posicao_opcao
      from resposta
)
update encaminhamento_naapa_resposta enr
   set excluido = true,
       alterado_por = left(coalesce(enr.alterado_por, ''), 195) || '_V782',
       alterado_rf = left(coalesce(enr.alterado_rf, ''), 195) || '_V782'
  from avaliada a
 where a.id = enr.id
   and (   (not a.preenchida and a.questao_tem_preenchida)                                        -- 4c
        or (a.tipo in (3, 4, 14, 15, 19) and a.posicao_questao > 1)                               -- 4b
        or (a.preenchida and a.tipo not in (3, 4, 14, 15, 19)
            and (a.resposta_id is not null or a.arquivo_id is not null)
            and a.posicao_opcao > 1));                                                             -- 4a

drop table if exists pg_temp.tmp_v782_destino;
drop table if exists pg_temp.tmp_v782_questao_origem;
drop table if exists pg_temp.tmp_v782_questao_excluir;
drop table if exists pg_temp.tmp_v782_secao_excluir;
drop function if exists pg_temp.v782_preenchida(bigint, bigint, varchar);
drop function if exists pg_temp.v782_atividade_usuario(timestamp, varchar, timestamp, varchar);
