import React from 'react';
import { Route } from 'react-router-dom';

const RotaMista = props => {
  const { component: Componente, ...propriedades } = props;

  return (
    <Route
      {...propriedades}
      render={propriedade => <Componente {...propriedade} />}
    />
  );
};

export default RotaMista;
