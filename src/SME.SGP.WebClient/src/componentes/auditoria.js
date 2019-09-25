import React, { useState } from 'react';
import styled from 'styled-components';

const Container = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 9px;
  font-weight: bold;
  color: #42474a;
`;
const Auditoria = ({ criadoPor, criadoEm, alteradoPor, alteradoEm }) => {
  const [criado] = useState(
    criadoEm ? window.moment(criadoEm) : window.moment()
  );
  const [alterado] = useState(
    alteradoEm ? window.moment(alteradoEm) : window.moment()
  );

  return (
    <Container>
      {criadoPor ? (
        <div className="col-xs-12 col-md-6 col-lg-12 d-flex justify-content-start mt-2">
          INSERIDO por {criadoPor} em{' '}
          {`${criado.format('DD/MM/YYYY')} às ${criado.format('HH:mm')}`}
        </div>
      ) : (
        ''
      )}
      {alteradoPor ? (
        <div className="col-xs-12 col-md-6 col-lg-12 d-flex justify-content-start mt-2">
          ALTERADO por {alteradoPor} em{' '}
          {`${alterado.format('DD/MM/YYYY')} às ${alterado.format('HH:mm')}`}
        </div>
      ) : (
        ''
      )}
    </Container>
  );
};

export default Auditoria;
