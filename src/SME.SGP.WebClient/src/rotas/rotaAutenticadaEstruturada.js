import React from 'react';
import Pagina from '~/componentes-sgp/conteudo';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';

const RotaAutenticadaEstruturada = props => {
  const { component: Componente, ...propriedades } = props;

  const logado = useSelector(state => state.usuario.logado);

  return (
    <Route
      {...propriedades}
      render={propriedade =>
        logado ? (
          <Pagina>
            <Componente {...propriedade} />
          </Pagina>
        ) : (
          <Redirect
            to={`/Login/${btoa(
              props.location.pathname + props.location.search
            )}`}
          />
        )
      }
    />
  );
};

export default RotaAutenticadaEstruturada;
