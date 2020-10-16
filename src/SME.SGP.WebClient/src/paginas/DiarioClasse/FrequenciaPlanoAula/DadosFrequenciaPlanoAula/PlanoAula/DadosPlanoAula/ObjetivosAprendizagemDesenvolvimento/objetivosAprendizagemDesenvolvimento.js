import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import TabsComponentesCorriculares from './TabsComponentesCorriculares/tabsComponentesCorriculares';

const ObjetivosAprendizagemDesenvolvimento = () => {
  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6',
  };

  return (
    <>
      <CardCollapse
        key="objetivos-aprendizagem-desenvolvimento"
        titulo="Objetivos de aprendizagem e meus objetivos (Currículo da Cidade)"
        indice="objetivos-aprendizagem-desenvolvimento"
        configCabecalho={configCabecalho}
        show
      >
        <div className="row mb-4">
          <div className="col-md-12 mb-2">
            <strong>Objetivos de componentes abordados na aula</strong>
          </div>
          <div className="col-md-12">
            Inclua os objetivos apenas para componentes curriculares que serão
            abordados nesta aula.
          </div>
        </div>
        <TabsComponentesCorriculares />
      </CardCollapse>
    </>
  );
};

export default ObjetivosAprendizagemDesenvolvimento;
