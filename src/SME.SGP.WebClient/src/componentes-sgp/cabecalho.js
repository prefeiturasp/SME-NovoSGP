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

  .titulo {
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

const Cabecalho = ({ titulo, pagina, children, classes }) => {
  return (
    <Container className={classes}>
      <div className="col-xs-12 col-md-12 col-lg-12 p-l-10">
        <span>{titulo}</span>
        <span className="titulo">{pagina}</span>
        {children}
      </div>
    </Container>
  );
};

Cabecalho.defaultProps = {
  titulo: '',
  pagina: '',
  children: '',
  classes: '',
};

Cabecalho.propTypes = {
  titulo: PropTypes.string,
  pagina: PropTypes.string,
  children: PropTypes.oneOfType([PropTypes.node, PropTypes.string]),
  classes: PropTypes.string,
};
export default Cabecalho;
