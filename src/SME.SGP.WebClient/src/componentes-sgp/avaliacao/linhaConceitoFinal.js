import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { tratarString } from '~/utils';

const LinhaConceitoFinal = props => {
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
              <div style={{ width: '400px', display: 'flex', height: '65px' }}>
                <div className="desc-linha-conceito-final">
                  Conceitos finais Regência de classe
                </div>
                {aluno && aluno.notasBimestre && aluno.notasBimestre.length
                  ? aluno.notasBimestre.map((item, index) => {
                      const disciplinaTratada = tratarString(item.disciplina);
                      return (
                        <div
                          style={{ paddingRight: '22px' }}
                          key={shortid.generate()}
                          name={`${disciplinaTratada}${aluno?.id}`}
                        >
                          {montarCampoNotaConceitoFinal(item.disciplina, index)}
                        </div>
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
