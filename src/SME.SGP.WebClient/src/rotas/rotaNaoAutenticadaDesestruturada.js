import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';

const RotaNaoAutenticadaDesestruturada = props => {
  const { component: Componente, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);

  return (
    <Route
      {...propriedades}
      render={propriedade =>
        !logado ? <Componente {...propriedade} /> : <Redirect to="/" />
      }
    />
  );
};

export default RotaNaoAutenticadaDesestruturada;
