import React, { memo } from 'react';
import t from 'prop-types';
import { Route, Redirect } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { setSomenteConsulta } from '~/redux/modulos/navegacao/actions';
import { Loader } from '~/componentes';

const RotaAutenticadaEstruturada = memo(
  ({
    component: Component,
    temPermissionamento,
    chavePermissao,
    location,
    ...propriedades
  }) => {
    const dispatch = useDispatch();
    const logado = useSelector(state => state.usuario.logado);
    const permissoes = useSelector(state => state.usuario.permissoes);
    const primeiroAcesso = useSelector(state => state.usuario.modificarSenha);
    const carregandoPerfil = useSelector(state => state.usuario.menu);
    const { loaderGeral } = useSelector(state => state.loader);

    dispatch(setSomenteConsulta(false));

    if (!logado) {
      return <Redirect to={`/login`} />;
    }

    if (primeiroAcesso) {
      return <Redirect to="/redefinir-senha" />;
    }

    if (temPermissionamento && !permissoes[chavePermissao]) {
      return <Redirect to="/sem-permissao" />;
    }

    return (
      <Loader loading={loaderGeral || !carregandoPerfil}>
        <Route {...propriedades} component={Component} />
      </Loader>
    );
  }
);

RotaAutenticadaEstruturada.propTypes = {
  component: t.oneOfType([t.any]),
  temPermissionamento: t.bool,
  chavePermissao: t.string,
  location: t.oneOfType([t.any]),
};

RotaAutenticadaEstruturada.defaultProps = {
  component: null,
  temPermissionamento: null,
  chavePermissao: null,
  location: null,
};

export default RotaAutenticadaEstruturada;
