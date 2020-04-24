import React, { useState } from 'react';
import SelectComponent from '~/componentes/select';
import { Combo } from './CampoConceitoPosConselho.css';
import { useSelector } from 'react-redux';

const CampoConceitoPosConselho = props => {
  const {
    nota,
    desabilitarCampo,
    listaTiposConceitos,
    ehRegencia,
    index,
  } = props;

  const [valorSelecionado, setValorSelecionado] = useState(nota);

  const notasJustificativas = useSelector(
    state => state.conselhoClasse.notasJustificativas
  );

  const onChangeNotaConceitoComponente = novoValor => {
    if (notasJustificativas.componentes.length > 0) {
      notasJustificativas.componentes[index].habilitado = !notasJustificativas
        .componentes[index].habilitado;
    }
    setValorSelecionado(novoValor);
  };

  const onChangeNotaConceitoComponenteRegencia = novoValor => {
    if (notasJustificativas.componentesRegencia.length > 0) {
      notasJustificativas.componentesRegencia[
        index
      ].habilitado = !notasJustificativas.componentesRegencia[index].habilitado;
    }
    setValorSelecionado(novoValor);
  };

  return (
    <Combo>
      <SelectComponent
        onChange={() =>
          ehRegencia
            ? onChangeNotaConceitoComponenteRegencia
            : onChangeNotaConceitoComponente
        }
        valueOption="id"
        valueText="valor"
        lista={listaTiposConceitos}
        valueSelect={valorSelecionado}
        showSearch
        placeholder="Conceito"
        disabled={desabilitarCampo}
      />
    </Combo>
  );
};

export default CampoConceitoPosConselho;
