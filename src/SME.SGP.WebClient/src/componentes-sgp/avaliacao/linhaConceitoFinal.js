import PropTypes from 'prop-types';
import React, { useEffect, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import CampoConceitoFinal from './campoConceitoFinal';

const LinhaConceitoFinal = props => {
  const dispatch = useDispatch();

  const expandirLinha = useSelector(
    store => store.notasConceitos.expandirLinha
  );

  const { indexLinha, dados, montarCampoNotaConceitoFinal, aluno } = props;

  const quantidadeAvaliacoes =
    dados && dados.avaliacoes && dados.avaliacoes.length
      ? dados.avaliacoes.length
      : 0;

  return (
    <>
      {expandirLinha[indexLinha] ? (
        <>
          <tr>
            <td
              colSpan={4 + quantidadeAvaliacoes}
              className="linha-conceito-final"
            >
              <div style={{ width: '400px', display: 'flex' }}>
                <div className="desc-linha-conceito-final">
                  Conceitos finais Regência de classe
                </div>
                {aluno && aluno.notasBimestre && aluno.notasBimestre.length
                  ? aluno.notasBimestre.map((item, index) => {
                      return (
                        <>
                          {montarCampoNotaConceitoFinal(item.disciplina, index)}
                        </>
                      );
                    })
                  : ''}
              </div>
            </td>
          </tr>
        </>
      ) : (
        ''
      )}
    </>
  );
};

LinhaConceitoFinal.defaultProps = {
  indexLinha: PropTypes.number,
  dados: PropTypes.array,
};

LinhaConceitoFinal.propTypes = {
  indexLinha: null,
  dados: [],
};

export default LinhaConceitoFinal;
