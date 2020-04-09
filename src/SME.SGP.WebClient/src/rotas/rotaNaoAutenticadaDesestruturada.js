import React from 'react';
import t from 'prop-types';

import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';

const RotaNaoAutenticadaDesestruturada = props => {
  const { component: Componente, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);

  if (!logado) {
    return <Route component={Componente} {...propriedades} />;
  }
  return <Redirect to="/" />;
};

RotaNaoAutenticadaDesestruturada.propTypes = {
  propriedades: t.oneOfType([t.any]),
  component: t.oneOfType([t.any]),
};

RotaNaoAutenticadaDesestruturada.defaultProps = {
  propriedades: {},
  component: () => {},
};

export default RotaNaoAutenticadaDesestruturada;
