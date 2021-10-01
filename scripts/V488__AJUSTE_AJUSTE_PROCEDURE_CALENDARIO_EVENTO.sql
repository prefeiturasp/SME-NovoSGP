create or replace view v_estrutura_eventos_listar
            (eventoid, nome, descricaoevento, data_inicio, data_fim, dre_id, letivo, feriado_id, tipo_calendario_id,
             tipo_evento_id, ue_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, status,
             tipoeventoid, ativo, tipo_data, descricaotipoevento, excluido, total_registros)
as
SELECT e.id         AS eventoid,
       e.nome,
       e.descricao  AS descricaoevento,
       e.data_inicio,
       e.data_fim,
       e.dre_id,
       e.letivo,
       e.feriado_id,
       e.tipo_calendario_id,
       e.tipo_evento_id,
       e.ue_id,
       e.criado_em,
       e.criado_por,
       e.alterado_em,
       e.alterado_por,
       e.criado_rf,
       e.alterado_rf,
       e.status,
       et.id        AS tipoeventoid,
       et.ativo,
       et.tipo_data,
       et.descricao AS descricaotipoevento,
       et.excluido,
       0            AS total_registros
FROM evento e
         JOIN evento_tipo et ON e.tipo_evento_id = et.id;

alter table v_estrutura_eventos_listar
    owner to postgres;

create or replace view v_estrutura_eventos
            (eventoid, nome, descricaoevento, data_inicio, data_fim, dre_id, letivo, feriado_id, tipo_calendario_id,
             tipo_evento_id, ue_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, status,
             tipoeventoid, ativo, tipo_data, descricaotipoevento, excluido, local_ocorrencia)
as
SELECT e.id         AS eventoid,
       e.nome,
       e.descricao  AS descricaoevento,
       e.data_inicio,
       e.data_fim,
       e.dre_id,
       e.letivo,
       e.feriado_id,
       e.tipo_calendario_id,
       e.tipo_evento_id,
       e.ue_id,
       e.criado_em,
       e.criado_por,
       e.alterado_em,
       e.alterado_por,
       e.criado_rf,
       e.alterado_rf,
       e.status,
       et.id        AS tipoeventoid,
       et.ativo,
       et.tipo_data,
       et.descricao AS descricaotipoevento,
       et.excluido,
       et.local_ocorrencia
FROM evento e
         JOIN evento_tipo et ON e.tipo_evento_id = et.id;

alter table v_estrutura_eventos
    owner to postgres;

create or replace function f_eventos_por_rf_criador(p_login character varying, p_tipo_calendario_id bigint,
                                                    p_dre_id character varying DEFAULT NULL::character varying,
                                                    p_ue_id character varying DEFAULT NULL::character varying,
                                                    p_data_inicio date DEFAULT NULL::date,
                                                    p_data_fim date DEFAULT NULL::date,
                                                    p_eh_perfil_sme boolean DEFAULT false,
                                                    p_eh_perfil_dre boolean DEFAULT false,
                                                    p_eh_perfil_ue boolean DEFAULT false,
                                                    p_eh_eventos_toda_rede boolean DEFAULT false) returns SETOF v_estrutura_eventos
    language sql
as
$$
select e.id,
       e.nome,
       e.descricao,
       e.data_inicio,
       e.data_fim,
      
       e.letivo,
       e.feriado_id,
       e.tipo_calendario_id,
       e.tipo_evento_id,       
       e.criado_em,
       e.criado_por,
       e.alterado_em,
       e.alterado_por,
       e.criado_rf,
       e.alterado_rf,
       e.status,
       et.id,
       et.ativo,
       et.tipo_data,
       et.descricao,
       et.excluido,
       et.local_ocorrencia,
       dre.dre_id,
       coalesce(dre.abreviacao, 'TODAS') as DreNome,
       ue.ue_id,
       coalesce(ue.nome, 'TODAS') as UeNome
from evento e
         inner join evento_tipo et
                    on e.tipo_evento_id = et.id
         inner join tipo_calendario tc
                    on e.tipo_calendario_id = tc.id
         left join dre
                   on e.dre_id = dre.dre_id
         left join ue
                   on e.ue_id = ue.ue_id
where not et.excluido
    and not e.excluido
    -- considera somente pendente de aprovao
    and e.status = 2
    and e.criado_rf = p_login
    and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo)
    and e.tipo_calendario_id = p_tipo_calendario_id
    and (p_dre_id is null or (p_dre_id is not null and e.dre_id = p_dre_id))
    and (p_ue_id is null or (p_ue_id is not null and e.ue_id = p_ue_id))
    and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
    and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)))
  and (
        (p_eh_perfil_sme
            and (
                 (p_eh_eventos_toda_rede and e.dre_id is null and e.ue_id is null)
                 or
                 (not p_eh_eventos_toda_rede
                     and ((p_dre_id is null)
                         or (p_ue_id is null and e.dre_id = p_dre_id)
                         or (p_ue_id is not null and e.dre_id = p_dre_id and e.ue_id = p_ue_id))))
            )
        or
        (p_eh_perfil_dre
            and ((p_ue_id is null and e.dre_id is null)
                or (p_dre_id = e.dre_id and p_ue_id = e.ue_id))
            )
        or
        (p_eh_perfil_ue
            and (e.dre_id is null or e.ue_id = p_ue_id))
        )
