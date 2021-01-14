import PropTypes from 'prop-types';
import React from 'react';
import { Card } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import BotoesAcoesEncaminhamentoAEE from './Componentes/botoesAcoesEncaminhamentoAEE';
import SecaoEncaminhamentoCollapse from './Componentes/SecaoEncaminhamento/secaoEncaminhamentoCollapse';
import SecaoLocalizarEstudanteCollapse from './Componentes/SecaoLocalizarEstudante/secaoLocalizarEstudanteCollapse';

const EncaminhamentoAEECadastro = ({ match }) => {
  return (
    <>
      <Cabecalho pagina="Encaminhamento AEE" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <BotoesAcoesEncaminhamentoAEE match={match} />
            </div>
            <div className="col-md-12 mb-2">
              <SecaoLocalizarEstudanteCollapse />
            </div>
            <div className="col-md-12 mb-2">
              <SecaoEncaminhamentoCollapse match={match} />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

EncaminhamentoAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

EncaminhamentoAEECadastro.defaultProps = {
  match: {},
};

export default EncaminhamentoAEECadastro;
