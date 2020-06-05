import React from 'react';

// Redux
import { useSelector } from 'react-redux';

import { Switch } from 'react-router-dom';
import shortid from 'shortid';
import rotasArray from '~/rotas/rotas';
import RotaAutenticadaEstruturada from '../rotas/rotaAutenticadaEstruturada';
import BreadcrumbSgp from './breadcrumb-sgp';
import Mensagens from './mensagens/mensagens';
import ModalConfirmacao from './modalConfirmacao';
import TempoExpiracaoSessao from './tempoExpiracaoSessao/tempoExpiracaoSessao';

const Conteudo = () => {
  const { versao } = useSelector(store => store.sistema);
  return (
    <div className="secao-conteudo">
      <TempoExpiracaoSessao />
      <BreadcrumbSgp />
      <div className="row h-100">
        <main role="main" className="col-md-12 col-lg-12 col-sm-12 col-xl-12">
          <ModalConfirmacao />
          <Mensagens />
        </main>
      </div>
      <Switch>
        {rotasArray.map(rota => (
          <RotaAutenticadaEstruturada
            key={shortid.generate()}
            path={rota.path}
            component={rota.component}
            temPermissionamento={rota.temPermissionamento}
            chavePermissao={rota.chavePermissao}
            exact={rota.exact}
            temPermissionamento={rota.temPermissionamento}
            chavePermissao={rota.chavePermissao}
          />
        ))}
      </Switch>
      <div
        className="row"
        style={{ bottom: 0, position: 'relative', padding: '1rem 0.5rem' }}
      >
        <div className="col-md-12">
          {!versao ? '' : <strong>{versao}&nbsp;</strong>} - Sistema homologado
          para navegadores: Google Chrome e Firefox
        </div>
      </div>
    </div>
  );
};

export default Conteudo;
