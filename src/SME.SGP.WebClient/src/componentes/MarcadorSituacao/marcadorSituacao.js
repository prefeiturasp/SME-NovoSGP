import React from 'react';
import PropTypes from 'prop-types';

import { Colors } from '~/componentes/colors';

import { Container } from './marcadorSituacao.css';

const MarcadorSituacao = ({ children, corFundo, corTexto }) => (
  <Container corFundo={corFundo} corTexto={corTexto}>
    {children}
  </Container>
);

MarcadorSituacao.propTypes = {
  children: PropTypes.node,
  corFundo: PropTypes.string,
  corTexto: PropTypes.string,
};

MarcadorSituacao.defaultProps = {
  children: PropTypes.oneOfType([PropTypes.object, PropTypes.string]),
  corFundo: Colors.Roxo,
  corTexto: Colors.Branco,
};

export default MarcadorSituacao;
