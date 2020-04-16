import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';

const Container = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 9px;
  font-weight: bold;
  color: #42474a;
  margin-top: 16px;
  width: 100%;
`;

const AuditoriaAnotacaoRecomendacao = () => {
  const auditoriaAnotacaoRecomendacao = useSelector(
    store => store.conselhoClasse.auditoriaAnotacaoRecomendacao
  );

  const { criadoPor, criadoEm, criadoRF, alteradoPor, alteradoEm, alteradoRF } =
    auditoriaAnotacaoRecomendacao || {};

  const [criado, setCriado] = useState();
  const [alterado, setAlterado] = useState();

  useEffect(() => {
    if (auditoriaAnotacaoRecomendacao) {
      setCriado(criadoEm ? window.moment(criadoEm) : window.moment());
      setAlterado(alteradoEm ? window.moment(alteradoEm) : window.moment());
    }
  }, [alteradoEm, auditoriaAnotacaoRecomendacao, criadoEm]);

  return (
    <>
      {auditoriaAnotacaoRecomendacao ? (
        <Container>
          {criadoPor && criado ? (
            <div className="col-xs-12 col-md-12 col-lg-12 d-flex justify-content-start mt-2">
              INSERIDO por {criadoPor} {criadoRF && `(rf: ${criadoRF})`} em{' '}
              {`${criado.format('DD/MM/YYYY')} às ${criado.format('HH:mm')}`}
            </div>
          ) : (
            ''
          )}
          {alteradoPor && alterado ? (
            <div className="col-xs-12 col-md-12 col-lg-12 d-flex justify-content-start mt-2">
              ALTERADO por {alteradoPor} {alteradoRF && `(rf: ${alteradoRF})`}{' '}
              em{' '}
              {`${alterado.format('DD/MM/YYYY')}  às ${alterado.format(
                'HH:mm'
              )}`}
            </div>
          ) : (
            ''
          )}
        </Container>
      ) : (
        ''
      )}
    </>
  );
};

export default AuditoriaAnotacaoRecomendacao;
