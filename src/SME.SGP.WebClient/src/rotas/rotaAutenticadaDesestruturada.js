import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';

const RotaAutenticadaDesestruturada = props => {
  const { component: Componente, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);
  const primeiroAcesso = useSelector(state => state.usuario.modificarSenha);

  if (!logado) {
    return (
      <Redirect
        to={`/login/${btoa(props.location.pathname + props.location.search)}`}
      />
    );
  }

  return (
    <Route
      {...propriedades}
      render={propriedade => <Componente {...propriedade} />}
    />
  );
};

export default RotaAutenticadaDesestruturada;
