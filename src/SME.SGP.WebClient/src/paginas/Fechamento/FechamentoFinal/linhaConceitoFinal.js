import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';

const LinhaConceitoFinal = props => {
  const expandirLinha = useSelector(
    store => store.notasConceitos.expandirLinha
  );

  const { indexLinha, montarCampoNotaConceitoFinal, aluno } = props;

  return (
    <>
      {expandirLinha[indexLinha] ? (
        <>
          <tr className="linha-conceito-regencia">
            <td colSpan="7">
              <div className="coluna-regencia">
                {aluno &&
                aluno.notasConceitoFinal &&
                aluno.notasConceitoFinal.length
                  ? aluno.notasConceitoFinal.map((item, index) => {
                      return (
                        <div
                          style={{ paddingRight: '22px' }}
                          key={shortid.generate()}
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
};

LinhaConceitoFinal.propTypes = {
  indexLinha: null,
};

export default LinhaConceitoFinal;
