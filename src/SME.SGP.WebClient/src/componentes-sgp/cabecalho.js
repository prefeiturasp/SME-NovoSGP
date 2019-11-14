import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';

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
    margin-bottom: 0.3rem;
  }
`;

const Cabecalho = ({ titulo, pagina }) => {
  return (
    <Container>
      <div className="col-xs-12 col-md-12 col-lg-12 p-l-10">
        <span>{titulo}</span>
        <h3>{pagina}</h3>
      </div>
    </Container>
  );
};

Cabecalho.defaultProps = {
  titulo: '',
  pagina: '',
};

Cabecalho.propTypes = {
  titulo: PropTypes.string,
  pagina: PropTypes.string,
};
export default Cabecalho;
