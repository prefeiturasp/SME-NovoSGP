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
        // <div>
        <tr>
          {/* <td className="sticky-col col-numero-chamada" /> */}
          {/* <td className="sticky-col desc-linha-conceito-final"> */}
          Conceitos finais Regência de classe
          {/* </td> */}
          {/* <div> */}
          {/* <td colSpan={6 + 2} align="left"> */}
          <CampoConceitoFinal />
          <CampoConceitoFinal />
          <CampoConceitoFinal />
          <CampoConceitoFinal />
          <CampoConceitoFinal />
          {/* </td> */}
          {/* </div> */}
        </tr>
      ) : (
        // </div>
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
