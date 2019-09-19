import React from 'react';
import { Route } from 'react-router-dom';

const RotaNaoAutenticadaDesestruturada = props => {
  const { component: Componente, ...propriedades } = props;

  return (
    <Route
      {...propriedades}
      render={propriedade => {
        return <Componente {...propriedade} />;
      }}
    />
  );
};

export default RotaNaoAutenticadaDesestruturada;
