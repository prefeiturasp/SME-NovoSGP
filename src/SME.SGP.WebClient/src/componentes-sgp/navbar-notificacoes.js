import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Badge } from 'antd';
import styled from 'styled-components';
import shortid from 'shortid';
import { useSelector } from 'react-redux';
import { Base, Colors } from '../componentes/colors';
import Button from '../componentes/button';
import { store } from '~/redux';
import {
  notificacoesLista,
  naoLidas,
} from '../redux/modulos/notificacoes/actions';
import api from '../servicos/api';

const NavbarNotificacoes = props => {
  const { Botao, Icone, Texto } = props;

  const Count = styled(Badge)`
    color: ${Base.Branco} !important;
    ${props =>
      !props.count &&
      `
      i {
        background: ${Base.CinzaDesabilitado} !important;
        cursor: default !important;
      }
    `}
    sup {
      background: ${Base.VermelhoNotificacao} !important;
      display: flex !important;
      font-size: 9px !important;
      height: 18px !important;
      justify-content: center !important;
      min-width: 18px !important;
      width: 18px !important;
    }
  `;

  const Lista = styled.div`
    font-size: 9px !important;
    margin-top: 5px !important;
    min-width: 360px !important;
    right: 0 !important;
    z-index: 0 !important;
  `;

  const Tr = styled.tr`
    &:hover {
      cursor: pointer !important;
    }
    &:first-child {
      th,
      td {
        border-top: 0 none !important;
      }
    }
    td:first-child {
      color: ${Base.CinzaIconeNotificacao} !important;
    }
    th,
    td {
      border-color: ${Base.CinzaDesabilitado} !important;
      padding-bottom: 0.5rem !important;
      padding-bottom: 0.5rem !important;
      ${props =>
        props.status === 1 &&
        `
        background: ${Base.RoxoNotificacao} !important;
        font-weight: bold !important;
        &.status {
            color: ${Base.VermelhoNotificacao} !important;
            text-transform: uppercase !important;
        }`}
      &.w-75 {
        width: 160px !important;
      }
      &.w-25 {
        width: 50px !important;
      }
    }
  `;

  const [mostraNotificacoes, setMostraNotificacoes] = useState(false);
  const statusLista = ['', 'Não lida', 'Lida', 'Aceita', 'Recusada'];

  const usuario = useSelector(state => state.usuario);
  const notificacoes = useSelector(state => state.notificacoes);

  const buscaNotificacoesPorAnoRf = async (ano, rf) => {
    await api
      .get(`v1/notificacoes/resumo?anoLetivo=${ano}&usuarioRf=${rf}`)
      .then(res => {
        if (res.data) {
          store.dispatch(naoLidas(res.data.quantidadeNaoLidas));
          store.dispatch(notificacoesLista(res.data.notificacoes));
        }
      });
  };

  useEffect(() => {
    if (usuario.rf.length > 0)
      if (notificacoes.notificacoes.length === 0)
        buscaNotificacoesPorAnoRf(2019, usuario.rf);
  }, [usuario.rf]);

  const onClickBotao = () => {
    setMostraNotificacoes(!mostraNotificacoes);
  };

  return (
    <div className="position-relative">
      <Botao className="text-center" onClick={onClickBotao}>
        <Count count={notificacoes.quantidade} overflowCount={99}>
          <Icone className="fa fa-bell fa-lg" />
        </Count>
        <Texto
          className={`d-block mt-1 ${mostraNotificacoes &&
            notificacoes.quantidade > 0 &&
            'font-weight-bold'}`}
        >
          Notificações
        </Texto>
      </Botao>
      {mostraNotificacoes &&
        notificacoes.quantidade > 0 &&
        notificacoes.notificacoes.length > 0 && (
          <Lista className="container position-absolute rounded border bg-white shadow p-0">
            <table className="table mb-0">
              <tbody>
                {notificacoes.notificacoes.map(notificacao => {
                  return (
                    <Tr key={shortid.generate()} status={notificacao.status}>
                      <td className="py-1 pl-2 pr-1 text-center align-middle">
                        <i className="fa fa-info-circle" />
                      </td>
                      <th
                        className="py-1 px-1 text-center align-middle"
                        scope="row"
                      >
                        {notificacao.codigo}
                      </th>
                      <td className="py-1 px-1 align-middle w-75">
                        {notificacao.titulo}
                      </td>
                      <td className="py-1 px-1 text-center align-middle status">
                        {statusLista[notificacao.status]}
                      </td>
                      <td className="py-1 px-2 align-middle w-25 text-right">
                        {notificacao.data}
                      </td>
                    </Tr>
                  );
                })}
              </tbody>
            </table>
            <Button
              label="Ver tudo"
              className="btn-block"
              color={Colors.Roxo}
              fontSize="12px"
              customRadius="border-top-right-radius: 0 !important; border-top-left-radius: 0 !important;"
              border
              bold
            />
          </Lista>
        )}
    </div>
  );
};

NavbarNotificacoes.propTypes = {
  Botao: PropTypes.object.isRequired,
  Icone: PropTypes.object.isRequired,
  Texto: PropTypes.object.isRequired,
};

export default NavbarNotificacoes;
