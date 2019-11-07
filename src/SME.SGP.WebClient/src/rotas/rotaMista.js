import React from 'react';
import { Route } from 'react-router-dom';
import { setSomenteConsulta } from '~/redux/modulos/navegacao/actions';
import { store } from '~/redux';

const RotaMista = props => {
  const { component: Componente, ...propriedades } = props;
  store.dispatch(setSomenteConsulta(false));

  return (
    <Route
      {...propriedades}
      render={propriedade => <Componente {...propriedade} />}
    />
  );
};

export default RotaMista;
