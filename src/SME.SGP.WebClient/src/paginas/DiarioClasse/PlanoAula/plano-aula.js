import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import styled from 'styled-components';
import TextEditor from '~/componentes/textEditor/component';

const PlanoAula = () => {
  const [mostrarCardPrincipal, setMostrarCardPrincipal] = useState(true);
  const [quantidadeAulas, setQuantidadeAulas] = useState(0);
  const [planoAula, setPlanoAula] = useState({
    objetivosAprendizagem: null,
    desenvolvimentoAula: 'teste',
    recuperacaoContinua: 'teste',
    licaoCasa: 'teste'
  })
  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6'
  }

  const QuantidadeBotoes = styled.div`
    padding: 0 0 20px 0;
  `;

  return (
    <CardCollapse
      key="plano-aula"
      onClick={() => { setMostrarCardPrincipal(!mostrarCardPrincipal) }}
      titulo={'Plano de aula'}
      indice={'Plano de aula'}
      show={mostrarCardPrincipal}
    >
      <QuantidadeBotoes className="col-md-12">
        <span>Quantidade de aulas: {quantidadeAulas}</span>
      </QuantidadeBotoes>
      <CardCollapse
        key="objetivos-aprendizagem"
        onClick={() => { }}
        titulo={'Objetivos de aprendizagem e meus objetivos (Currículo da Cidade)'}
        indice={'objetivos-aprendizagem'}
        show={true}
        configCabecalho={configCabecalho}
      >
      </CardCollapse>

      <CardCollapse
        key="desenv-aula"
        onClick={() => { }}
        titulo={'Desenvolvimento da aula'}
        indice={'desenv-aula'}
        show={true}
        configCabecalho={configCabecalho}
      >
        <fieldset className="mt-3">
          <form action="">
            <TextEditor
              className="form-control"
              id="textEditor-desenv-aula"
              height="135px"
              alt="Desenvolvimento da aula"
              value={planoAula.desenvolvimentoAula}
            />
          </form>
        </fieldset>
      </CardCollapse>

      <CardCollapse
        key="rec-continua"
        onClick={() => { }}
        titulo={'Recuperação contínua'}
        indice={'rec-continua'}
        show={true}
        configCabecalho={configCabecalho}
      >
        <fieldset className="mt-3">
          <form action="">
            <TextEditor
              className="form-control"
              id="textEditor-rec-=continua"
              height="135px"
              alt="Recuperação contínua"
              value={planoAula.recuperacaoContinua}
            />
          </form>
        </fieldset>
      </CardCollapse>

      <CardCollapse
        key="licao-casa"
        onClick={() => { }}
        titulo={'Lição de casa'}
        indice={'licao-casa'}
        show={true}
        configCabecalho={configCabecalho}
      >
        <fieldset className="mt-3">
          <form action="">
            <TextEditor
              className="form-control"
              id="textEditor-licao-casa"
              height="135px"
              alt="Lição de casa"
              value={planoAula.licaoCasa}
            />
          </form>
        </fieldset>
      </CardCollapse>
    </CardCollapse>
  )
}

export default PlanoAula;