$$;

alter function f_eventos_por_rf_criador(varchar, bigint, varchar, varchar, date, date, boolean, boolean, boolean, boolean) owner to postgres;


create or replace function f_eventos(p_login character varying, p_perfil_id uuid, p_historico boolean,
                                     p_tipo_calendario_id bigint, p_considera_pendente_aprovacao boolean DEFAULT false,
                                     p_desconsidera_local_dre boolean DEFAULT false,
                                     p_dre_id character varying DEFAULT NULL::character varying,
                                     p_ue_id character varying DEFAULT NULL::character varying,
                                     p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false,
                                     p_data_inicio date DEFAULT NULL::date, p_data_fim date DEFAULT NULL::date,
                                     p_eh_perfil_sme boolean DEFAULT false, p_eh_perfil_dre boolean DEFAULT false,
                                     p_eh_perfil_ue boolean DEFAULT false,
                                     p_eh_eventos_toda_rede boolean DEFAULT false) returns SETOF v_estrutura_eventos
    language sql
as
$$
select e.id,
       e.nome,
       e.descricao,
       e.data_inicio,
       e.data_fim,
       e.dre_id,
       e.letivo,
       e.feriado_id,
       e.tipo_calendario_id,
       e.tipo_evento_id,
       e.ue_id,
       e.criado_em,
       e.criado_por,
       e.alterado_em,
       e.alterado_por,
       e.criado_rf,
       e.alterado_rf,
       e.status,
       et.id,
       et.ativo,
       et.tipo_data,
       et.descricao,
       et.excluido,
       et.local_ocorrencia
from evento e
         inner join evento_tipo et
                    on e.tipo_evento_id = et.id
         inner join tipo_calendario tc
                    on e.tipo_calendario_id = tc.id
         left join f_abrangencia_dres(p_login, p_perfil_id, p_historico) ad
                   on e.dre_id = ad.codigo
                       -- modalidade 1 (fundamental/medio) do tipo de calendario, considera as modalidades 5 (Fundamental) e 6 (medio)
                       -- modalidade 2 (EJA) do tipo de calendario, considera modalidade 3 (EJA)
                       and ((tc.modalidade = 1 and ad.modalidade_codigo in (5, 6)) or
                            (tc.modalidade = 2 and ad.modalidade_codigo = 3))
                       -- para DREs considera local da ocorrencia 2 (DRE) e 5 (Todos)
                       and et.local_ocorrencia in (2, 3, 5)
         left join f_abrangencia_ues(p_login, p_perfil_id, p_historico) au
                   on e.ue_id = au.codigo
                       and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) or
                            (tc.modalidade = 2 and au.modalidade_codigo = 3))
                       -- para UEs considera local da ocorrencia 1 (UE) e 4 (SME/UE) e 5 (Todos)
                       and et.local_ocorrencia in (1, 3, 4, 5)
where not e.excluido
  and not et.excluido
  and (extract(year from e.data_inicio) = tc.ano_letivo or extract(year from e.data_fim) = tc.ano_letivo)
  and e.tipo_calendario_id = p_tipo_calendario_id

  and (
        (p_eh_perfil_sme
            and (
                (p_eh_eventos_toda_rede and e.dre_id is null and e.ue_id is null)
                or
                (not p_eh_eventos_toda_rede
                    and ((p_dre_id is null)
                or (p_ue_id is null and e.dre_id = p_dre_id)
                or (p_ue_id is not null and e.dre_id = p_dre_id and e.ue_id = p_ue_id))))
            )
        or
        (p_eh_perfil_dre
            and ((p_ue_id is null and e.dre_id is null)
                or (p_dre_id = e.dre_id and p_ue_id = e.ue_id))
            )
        or
        (p_eh_perfil_ue
            and (e.dre_id is null
                or e.ue_id = p_ue_id
                or (p_perfil_id in ('46e1e074-37d6-e911-abd6-f81654fe895d', '45e1e074-37d6-e911-abd6-f81654fe895d',
                                    '44e1e074-37d6-e911-abd6-f81654fe895d')
                    and p_dre_id = e.dre_id
                    and e.ue_id is null))
            )
    )
  -- caso considere 1 (aprovado) e 2 (pendente de aprovacao), senao considera so aprovados
  and ((p_considera_pendente_aprovacao = true and e.status in (1, 2)) or
       (p_considera_pendente_aprovacao = false and e.status = 1))
  and (p_desconsidera_local_dre = false or (p_desconsidera_local_dre = true and et.local_ocorrencia != 2))
  and ((p_dre_id is null and ((e.dre_id is null and e.ue_id is null) or e.dre_id in (select codigo
                                                                                     from f_abrangencia_dres(p_login, p_perfil_id, p_historico)))) or
       (p_dre_id is not null and ((e.dre_id is null and e.ue_id is null) or e.dre_id = p_dre_id)))
  and ((p_ue_id is null and
        (e.ue_id is null or e.ue_id in (select codigo from f_abrangencia_ues(p_login, p_perfil_id, p_historico)))) or
       (p_ue_id is not null and (e.ue_id is null or e.ue_id = p_ue_id)))
  -- caso desconsidere 6 (liberacao excepcional) e 15 (reposicao de recesso)
  and (p_desconsidera_liberacao_excep_reposicao_recesso = true or
       (p_desconsidera_liberacao_excep_reposicao_recesso = false and et.codigo not in (6, 15)))
  and (p_data_inicio is null or (p_data_inicio is not null and date(e.data_inicio) >= date(p_data_inicio)))
  and (p_data_fim is null or (p_data_fim is not null and date(e.data_fim) <= date(p_data_fim)));
