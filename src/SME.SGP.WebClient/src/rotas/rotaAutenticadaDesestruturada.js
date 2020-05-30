import React from 'react';
import t from 'prop-types';

import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';

const RotaAutenticadaDesestruturada = props => {
  const { component: Componente, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);
  const primeiroAcesso = useSelector(state => state.usuario.modificarSenha);

  if (!logado) {
    return <Redirect to={`/login`} />;
  }

  return (
    <Route
      {...propriedades}
      render={propriedade => <Componente {...propriedade} />}
    />
  );
};

RotaAutenticadaDesestruturada.propTypes = {
  propriedades: t.oneOfType([t.any]),
  props: t.oneOfType([t.any]),
  component: t.oneOfType([t.any]),
};

RotaAutenticadaDesestruturada.defaultProps = {
  propriedades: {},
  props: {},
  component: () => {},
};

export default RotaAutenticadaDesestruturada;
