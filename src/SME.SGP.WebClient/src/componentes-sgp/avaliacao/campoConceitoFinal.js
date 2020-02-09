import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import SelectComponent from '~/componentes/select';

const CampoConceitoFinal = props => {
  const {
    montaNotaConceitoFinal,
    onChangeNotaConceitoFinal,
    desabilitarCampo,
    podeEditar,
  } = props;

  const modoEdicaoGeral = useSelector(
    store => store.notasConceitos.modoEdicaoGeral
  );

  const [conceitoValorAtual, setConceitoValorAtual] = useState();
  const [notaConceitoBimestre, setNotaConceitoBimestre] = useState();

  const listaConceitos = [
    { valor: '1', descricao: 'P' },
    { valor: '2', descricao: 'S' },
    { valor: '3', descricao: 'NS' },
  ];

  const validaSeTeveAlteracao = useCallback(
    notaConceito => {
      if (notaConceitoBimestre.notaOriginal) {
        notaConceitoBimestre.conceitoAlterado =
          notaConceito != notaConceitoBimestre.notaOriginal;
      }
    },
    [notaConceitoBimestre]
  );

  useEffect(() => {
    setNotaConceitoBimestre(montaNotaConceitoFinal());
  }, [montaNotaConceitoFinal]);

  useEffect(() => {
    if (notaConceitoBimestre) {
      validaSeTeveAlteracao(notaConceitoBimestre.notaConceito);
      setConceitoValorAtual(notaConceitoBimestre.notaConceito);
    }
  }, [notaConceitoBimestre, validaSeTeveAlteracao]);

  const setarValorNovo = valorNovo => {
    if (!desabilitarCampo && podeEditar) {
      validaSeTeveAlteracao(valorNovo);
      onChangeNotaConceitoFinal(notaConceitoBimestre, valorNovo);
      setConceitoValorAtual(valorNovo);
    }
  };

  return (
    <SelectComponent
      onChange={valorNovo => setarValorNovo(valorNovo)}
      valueOption="valor"
      valueText="descricao"
      lista={listaConceitos}
      valueSelect={String(conceitoValorAtual) || undefined}
      showSearch
      placeholder="Final"
      className={`tamanho-conceito-final ${
        notaConceitoBimestre && notaConceitoBimestre.conceitoAlterado
          ? 'border-registro-alterado'
          : ''
      }`}
      disabled={desabilitarCampo || modoEdicaoGeral || !podeEditar}
    />
  );
};

CampoConceitoFinal.defaultProps = {
  onChangeNotaConceitoFinal: PropTypes.func,
  montaNotaConceitoFinal: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  podeEditar: PropTypes.bool,
};

CampoConceitoFinal.propTypes = {
  onChangeNotaConceitoFinal: () => {},
  montaNotaConceitoFinal: () => {},
  desabilitarCampo: false,
  podeEditar: false,
};

export default CampoConceitoFinal;
