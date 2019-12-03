import React, { useEffect } from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';
import Pagina from '~/componentes-sgp/conteudo';
import { setSomenteConsulta } from '~/redux/modulos/navegacao/actions';
import { store } from '~/redux';

const RotaAutenticadaEstruturada = props => {
  const { component: Component, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);
  const permissoes = useSelector(state => state.usuario.permissoes);
  const primeiroAcesso = useSelector(state => state.usuario.modificarSenha);
  store.dispatch(setSomenteConsulta(false));

  if (!logado) {
    return (
      <Redirect
        to={`/login/${btoa(props.location.pathname + props.location.search)}`}
      />
    );
  }
  if (primeiroAcesso) {
    return <Redirect to="/redefinir-senha" />;
  }
  if (props.temPermissionamento && !permissoes[props.chavePermissao]) {
    return <Redirect to={'/sem-permissao'} />;
  }

  return <Route {...propriedades} component={Component} />;
};

export default RotaAutenticadaEstruturada;
