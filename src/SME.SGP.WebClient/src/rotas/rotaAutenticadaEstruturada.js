import React from 'react';
import Pagina from '~/componentes-sgp/conteudo';
import { Route } from 'react-router-dom';

const RotaAutenticadaEstruturada = props => {
  const { component: Componente, ...propriedades } = props;

  return (
    <Route
      {...propriedades}
      render={propriedade => {
        return (
          <Pagina>
            <Componente {...propriedade} />
          </Pagina>
        );
      }}
    />
  );
};

export default RotaAutenticadaEstruturada;
