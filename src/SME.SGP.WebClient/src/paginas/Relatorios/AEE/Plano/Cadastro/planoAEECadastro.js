import React from 'react';
import PropTypes from 'prop-types';
import { Cabecalho } from '~/componentes-sgp';
import Card from '~/componentes/card';
import BotoesAcoesPlanoAEE from './Componentes/botoesAcoesPlanoAEE';
import SecaoLocalizarEstudanteCollapse from './Componentes/SecaoLocalizarEstudante/secaoLocalizarEstudanteCollapse';
import LoaderPlano from './Componentes/LoaderPlano/loaderPlano';
import TabCadastroPasso from './Componentes/TabCadastroPlano/tabCadastroPlano';

const PlanoAEECadastro = ({ match }) => {
  return (
    <LoaderPlano>
      <Cabecalho pagina="Plano AEE" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <BotoesAcoesPlanoAEE match={match} />
            </div>
            {match?.params?.id ? (
              ''
            ) : (
              <div className="col-md-12 mb-2">
                <SecaoLocalizarEstudanteCollapse />
              </div>
            )}
            <div className="col-md-12 mb-2">
              {<TabCadastroPasso match={match} />}
              {/* <SecaoEncaminhamentoCollapse match={match} /> */}
            </div>
          </div>
        </div>
      </Card>
    </LoaderPlano>
  );
};

PlanoAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

PlanoAEECadastro.defaultProps = {
  match: {},
};

export default PlanoAEECadastro;
