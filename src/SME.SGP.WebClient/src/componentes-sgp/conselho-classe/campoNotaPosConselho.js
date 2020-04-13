import React, { useState } from 'react';
import CampoNumero from '~/componentes/campoNumero';

const CampoNotaPosConselho = props => {
  const { onChangeNotaConceito, nota, desabilitarCampo } = props;
  const [valor, setValor] = useState(nota);

  const changeValor = novoValor => {
    setValor(novoValor);
  };

  return (
    <CampoNumero
      onChange={changeValor}
      value={valor}
      min={0}
      max={10}
      step={0.5}
      placeholder="Nota"
      disabled={desabilitarCampo}
    />
  );
};

export default CampoNotaPosConselho;
