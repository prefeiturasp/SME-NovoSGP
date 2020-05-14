import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import styled from 'styled-components';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';
import { URL_LOGIN } from '~/constantes/url';
import { store } from '~/redux';
import { limparDadosFiltro } from '~/redux/modulos/filtro/actions';
import { LimparSessao } from '~/redux/modulos/sessao/actions';
import {
  DeslogarSessaoExpirou,
  salvarLoginRevalidado,
} from '~/redux/modulos/usuario/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

import ContadorExpiracao from './contadorExpiracao';
import { setExibirMensagemSessaoExpirou } from '~/redux/modulos/mensagens/actions';

const Container = styled.div`
  margin-right: 10px;
  display: flex;
  justify-content: flex-end;
  margin-bottom: -49px !important;
  padding-top: 6px;

  .desc-tempo-expiracao-sessao {
    width: 295px;
    height: 47px;
    font-family: Roboto;
    font-size: 12px;
    font-weight: bold;
    letter-spacing: 0.1px;
    color: #b40c02;
    margin-top: 6px;
  }
`;

const CaixaTempoExpiracao = styled.div`
  z-index: 2000;
  display: grid;
  grid-template-columns: 30px 60px 40px;
  width: 138px;
  height: 47px;
  border-radius: 3px;
  background-color: ${Base.CinzaDesabilitado};

  .tempo-restante {
    margin-top: 10px;
    font-size: 21.2px;
    font-weight: bold;
    color: #42474a;
  }

  .botao-refresh {
    i {
      margin-left: 5px;
    }

    margin-top: 5px;
  }

  .icone-tempo {
    font-size: 21px;
    color: white;
    margin-top: 13px;
    margin-left: 6px;
  }
`;

const TempoExpiracaoSessao = () => {
  const dataHoraExpiracao = useSelector(e => e.usuario.dataHoraExpiracao);

  const [mostraTempoExpiracao, setMostraTempoExpiracao] = useState(false);
  const [tempoParaExpirar, setTempoParaExpirar] = useState({
    expiraEm: '',
    diferenca: '',
  });
  const [botaoDesabilitado, setBotaoDesabilitado] = useState(false);

  const calcularTempoExpiracao = useCallback(() => {
    const diferenca = +new Date(dataHoraExpiracao) - +new Date();
    if (diferenca > 0) {
      const quinzeMinutos = 900000;
      const tempoParaExibir =
        diferenca > quinzeMinutos ? diferenca - quinzeMinutos : 1;
      setTempoParaExpirar({
        expiraNaData: dataHoraExpiracao,
        tempoParaExibir,
        jaExpirou: diferenca < 1,
      });
    }
  }, [dataHoraExpiracao]);

  const deslogarDoUsuario = () => {
    store.dispatch(limparDadosFiltro());
    store.dispatch(DeslogarSessaoExpirou());
    store.dispatch(LimparSessao());
    history.push(URL_LOGIN);
  };

  useEffect(() => {
    if (dataHoraExpiracao) {
      calcularTempoExpiracao();
    }
  }, [calcularTempoExpiracao, dataHoraExpiracao]);

  useEffect(() => {
    if (tempoParaExpirar && tempoParaExpirar.expiraNaData) {
      if (tempoParaExpirar.tempoParaExibir > 0) {
        const timeOutExpiracao = setTimeout(() => {
          setMostraTempoExpiracao(true);
        }, tempoParaExpirar.tempoParaExibir);
        return () => clearTimeout(timeOutExpiracao);
      }
      if (tempoParaExpirar.jaExpirou) {
        deslogarDoUsuario();
      }
    }
    setMostraTempoExpiracao(false);
  }, [tempoParaExpirar]);

  const revalidarAutenticacao = async () => {
    setBotaoDesabilitado(true);

    const autenticado = await api
      .post('v1/autenticacao/revalidar')
      .catch(e => erros(e))
      .finally(() => setBotaoDesabilitado(false));

    setMostraTempoExpiracao(false);

    if (autenticado && autenticado.data && autenticado.data.token) {
      store.dispatch(
        salvarLoginRevalidado({
          token: autenticado.data.token,
          dataHoraExpiracao: autenticado.data.dataHoraExpiracao,
        })
      );
    }
  };

  return (
    <>
      {mostraTempoExpiracao ? (
        <Container>
          <div className="desc-tempo-expiracao-sessao">
            Sua sessão irá expirar em 15 minutos. Renove sua sessão aqui para
            não perder nenhum dado imputado.
          </div>
          <CaixaTempoExpiracao>
            <i className="far fa-clock icone-tempo" />
            <span className="tempo-restante">
              <ContadorExpiracao
                dataHoraExpiracao={dataHoraExpiracao}
                deslogarDoUsuario={deslogarDoUsuario}
              />
            </span>
            <Button
              id={shortid.generate()}
              icon="sync-alt"
              color={Colors.Azul}
              border
              className="botao-refresh"
              onClick={revalidarAutenticacao}
              disabled={botaoDesabilitado}
            />
          </CaixaTempoExpiracao>
        </Container>
      ) : (
        ''
      )}
    </>
  );
};

export default TempoExpiracaoSessao;
