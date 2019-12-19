import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import styled from 'styled-components';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';
import { salvarLoginRevalidado } from '~/redux/modulos/usuario/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';

import ContadorExpiracao from './contadorExpiracao';

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
  const dispatch = useDispatch();

  const [mostraTempoExpiracao, setMostraTempoExpiracao] = useState(true);

  // useEffect(() => {
  //   const interval = setInterval(() => {
  //     setMostraTempoExpiracao(!mostraTempoExpiracao);
  //   }, 5000);
  //   return () => clearInterval(interval);
  // }, [mostraTempoExpiracao]);

  const revalidarAutenticacao = async () => {
    const autenticado = await api
      .post('v1/autenticacao/revalidar')
      .catch(e => erros(e));
    dispatch(
      salvarLoginRevalidado({
        token: autenticado.data,
      })
    );
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
              <ContadorExpiracao />
            </span>
            <Button
              icon="sync-alt"
              color={Colors.Azul}
              border
              className="botao-refresh"
              onClick={revalidarAutenticacao}
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
