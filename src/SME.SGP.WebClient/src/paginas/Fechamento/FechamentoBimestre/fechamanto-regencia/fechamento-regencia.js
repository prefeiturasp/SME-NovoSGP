import shortid from 'shortid';
import React from 'react';
import {
  CampoNotaRegencia,
  LinhaNotaRegencia,
  TdRegencia,
  TrRegencia,
} from './fechamento-regencia.css';
import ServicoFechamentoBimestre from '~/servicos/Paginas/Fechamento/ServicoFechamentoBimestre';

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
                <CampoNotaRegencia key={shortid.generate()}>
                  <span className="centro disciplina">{item.disciplina}</span>
                  <span className="centro nota">
                    {item.ehConceito
                      ? item.conceitoDescricao
                      : ServicoFechamentoBimestre.formatarNotaConceito(
                          item.notaConceito
                        )}
                  </span>
                </CampoNotaRegencia>
              ))
            : null}
        </LinhaNotaRegencia>
      </TdRegencia>
    </TrRegencia>
  );
};

export default FechamentoRegencia;
