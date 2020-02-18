import React from 'react';
import { useSelector } from 'react-redux';
import { Switch } from 'react-router-dom';
import shortid from 'shortid';
import rotasArray from '~/rotas/rotas';
import Editor from '~/componentes/editor/editor';
import RotaAutenticadaEstruturada from '../rotas/rotaAutenticadaEstruturada';
import BreadcrumbSgp from './breadcrumb-sgp';
import Mensagens from './mensagens/mensagens';
import ModalConfirmacao from './modalConfirmacao';
import TempoExpiracaoSessao from './tempoExpiracaoSessao/tempoExpiracaoSessao';

const Conteudo = () => {
  const menuRetraido = useSelector(store => store.navegacao.retraido);

  return (
    <div style={{ marginLeft: menuRetraido ? '115px' : '250px' }}>
      <TempoExpiracaoSessao />
      <BreadcrumbSgp />
      <div className="row h-100">
        <main role="main" className="col-md-12 col-lg-12 col-sm-12 col-xl-12">
          <ModalConfirmacao />
          <Mensagens somenteConsulta={menuRetraido.somenteConsulta} />
        </main>
      </div>
      <Switch>
        {rotasArray.map(rota => (
          <RotaAutenticadaEstruturada
            key={shortid.generate()}
            path={rota.path}
            component={rota.component}
            exact={rota.exact}
          />
        ))}
      </Switch>
    </div>
  );
};

export default Conteudo;
