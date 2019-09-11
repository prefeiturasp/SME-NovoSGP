import React from 'react';
import styled from 'styled-components';

const Container = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 9px;
  font-weight: bold;
  color: #42474a;
`;
const Auditoria = ({ criadoPor, criadoEm, alteradoPor, alteradoEm }) => {
  return (
    <Container>
      {criadoPor ? (
        <div className="col-xs-12 col-md-6 col-lg-12 d-flex justify-content-start mt-2">
          INSERIDO por {criadoPor} em {window.moment(criadoEm).format('LLL')}
        </div>
      ) : (
        ''
      )}
      {alteradoPor ? (
        <div className="col-xs-12 col-md-6 col-lg-12 d-flex justify-content-start mt-2">
          ALTERADO por {alteradoPor} em{' '}
          {window.moment(alteradoEm).format('LLL')}
        </div>
      ) : (
        ''
      )}
    </Container>
  );
};

export default Auditoria;
