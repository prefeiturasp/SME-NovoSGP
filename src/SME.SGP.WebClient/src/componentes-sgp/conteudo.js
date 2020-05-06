import React from 'react';
import { Switch } from 'react-router-dom';
import shortid from 'shortid';
import rotasArray from '~/rotas/rotas';
import RotaAutenticadaEstruturada from '../rotas/rotaAutenticadaEstruturada';
import BreadcrumbSgp from './breadcrumb-sgp';
import Mensagens from './mensagens/mensagens';
import ModalConfirmacao from './modalConfirmacao';
import TempoExpiracaoSessao from './tempoExpiracaoSessao/tempoExpiracaoSessao';

const Conteudo = () => {
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
            exact={rota.exact}
            temPermissionamento={rota.temPermissionamento}
            chavePermissao={rota.chavePermissao}
          />
        ))}
      </Switch>
    </div>
  );
};

export default Conteudo;
