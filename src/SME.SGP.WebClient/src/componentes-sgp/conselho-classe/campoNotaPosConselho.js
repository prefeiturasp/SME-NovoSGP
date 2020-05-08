import React, { useState } from 'react';
import CampoNumero from '~/componentes/campoNumero';
import { useSelector } from 'react-redux';

const CampoNotaPosConselho = props => {
  const { nota, desabilitarCampo, ehRegencia, index } = props;
  const [valor, setValor] = useState(nota);

  const notasJustificativas = useSelector(
    state => state.conselhoClasse.notasJustificativas
  );

  const onChangeNotaConceitoComponente = novoValor => {
    if (notasJustificativas.componentes.length > 0) {
      notasJustificativas.componentes[index].habilitado = !notasJustificativas
        .componentes[index].habilitado;
    }
    setValor(novoValor);
  };

  const onChangeNotaConceitoComponenteRegencia = novoValor => {
    if (notasJustificativas.componentesRegencia.length > 0) {
      notasJustificativas.componentesRegencia[
        index
      ].habilitado = !notasJustificativas.componentesRegencia[index].habilitado;
    }
    setValor(novoValor);
  };

  return (
    <CampoNumero
      onChange={novoValor =>
        ehRegencia
          ? onChangeNotaConceitoComponenteRegencia(novoValor)
          : onChangeNotaConceitoComponente(novoValor)
      }
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
