import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
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

  const [notaBimestre, setNotaBimestre] = useState();
  const [notaValorAtual, setNotaValorAtual] = useState();
  const [notaAlterada, setNotaAlterada] = useState(false);

  const validaSeTeveAlteracao = useCallback((notaOriginal, notaNova) => {
    if (
      notaOriginal != undefined &&
      notaOriginal != null &&
      notaOriginal.trim() !== ''
    ) {
      setNotaAlterada(
        Number(notaNova).toFixed(1) !== Number(notaOriginal).toFixed(1)
      );
    }
  }, []);

  useEffect(() => {
    setNotaBimestre(montaNotaFinal());
  }, [montaNotaFinal]);

  useEffect(() => {
    if (notaBimestre) {
      setNotaValorAtual(notaBimestre.notaConceito);
      validaSeTeveAlteracao(
        notaBimestre.notaOriginal,
        notaBimestre.notaConceito
      );
    }
  }, [notaBimestre, validaSeTeveAlteracao]);

  const setarValorNovo = async valorNovo => {
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
      setNotaValorAtual(notaArredondada);
    }

    validaSeTeveAlteracao(notaBimestre.notaOriginal, notaArredondada);
    onChangeNotaConceitoFinal(notaBimestre, notaArredondada);
    validaSeTeveAlteracao(notaBimestre.notaOriginal, notaArredondada);
  };

  return (
    <CampoNumero
      onBlur={valorNovo => setarValorNovo(valorNovo.target.value)}
      value={notaValorAtual}
      min={0}
      max={10}
      step={0.5}
      placeholder="Nota Final"
      desabilitado={desabilitarCampo || !podeEditar}
      className={`${notaAlterada ? 'border-registro-alterado' : ''}`}
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
