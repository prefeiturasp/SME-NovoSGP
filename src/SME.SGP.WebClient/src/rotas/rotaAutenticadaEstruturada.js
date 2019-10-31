import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useSelector } from 'react-redux';
import Pagina from '~/componentes-sgp/conteudo';
import { getObjetoStorageUsuario } from '~/servicos/servico-navegacao'

const RotaAutenticadaEstruturada = props => {
  const { component: Componente, ...propriedades } = props;
  let logado = useSelector(state => state.usuario.logado);
  let permissoes = useSelector(state => state.usuario.permissoes);
  const logadoStorage = getObjetoStorageUsuario('logado');
  const permissoesStorage = getObjetoStorageUsuario('permissoes');
  const primeiroAcesso = useSelector(state => state.usuario.modificarSenha);
  logado = logado ? logado : logadoStorage;
  permissoes = permissoes && permissoes.length>0 ? permissoes : permissoesStorage;

  return (
    <Route
      {...propriedades}
      render={propriedade =>
        logado ? (
          primeiroAcesso ? (
            <Redirect to="/redefinir-senha" />
          ) : (
              !props.temPermissionamento || (props.temPermissionamento && permissoes[props.path]) ?
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
