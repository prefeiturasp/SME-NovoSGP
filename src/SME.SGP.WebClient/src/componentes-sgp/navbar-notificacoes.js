import React, { useState, useEffect, useLayoutEffect, useRef } from 'react';
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
import history from '~/servicos/history';

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
    cursor: pointer !important;
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

  const listaRef = useRef();

  const [mostraNotificacoes, setMostraNotificacoes] = useState(false);
  const statusLista = ['', 'Não lida', 'Lida', 'Aceita', 'Recusada'];

  const usuario = useSelector(state => state.usuario);
  const notificacoes = useSelector(state => state.notificacoes);

  const buscaNotificacoesPorAnoRf = async (ano, rf) => {
    const res = {
      data: {
        notificacoes: [
          {
            categoria: 3,
            codigo: '000000007',
            data: '16/09/2019 11:32:11',
            descricaoStatus: 'Teste 123',
            id: 7,
            status: 1,
            tipo: 'Notas',
            titulo: 'Você tem um teste',
          },
          {
            categoria: 3,
            codigo: '000000005',
            data: '16/09/2019 11:32:04',
            descricaoStatus: 'Teste 123',
            id: 5,
            status: 1,
            tipo: 'Fechamento',
            titulo: 'Você tem um teste',
          },
          {
            categoria: 1,
            codigo: '000000001',
            data: '16/09/2019 11:16:07',
            descricaoStatus: 'Teste 123',
            id: 1,
            status: 1,
            tipo: 'Calendario',
            titulo: 'Você tem um teste',
          },
          {
            categoria: 1,
            codigo: '000000014',
            data: '16/09/2019 11:32:48',
            descricaoStatus: 'Teste 123',
            id: 14,
            status: 2,
            tipo: 'Sondagem',
            titulo: 'Você tem um teste',
          },
          {
            categoria: 2,
            codigo: '000000013',
            data: '16/09/2019 11:32:40',
            descricaoStatus: 'Teste 123',
            id: 13,
            status: 2,
            tipo: 'PlanoDeAula',
            titulo: 'Você tem um teste',
          },
        ],
        quantidadeNaoLidas: 3,
      },
    };
    // await api
    // .get(`v1/notificacoes/resumo?anoLetivo=${ano}&usuarioRf=${rf}`)
    // .then(res => {
    // if (res.data) {
    store.dispatch(naoLidas(res.data.quantidadeNaoLidas));
    store.dispatch(notificacoesLista(res.data.notificacoes));
    // }
    // });
  };

  useEffect(() => {
    if (usuario.rf.length > 0)
      if (notificacoes.notificacoes.length === 0)
        buscaNotificacoesPorAnoRf(2019, usuario.rf);
  }, [usuario.rf]);

  useLayoutEffect(() => {
    if (mostraNotificacoes) document.addEventListener('click', handleClickFora);
    else document.removeEventListener('click', handleClickFora);
  }, [mostraNotificacoes]);

  const onClickBotao = () => {
    setMostraNotificacoes(!mostraNotificacoes);
  };

  const onClickNotificacao = codigo => {
    if (codigo) {
      history.push(`/notificacoes/${codigo}`);
      setMostraNotificacoes(!mostraNotificacoes);
    }
  };

  const onClickVerTudo = () => {
    history.push(`/notificacoes`);
    setMostraNotificacoes(!mostraNotificacoes);
  };

  const handleClickFora = event => {
    if (listaRef.current && !listaRef.current.contains(event.target)) {
      setMostraNotificacoes(!mostraNotificacoes);
    }
  };

  return (
    <div ref={listaRef} className="position-relative">
      <Botao className="text-center stretched-link" onClick={onClickBotao}>
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
                    <Tr
                      key={shortid.generate()}
                      status={notificacao.status}
                      onClick={() => onClickNotificacao(notificacao.codigo)}
                    >
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
              onClick={onClickVerTudo}
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
