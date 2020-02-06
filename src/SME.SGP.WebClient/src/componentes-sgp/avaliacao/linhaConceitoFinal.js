import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';

import CampoConceitoFinal from './campoConceitoFinal';

const LinhaConceitoFinal = props => {
  const dispatch = useDispatch();

  const expandirLinha = useSelector(
    store => store.notasConceitos.expandirLinha
  );

  const { indexLinha, dados } = props;

  return (
    <>
      {expandirLinha[indexLinha] ? (
        <>
          <tr>
            <td colSpan="5" style={{ overflow: 'scroll' }}>
              <div style={{ width: '300px', display: 'flex' }}>
                Conceitos finais Regência de classe
                <CampoConceitoFinal />
                <CampoConceitoFinal />
                <CampoConceitoFinal />
                <CampoConceitoFinal />
                <CampoConceitoFinal />
                <CampoConceitoFinal />
              </div>
            </td>
          </tr>

          {/* <div style={{ height: '83px' }}>
            <tr className="linha-conceito-final">
              <div className="desc-conceito-final">
                Conceitos finais Regência de classe
              </div>
              <CampoConceitoFinal />
              <CampoConceitoFinal />
              <CampoConceitoFinal />
              <CampoConceitoFinal />
              <CampoConceitoFinal />
            </tr>
          </div> */}

          {/* <tr>
            <td className="sticky-col col-numero-chamada" />
            <td className="sticky-col col-nome-aluno">
              Conceitos finais Regência de classe
            </td>
            <td className="sticky-col col-nome-aluno">
              <CampoConceitoFinal />
            </td>
            <td className="sticky-col col-nota-final linha-nota-conceito-final"></td>

            <td className="sticky-col col-frequencia linha-frequencia "></td>
          </tr> */}
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
