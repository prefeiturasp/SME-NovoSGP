import React from 'react';

import { Card } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import BotoesAcoesEncaminhamentoAEE from './Componentes/botoesAcoesEncaminhamentoAEE';
import SecaoLocalizarEstudanteCollapse from './Componentes/SecaoLocalizarEstudante/secaoLocalizarEstudanteCollapse';
import IndicativosEstudante from './Componentes/IndicativosEstudante/indicativosEstudante';

const EncaminhamentoAEECadastro = () => {
  return (
    <>
      <Cabecalho pagina="Encaminhamento AEE" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <BotoesAcoesEncaminhamentoAEE />
            </div>
            <div className="col-md-12 mb-2">
              <SecaoLocalizarEstudanteCollapse />
            </div>
            <div className="col-md-12 mb-2">
              <IndicativosEstudante />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default EncaminhamentoAEECadastro;
