import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import CampoObservacoesAdicionais from './campoObservacoesAdicionais';

const ObservacoesAdicionais = () => {
  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="observacoes-adicionais-collapse"
        titulo="Observações adicionais"
        indice="observacoes-adicionais"
        alt="observacoes-adicionais"
      >
        <CampoObservacoesAdicionais />
      </CardCollapse>
    </div>
  );
};

export default ObservacoesAdicionais;
