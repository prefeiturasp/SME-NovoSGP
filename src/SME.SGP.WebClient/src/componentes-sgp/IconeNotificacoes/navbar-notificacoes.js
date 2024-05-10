import React, { useState, useEffect, useLayoutEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import { useSelector } from 'react-redux';
import * as moment from 'moment';
import { Colors } from '~/componentes/colors';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import servicoNotificacao from '~/servicos/Paginas/ServicoNotificacao';
import { erros } from '~/servicos/alertas';
import { Tr, Lista, Count } from './navbar-notificacoes.css';

const NavbarNotificacoes = props => {
  const { Botao, Icone, Texto } = props;

  const listaRef = useRef();

  const [mostraNotificacoes, setMostraNotificacoes] = useState(false);
  const statusLista = ['', 'Não lida', 'Lida', 'Aceita', 'Recusada'];

  const notificacoes = useSelector(state => state.notificacoes);
  const { loaderGeral } = useSelector(state => state.loader);

  useEffect(() => {
    const interval = setInterval(() => {
      if (!loaderGeral) {
        servicoNotificacao
          .obterQuantidadeNotificacoesNaoLidas()
          .catch(e => erros(e));
      }
    }, 60000);
    return () => clearInterval(interval);
  }, [loaderGeral]);

  useLayoutEffect(() => {
    const handleClickFora = event => {
      if (listaRef.current && !listaRef.current.contains(event.target)) {
        setMostraNotificacoes(!mostraNotificacoes);
      }
    };
    if (mostraNotificacoes) document.addEventListener('click', handleClickFora);
    else document.removeEventListener('click', handleClickFora);
  }, [mostraNotificacoes]);

  useEffect(() => {
    if (mostraNotificacoes) {
      servicoNotificacao
        .obterUltimasNotificacoesNaoLidas()
        .catch(e => erros(e));
    }
  }, [mostraNotificacoes]);

  const onClickBotao = () => {
    setMostraNotificacoes(antigo => !antigo);
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
      {mostraNotificacoes && notificacoes.notificacoes.length > 0 && (
        <Lista className="container position-absolute rounded border bg-white shadow p-0">
          <table className="table mb-0">
            <tbody>
              {notificacoes.notificacoes.map(notificacao => {
                return (
                  <Tr
                    key={shortid.generate()}
                    status={notificacao.status}
                    onClick={() => onClickNotificacao(notificacao.id)}
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
                      {moment(notificacao.data).format('DD/MM/YYYY HH:mm:ss')}
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
