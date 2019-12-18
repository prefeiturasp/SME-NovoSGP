import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';

const Container = styled.div`
  margin-top: -22px !important;
  margin-right: 10px;

  .desc-tempo-expiracao-sessao {
    width: 295px;
    height: 47px;
    font-family: Roboto;
    font-size: 12px;
    font-weight: bold;
    letter-spacing: 0.1px;
    color: #b40c02;
    position: relative;
    float: right;
    margin-top: 6px;
  }
`;

const CaixaTempoExpiracao = styled.div`
  display: grid;
  grid-template-columns: 30px 60px 40px;
  position: relative;
  float: right;
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
  const [mostraTempoExpiracao, setMostraTempoExpiracao] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setMostraTempoExpiracao(!mostraTempoExpiracao);
    }, 5000);
    return () => clearInterval(interval);
  }, [mostraTempoExpiracao]);

  return (
    <>
      {mostraTempoExpiracao ? (
        <Container>
          <CaixaTempoExpiracao>
            <i class="far fa-clock icone-tempo"></i>
            <span className="tempo-restante">15:00</span>
            <Button
              icon="sync-alt"
              color={Colors.Azul}
              border
              className="botao-refresh"
            />
          </CaixaTempoExpiracao>
          <div className="desc-tempo-expiracao-sessao">
            Sua sessão irá expirar em 15 minutos. Renove sua sessão aqui para
            não perder nenhum dado imputado.
          </div>
        </Container>
      ) : (
        ''
      )}
    </>
  );
};

export default TempoExpiracaoSessao;
