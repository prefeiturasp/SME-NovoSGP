import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';

const RotaAutenticadaDesestruturada = props => {
  const { component: Componente, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);
  const primeiroAcesso = useSelector(state => state.usuario.modificarSenha);

  return (
    <Route
      {...propriedades}
      render={propriedade =>
        logado ? (
          primeiroAcesso ? (
            <Redirect to="/redefinir-senha" />
          ) : (
            <Componente {...propriedade} />
          )
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

export default RotaAutenticadaDesestruturada;
