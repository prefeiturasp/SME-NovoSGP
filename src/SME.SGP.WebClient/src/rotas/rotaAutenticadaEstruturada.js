import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';
import Pagina from '~/componentes-sgp/conteudo';
import { setSomenteConsulta } from '~/redux/modulos/navegacao/actions';
import { store } from '~/redux';

const RotaAutenticadaEstruturada = props => {
  const { component: Componente, ...propriedades } = props;
  const logado = useSelector(state => state.usuario.logado);
  const permissoes = useSelector(state => state.usuario.permissoes);
  const primeiroAcesso = useSelector(state => state.usuario.modificarSenha);
  store.dispatch(setSomenteConsulta(false));

  return (
    <Route
      {...propriedades}
      render={propriedade =>
        logado ? (
          primeiroAcesso ? (
            <Redirect to="/redefinir-senha" />
          ) : (
              !props.temPermissionamento || (props.temPermissionamento && permissoes[props.chavePermissao]) ?
                <Pagina>
                  <Componente {...propriedade} />
                </Pagina>
                :
                <Redirect
                  to={'/sem-permissao'}
                />
            )
        ) : (
            <Redirect
              to={`/login/${btoa(
                props.location.pathname + props.location.search
              )}`}
            />
          )
      }
    />
  );
};

export default RotaAutenticadaEstruturada;
