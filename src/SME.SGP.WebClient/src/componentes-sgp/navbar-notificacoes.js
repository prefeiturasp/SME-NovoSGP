import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Badge } from 'antd';
import styled from 'styled-components';
import shortid from 'shortid';
import { Base } from '../componentes/colors';
import Button from '~/componentes/button';

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
    min-width: 360px;
    right: 0;
  `;

  const [mostraNotificacoes, setMostraNotificacoes] = useState(false);
  const [notificacoes, setNotificacoes] = useState([]);

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
        mensagem: 'Validação da frequencia da turma 5A',
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
        <Count count={15} overflowCount={99}>
          <Icone className="fa fa-bell fa-lg" />
        </Count>
        <Texto className="d-block mt-1">Notificações</Texto>
      </Botao>
      {mostraNotificacoes && (
        <Lista className="container position-absolute rounded border bg-white shadow p-0">
          {notificacoes.length > 0 && (
            <>
              <table className="table mb-0">
                <tbody>
                  {notificacoes.map((notificacao, indice) => {
                    return (
                      <tr key={shortid.generate()}>
                        <td className={`${indice === 0 && 'border-top-0'}`}>
                          <i className="fa fa-info-circle" />
                        </td>
                        <th
                          className={`${indice === 0 && 'border-top-0'}`}
                          scope="row"
                        >
                          {notificacao.id}
                        </th>
                        <td className={`${indice === 0 && 'border-top-0'}`}>
                          {notificacao.mensagem}
                        </td>
                        <td className={`${indice === 0 && 'border-top-0'}`}>
                          {notificacao.status}
                        </td>
                        <td className={`${indice === 0 && 'border-top-0'}`}>
                          {notificacao.data}
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
              <Button
                label="Ver tudo"
                className="block"
                color={Base.Roxo}
                border
              />
            </>
          )}
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
