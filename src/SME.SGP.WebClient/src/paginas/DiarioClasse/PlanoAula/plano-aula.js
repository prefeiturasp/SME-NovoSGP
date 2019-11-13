import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';

const PlanoAula = () => {
  return (
    <CardCollapse
      key="plano-aula"
      onClick={() => { }}
      titulo={'Plano de aula'}
      indice={'Plano de aula'}
      show={true}
    >
      <CardCollapse
        key="teste"
        onClick={() => { }}
        titulo={'Objetivos de aprendizagem e meus objetivos (Currículo da Cidade)'}
        indice={'teste'}
        show={true}
        configCabecalho={{
          altura: '44px',
          corBorda: '#4072d6'
        }}
      >

      </CardCollapse>
    </CardCollapse>
  )
}

export default PlanoAula;
