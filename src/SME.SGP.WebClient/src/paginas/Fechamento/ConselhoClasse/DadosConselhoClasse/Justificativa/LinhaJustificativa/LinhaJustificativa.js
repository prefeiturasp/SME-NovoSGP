import React from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import Justificativa from '../Justificativa';

const LinhaJustificativa = props => {
  const { idCampo, ehRegencia, alunoDesabilitado } = props;

  const expandirLinha = useSelector(
    store => store.conselhoClasse.expandirLinha[idCampo]
  );

  return (
    <>
      {expandirLinha ? (
        <tr>
          <td style={{ height: '100px' }} colSpan={ehRegencia ? '4' : '8'}>
            <Justificativa alunoDesabilitado={alunoDesabilitado} />
          </td>
        </tr>
      ) : null}
    </>
  );
};

LinhaJustificativa.propTypes = {
  idCampo: PropTypes.oneOfType([PropTypes.any]).isRequired,
  ehRegencia: PropTypes.bool.isRequired,
  alunoDesabilitado: PropTypes.bool.isRequired,
};

export default LinhaJustificativa;
