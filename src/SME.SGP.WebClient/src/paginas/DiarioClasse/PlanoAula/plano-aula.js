import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';

const PlanoAula = () => {
  const [mostrarCardPrincipal, setMostrarCardPrincipal] = useState(true);
  const [quantidadeAulas, setQuantidadeAulas] = useState(0);
  return (
    <CardCollapse
      key="plano-aula"
      onClick={() => { setMostrarCardPrincipal(!mostrarCardPrincipal) }}
      titulo={'Plano de aula'}
      indice={'Plano de aula'}
      show={mostrarCardPrincipal}
    >
      <span>Quantidade de aulas: {quantidadeAulas}</span>
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
