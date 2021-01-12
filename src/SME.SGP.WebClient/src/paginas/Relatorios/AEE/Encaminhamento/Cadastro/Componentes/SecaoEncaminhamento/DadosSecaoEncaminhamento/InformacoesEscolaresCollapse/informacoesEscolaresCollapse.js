import React from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';

const InformacoesEscolaresCollapse = () => {
  const configCabecalho = {
    altura: '50px',
    corBorda: Base.AzulBordaCard,
  };

  const onClickCardCollapse = () => {};

  return (
    <div className="pt-2 pb-2">
      <CardCollapse
        key="informacoes-escolares-collapse-key"
        titulo="Informações escolares"
        indice="informacoes-escolares-collapse-indice"
        alt="informacoes-escolares-alt"
        onClick={onClickCardCollapse}
        configCabecalho={configCabecalho}
      >
        Informações escolares Informações escolares Informações escolares
        Informações escolares Informações escolares Informações escolares
        Informações escolares Informações escolares Informações escolares
        Informações escolares Informações escolares Informações escolares
        Informações escolares Informações escolares Informações escolares
        Informações escolares Informações escolares Informações escolares
      </CardCollapse>
    </div>
  );
};

export default InformacoesEscolaresCollapse;
