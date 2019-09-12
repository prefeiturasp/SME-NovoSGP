import React from 'react';
import styled from 'styled-components';

const Container = styled.div`
  span {
    object-fit: contain;
    font-family: Roboto;
    font-size: 12px;
    font-style: normal;
    font-stretch: normal;
    line-height: normal;
    letter-spacing: normal;
    font-weight: bold;
    letter-spacing: normal;
    color: #a4a4a4;
  }

  h3 {
    width: 137px;
    height: 29px;
    object-fit: contain;
    font-family: Roboto;
    font-size: 24px;
    font-weight: bold;
    font-style: normal;
    font-stretch: normal;
    line-height: normal;
    letter-spacing: normal;
    color: #353535;
  }
`;

const Cabecalho = ({ titulo, pagina }) => {
  return (
    <Container>
      <div className="col-xs-12 p-l-10">
        <span>{titulo || 'NOVO SGP'}</span>
        <h3>{pagina}</h3>
      </div>
    </Container>
  );
};
export default Cabecalho;
