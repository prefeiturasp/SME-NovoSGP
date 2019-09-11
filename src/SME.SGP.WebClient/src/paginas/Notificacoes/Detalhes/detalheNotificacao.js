import React from 'react';
import Card from '~/componentes/card';
import { EstiloDetalhe } from './estiloDetalhe';

const DetalheNotificacao = () => {
  return (
    <>
      <h3>Notificações</h3>
      <Card>
        <EstiloDetalhe>
          <div className="col-xs-12 col-md-12 col-lg-12">
            <div className="row">
              <div className="col-xs-12 col-md-12 col-lg-2 bg-id">
                <div className="row">
                  <div className="col-xs-12 col-md-12 col-lg-12 text-center">
                    ID
                  </div>
                  <div className="col-xs-12 col-md-12 col-lg-12 text-center">
                    00000010
                  </div>
                </div>
              </div>
              <div className="col-xs-12 col-md-12 col-lg-3">Tipo</div>
              <div className="col-xs-12 col-md-12 col-lg-4">Título</div>
              <div className="col-xs-12 col-md-12 col-lg-2">Situação</div>
            </div>
          </div>
        </EstiloDetalhe>
      </Card>
    </>
  );
};
export default DetalheNotificacao;