$$;

alter function f_eventos(varchar, uuid, boolean, bigint, boolean, boolean, varchar, varchar, boolean, date, date, boolean,boolean,boolean, boolean) owner to postgres;


create or replace function f_eventos_listar_sem_paginacao(p_login character varying, p_perfil_id uuid,
                                                          p_historico boolean, p_tipo_calendario_id bigint,
                                                          p_considera_pendente_aprovacao boolean DEFAULT false,
                                                          p_desconsidera_local_dre boolean DEFAULT false,
                                                          p_dre_id character varying DEFAULT NULL::character varying,
                                                          p_ue_id character varying DEFAULT NULL::character varying,
                                                          p_desconsidera_liberacao_excep_reposicao_recesso boolean DEFAULT false,
                                                          p_data_inicio date DEFAULT NULL::date,
                                                          p_data_fim date DEFAULT NULL::date,
                                                          p_tipo_evento_id bigint DEFAULT NULL::bigint,
                                                          p_nome_evento character varying DEFAULT NULL::character varying,
                                                          p_eh_perfil_sme boolean DEFAULT false,
                                                          p_eh_perfil_dre boolean DEFAULT false,
                                                          p_eh_perfil_ue boolean DEFAULT false,
                                                          p_eh_eventos_toda_rede boolean DEFAULT false) returns SETOF v_estrutura_eventos_listar
    language sql
as
$$
select eventoid,
       nome,
       descricaoevento,
       data_inicio,
       data_fim,
       dre_id,
       letivo,
       feriado_id,
       tipo_calendario_id,
       tipo_evento_id,
       ue_id,
       criado_em,
       criado_por,
       alterado_em,
       alterado_por,
       criado_rf,
       alterado_rf,
       status,
       tipoeventoid,
       ativo,
       tipo_data,
       descricaotipoevento,
       excluido,
       int4(0) total_registros
from (
         select eventoid,
                nome,
                descricaoevento,
                data_inicio,
                data_fim,
                dre_id,
                letivo,
                feriado_id,
                tipo_calendario_id,
                tipo_evento_id,
                ue_id,
                criado_em,
                criado_por,
                alterado_em,
                alterado_por,
                criado_rf,
                alterado_rf,
                status,
                tipoeventoid,
                ativo,
                tipo_data,
                descricaotipoevento,
                excluido
         from f_eventos(p_login, p_perfil_id, p_historico, p_tipo_calendario_id, p_considera_pendente_aprovacao,
                        p_desconsidera_local_dre, p_dre_id, p_ue_id, p_desconsidera_liberacao_excep_reposicao_recesso,
                        p_data_inicio, p_data_fim, p_eh_perfil_sme, p_eh_perfil_dre, p_eh_perfil_ue, p_eh_eventos_toda_rede)

         union

         select eventoid,
                nome,
                descricaoevento,
                data_inicio,
                data_fim,
                dre_id,
                letivo,
                feriado_id,
                tipo_calendario_id,
                tipo_evento_id,
                ue_id,
                criado_em,
                criado_por,
                alterado_em,
                alterado_por,
                criado_rf,
                alterado_rf,
                status,
                tipoeventoid,
                ativo,
                tipo_data,
                descricaotipoevento,
                excluido
         from f_eventos_por_rf_criador(p_login, p_tipo_calendario_id, p_dre_id, p_ue_id, p_data_inicio, p_data_fim,
                                       p_eh_perfil_sme, p_eh_perfil_dre, p_eh_perfil_ue, p_eh_eventos_toda_rede)) lista
where (p_tipo_evento_id is null or (p_tipo_evento_id is not null and tipo_evento_id = p_tipo_evento_id))
  and (p_nome_evento is null or (p_nome_evento is not null and upper(nome) like upper('%' || p_nome_evento || '%')));
$$;

alter function f_eventos_listar_sem_paginacao(varchar, uuid, boolean, bigint, boolean, boolean, varchar, varchar, boolean, date, date, bigint, varchar, boolean, boolean, boolean, boolean) owner to postgres;

