import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Badge } from 'antd';
import styled from 'styled-components';
import shortid from 'shortid';
import { Base, Colors } from '../componentes/colors';
import Button from '../componentes/button';

const NavbarNotificacoes = props => {
  const { Botao, Icone, Texto } = props;

  const Count = styled(Badge)`
    color: ${Base.Branco} !important;
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
      cursor: pointer;
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
        !props.status &&
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

  // eslint-disable-next-line no-unused-vars
  const Aba = styled.div`
    background: ${Base.Branco} !important;
    border-left: 1px solid ${Base.CinzaDesabilitado} !important;
    border-top: 1px solid ${Base.CinzaDesabilitado} !important;
    display: block !important;
    height: 21px !important;
    position: absolute !important;
    top: -50% !important;
    transform: rotate(45deg) translateX(303px) translateY(-169px) !important;
    z-index: -1 !important;
    width: 21px !important;
  `;

  const [mostraNotificacoes, setMostraNotificacoes] = useState(false);
  const [notificacoes, setNotificacoes] = useState([]);

  const statusLista = ['Pendente', 'Aprovado'];

  useEffect(() => {
    setNotificacoes([
      {
        id: '0000010',
        status: 0,
        mensagem: 'Validação da frequencia da turma 4A',
        data: '15/08/2019 15:18',
      },
      {
        id: '0000009',
        status: 0,
        mensagem: 'Validação da frequencia da turma 4B',
        data: '15/08/2019 08:34',
      },
      {
        id: '0000008',
        status: 1,
        mensagem: 'Validação da frequencia da turma 4C',
        data: '14/08/2019 12:05',
      },
      {
        id: '0000007',
        status: 1,
        mensagem: 'Validação da frequencia da turma 4D',
        data: '14/08/2019 12:05',
      },
      {
        id: '0000006',
        status: 1,
        mensagem: 'Validação da frequencia da turma 4E',
        data: '14/08/2019 12:05',
      },
    ]);
  }, []);

  const onClickBotao = () => {
    setMostraNotificacoes(!mostraNotificacoes);
  };

  return (
    <div className="position-relative">
      <Botao className="text-center" onClick={onClickBotao}>
        <Count count={notificacoes.length} overflowCount={99}>
          <Icone className="fa fa-bell fa-lg" />
        </Count>
        <Texto
          className={`d-block mt-1 ${mostraNotificacoes && 'font-weight-bold'}`}
        >
          Notificações
        </Texto>
      </Botao>
      {mostraNotificacoes && notificacoes.length > 0 && (
        <Lista className="container position-absolute rounded border bg-white shadow p-0">
          <table className="table mb-0">
            <tbody>
              {notificacoes.map(notificacao => {
                return (
                  <Tr key={shortid.generate()} status={notificacao.status}>
                    <td className="py-1 pl-2 pr-1 text-center align-middle">
                      <i className="fa fa-info-circle" />
                    </td>
                    <th
                      className="py-1 px-1 text-center align-middle"
                      scope="row"
                    >
                      {notificacao.id}
                    </th>
                    <td className="py-1 px-1 align-middle w-75">
                      {notificacao.mensagem}
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
