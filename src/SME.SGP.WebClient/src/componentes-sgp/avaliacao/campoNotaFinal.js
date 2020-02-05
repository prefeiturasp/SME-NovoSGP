import React, { useCallback, useEffect, useState } from 'react';
import CampoNumero from '~/componentes/campoNumero';

const CampoNotaFinal = props => {
  // const { nota, onChangeNotaConceito } = props;

  const [notaValorAtual, setNotaValorAtual] = useState();

  const setarValorNovo = async valorNovo => {
    setNotaValorAtual(valorNovo);
  };

  return (
    <CampoNumero
      onChange={valorNovo => setarValorNovo(valorNovo)}
      value={notaValorAtual}
      min={0}
      max={10}
      step={0.5}
      placeholder="Nota Final"
    />
  );
};

// CampoNotaFinal.defaultProps = {
//   onChangeNotaConceito: PropTypes.func,
// };

// CampoNotaFinal.propTypes = {
//   onChangeNotaConceito: () => {},
// };

export default CampoNotaFinal;
