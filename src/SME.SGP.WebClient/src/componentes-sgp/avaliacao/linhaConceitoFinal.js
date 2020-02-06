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
        <div style={{ height: '83px' }}>
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
        </div>
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
