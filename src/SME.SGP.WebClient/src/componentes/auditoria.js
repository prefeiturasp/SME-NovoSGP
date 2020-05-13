import React, { useState } from 'react';
import styled from 'styled-components';

const Auditoria = ({
  criadoPor,
  criadoEm,
  alteradoPor,
  alteradoEm,
  criadoRf,
  alteradoRf,
  ignorarMarginTop,
}) => {
  const [criado] = useState(
    criadoEm ? window.moment(criadoEm) : window.moment()
  );
  const [alterado] = useState(
    alteradoEm ? window.moment(alteradoEm) : window.moment()
  );

  const Container = styled.div`
    object-fit: contain;
    font-family: Roboto;
    font-size: 9px;
    font-weight: bold;
    color: #42474a;
    margin-top: ${ignorarMarginTop ? '0px' : '16px'};
    width: 100%;
  `;

  return (
    <Container>
      {criadoPor ? (
        <div
          className={`col-xs-12 col-md-12 col-lg-12 d-flex justify-content-start ${
            ignorarMarginTop ? '' : 'mt-2'
          }`}
        >
          INSERIDO por {criadoPor} {criadoRf && `(rf: ${criadoRf})`} em{' '}
          {`${criado.format('DD/MM/YYYY')} às ${criado.format('HH:mm')}`}
        </div>
      ) : (
        ''
      )}
      {alteradoPor ? (
        <div className="col-xs-12 col-md-12 col-lg-12 d-flex justify-content-start mt-2">
          ALTERADO por {alteradoPor} {alteradoRf && `(rf: ${alteradoRf})`} em{' '}
          {`${alterado.format('DD/MM/YYYY')}  às ${alterado.format('HH:mm')}`}
        </div>
      ) : (
        ''
      )}
    </Container>
  );
};

export default Auditoria;
