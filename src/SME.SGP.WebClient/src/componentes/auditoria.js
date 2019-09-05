import React from 'react';
import styled from 'styled-components';

const Container = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 9px;
  font-weight: bold;
  color: #42474a;
`;
const Auditoria = ({ inserido, alterado }) => {
  return (
    <Container>
      {inserido ? (
        <div className="col-xs-12 col-md-6 col-lg-12 d-flex justify-content-start mt-2">
          {inserido}
        </div>
      ) : (
        ''
      )}
      {alterado ? (
        <div className="col-xs-12 col-md-6 col-lg-12 d-flex justify-content-start mt-2">
          {alterado}
        </div>
      ) : (
        ''
      )}
    </Container>
  );
};

export default Auditoria;
