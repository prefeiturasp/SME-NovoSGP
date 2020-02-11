import React from 'react';
import {
  CampoNotaRegencia,
  LinhaNotaRegencia,
  TdRegencia,
  TrRegencia,
} from './fechamento-regencia.css';

const FechamentoRegencia = props => {
  const { idRegencia, dados } = props;

  return (
    <TrRegencia id={idRegencia} style={{ display: 'none' }}>
      <td colSpan="2" className="destaque-label">
        Conceitos finais regência de classe
      </td>
      <TdRegencia colSpan="4">
        <LinhaNotaRegencia>
          {dados
            ? dados.map(item => (
                <CampoNotaRegencia>
                  <span className="centro disciplina">{item.disciplina}</span>
                  <span className="centro nota">{item.notaConceito}</span>
                </CampoNotaRegencia>
              ))
            : null}
        </LinhaNotaRegencia>
      </TdRegencia>
    </TrRegencia>
  );
};

export default FechamentoRegencia;
