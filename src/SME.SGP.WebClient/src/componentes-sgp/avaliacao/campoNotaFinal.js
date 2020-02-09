import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import CampoNumero from '~/componentes/campoNumero';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';

const CampoNotaFinal = props => {
  const {
    montaNotaFinal,
    onChangeNotaConceitoFinal,
    desabilitarCampo,
    podeEditar,
    periodoFim,
  } = props;

  const modoEdicaoGeral = useSelector(
    store => store.notasConceitos.modoEdicaoGeral
  );

  const [notaBimestre, setNotaBimestre] = useState();
  const [notaValorAtual, setNotaValorAtual] = useState();

  const validaSeTeveAlteracao = useCallback(
    notaArredondada => {
      if (
        notaBimestre.notaOriginal != undefined &&
        notaBimestre.notaOriginal != null &&
        notaBimestre.notaOriginal.trim() !== ''
      ) {
        notaBimestre.notaAlterada =
          Number(notaArredondada).toFixed(1) !==
          Number(notaBimestre.notaOriginal).toFixed(1);
      }
    },
    [notaBimestre]
  );

  useEffect(() => {
    setNotaBimestre(montaNotaFinal());
  }, [montaNotaFinal]);

  useEffect(() => {
    if (notaBimestre) {
      validaSeTeveAlteracao(notaBimestre.notaConceito);
      setNotaValorAtual(notaBimestre.notaConceito);
    }
  }, [notaBimestre, validaSeTeveAlteracao]);

  const setarValorNovo = async valorNovo => {
    if (!desabilitarCampo && podeEditar) {
      setNotaValorAtual(valorNovo);
      const retorno = await api
        .get(
          `v1/avaliacoes/notas/${Number(
            valorNovo
          )}/arredondamento?data=${periodoFim}`
        )
        .catch(e => erros(e));

      let notaArredondada = valorNovo;
      if (retorno && retorno.data) {
        notaArredondada = retorno.data;
      }

      validaSeTeveAlteracao(notaArredondada);
      onChangeNotaConceitoFinal(notaBimestre, notaArredondada);
      setNotaValorAtual(notaArredondada);
    }
  };

  return (
    <CampoNumero
      onBlur={valorNovo => setarValorNovo(valorNovo.target.value)}
      value={notaValorAtual}
      min={0}
      max={10}
      step={0.5}
      placeholder="Nota Final"
      disabled={desabilitarCampo || modoEdicaoGeral || !podeEditar}
      className={`${
        notaBimestre && notaBimestre.notaAlterada
          ? 'border-registro-alterado'
          : ''
      }`}
    />
  );
};

CampoNotaFinal.defaultProps = {
  onChangeNotaConceitoFinal: PropTypes.func,
  montaNotaFinal: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  podeEditar: PropTypes.bool,
  periodoFim: PropTypes.string,
};

CampoNotaFinal.propTypes = {
  onChangeNotaConceitoFinal: () => {},
  montaNotaFinal: () => {},
  desabilitarCampo: false,
  podeEditar: false,
  periodoFim: '',
};

export default CampoNotaFinal;
