import React from 'react';
import { CampoNotaRegencia, LinhaNotaRegencia, TdRegencia, TrRegencia } from './fechamento-regencia.css';

const FechamentoRegencia = props => {
  const { ocultar } = props;

  return (
    <TrRegencia hidden={false}>
      <td colSpan="2" style={{ verticalAlign: 'middle' }}>Conceitos finais regência de classe</td>
      <TdRegencia colSpan="4">
        <LinhaNotaRegencia>
          <CampoNotaRegencia>
            <span className="centro disciplina">Português</span>
            <span className="centro nota">1</span>
          </CampoNotaRegencia>
          <CampoNotaRegencia>
            <span className="centro disciplina">Matemática</span>
            <span className="centro nota">1</span>
          </CampoNotaRegencia>

          <CampoNotaRegencia>
            <span className="centro disciplina">Matemática</span>
            <span className="centro nota">1</span>
          </CampoNotaRegencia>
          <CampoNotaRegencia>
            <span className="centro disciplina">Matemática</span>
            <span className="centro nota">1</span>
          </CampoNotaRegencia>
          <CampoNotaRegencia>
            <span className="centro disciplina">Matemática</span>
            <span className="centro nota">1</span>
          </CampoNotaRegencia>
          <CampoNotaRegencia>
            <span className="centro disciplina">Matemática</span>
            <span className="centro nota">1</span>
          </CampoNotaRegencia>
        </LinhaNotaRegencia>
      </TdRegencia>
    </TrRegencia>
  )
}

export default FechamentoRegencia;
