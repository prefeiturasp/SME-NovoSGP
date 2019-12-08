import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';

const RotaNaoAutenticadaDesestruturada = props => {
  const { component: Componente, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);

  if (!logado) {
    return <Componente {...propriedades} />;
  }
  return <Redirect to="/" />;
};

export default RotaNaoAutenticadaDesestruturada;
