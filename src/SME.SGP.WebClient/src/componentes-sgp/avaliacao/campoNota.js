import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import CampoNumero from '~/componentes/campoNumero';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';
import { converterAcaoTecla } from '~/utils';

const CampoNota = props => {
  const {
    nota,
    onChangeNotaConceito,
    desabilitarCampo,
    clicarSetas,
    name,
    esconderSetas,
    step,
  } = props;

  const modoEdicaoGeralNotaFinal = useSelector(
    store => store.notasConceitos.modoEdicaoGeralNotaFinal
  );

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

  const removerCaracteresInvalidos = texto => {
    return texto.replace(/[^0-9,.]+/g, '');
  };

  const editouCampo = (notaOriginal, notaNova) => {
    notaOriginal = removerCaracteresInvalidos(String(notaOriginal));
    notaNova = removerCaracteresInvalidos(String(notaNova));
    if (notaOriginal === '' && notaNova === '') {
      return false;
    }
    return notaOriginal !== notaNova;
  };

  useEffect(() => {
    setNotaValorAtual(nota.notaConceito);
    validaSeTeveAlteracao(nota.notaOriginal, nota.notaConceito);
  }, [nota.notaConceito, nota.notaOriginal, validaSeTeveAlteracao]);

  const setarValorNovo = async valorNovo => {
    if (!desabilitarCampo && nota.podeEditar) {
      setNotaValorAtual(valorNovo);
      const resto = valorNovo % 0.5;
      let notaArredondada = valorNovo;
      if (resto) {
        setNotaValorAtual(valorNovo);
        const retorno = await api
          .get(
            `v1/avaliacoes/${nota.atividadeAvaliativaId}/notas/${Number(
              valorNovo
            )}/arredondamento`
          )
          .catch(e => erros(e));

        if (retorno && retorno.data) {
          notaArredondada = retorno.data;
        }
      }

      validaSeTeveAlteracao(nota.notaOriginal, notaArredondada);
      onChangeNotaConceito(notaArredondada);
      setNotaValorAtual(notaArredondada);
    }
  };

  const valorInvalido = valorNovo => {
    const regexValorInvalido = /[^0-9,.]+/g;
    return regexValorInvalido.test(String(valorNovo));
  };

  const apertarTecla = e => {
    const teclaEscolhida = converterAcaoTecla(e.keyCode);
    if (teclaEscolhida === 0) {
      setarValorNovo(0);
    }
  };

  return (
    <CampoNumero
      esconderSetas={esconderSetas}
      name={name}
      onKeyDown={clicarSetas}
      onKeyUp={apertarTecla}
      onChange={valorNovo => {
        let valorEnviado = null;
        if (valorNovo) {
          const invalido = valorInvalido(valorNovo);
          if (!invalido && editouCampo(notaValorAtual, valorNovo)) {
            valorEnviado = valorNovo;
          }
        }
        const valorCampo = valorNovo > 0 ? valorNovo : null;
        setarValorNovo(valorEnviado || valorCampo);
      }}
      value={notaValorAtual}
      min={0}
      max={10}
      step={step}
      placeholder="Nota"
      classNameCampo={`${nota.ausente ? 'aluno-ausente-notas' : ''}`}
      disabled={
        desabilitarCampo || modoEdicaoGeralNotaFinal || !nota.podeEditar
      }
      className={`${notaAlterada ? 'border-registro-alterado' : ''}`}
    />
  );
};

CampoNota.defaultProps = {
  nota: '',
  onChangeNotaConceito: () => {},
  desabilitarCampo: false,
  clicarSetas: () => {},
  name: '',
  esconderSetas: false,
  step: 0.5,
};

CampoNota.propTypes = {
  nota: PropTypes.string,
  onChangeNotaConceito: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  clicarSetas: PropTypes.func,
  name: PropTypes.string,
  esconderSetas: PropTypes.bool,
  step: PropTypes.number,
};

export default CampoNota;
