import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import CampoNumero from '~/componentes/campoNumero';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';
import { Loader } from '~/componentes';
import { converterAcaoTecla } from '~/utils';

const CampoNotaFinal = props => {
  const {
    montaNotaFinal,
    onChangeNotaConceitoFinal,
    desabilitarCampo,
    podeEditar,
    eventoData,
    mediaAprovacaoBimestre,
    label,
    clicarSetas,
    name,
    esconderSetas,
    step,
  } = props;

  const [notaBimestre, setNotaBimestre] = useState();
  const [notaValorAtual, setNotaValorAtual] = useState();
  const [notaAlterada, setNotaAlterada] = useState(false);
  const [abaixoDaMedia, setAbaixoDaMedia] = useState(false);
  const [
    carregandoValorArredondamento,
    setCarregandoValorArredondamento,
  ] = useState(false);

  const validaSeTeveAlteracao = useCallback(
    notaArredondada => {
      if (
        notaBimestre.notaConceitoAtual != undefined &&
        notaBimestre.notaConceitoAtual != null &&
        notaBimestre.notaConceitoAtual.trim() !== ''
      ) {
        const alterada =
          Number(notaArredondada).toFixed(1) !==
          Number(notaBimestre.notaConceitoAtual).toFixed(1);
        notaBimestre.notaAlterada = alterada;
        setNotaAlterada(alterada);
      }
    },
    [notaBimestre]
  );

  const validaSeEstaAbaixoDaMedia = useCallback(
    valorAtual => {
      valorAtual = removerCaracteresInvalidos(String(valorAtual));
      if (valorAtual && valorAtual < mediaAprovacaoBimestre) {
        notaBimestre.abaixoDaMedia = true;
        setAbaixoDaMedia(true);
      } else {
        notaBimestre.abaixoDaMedia = false;
        setAbaixoDaMedia(false);
      }
    },
    [mediaAprovacaoBimestre, notaBimestre]
  );

  const notaAtual = notaValorAtual >= 0 ? notaValorAtual : '';

  useEffect(() => {
    setNotaBimestre(montaNotaFinal());
  }, [montaNotaFinal]);

  useEffect(() => {
    if (notaBimestre) {
      const notaConceitoParseada = String(notaBimestre.notaConceito);
      const notaConceitoAlterada = notaConceitoParseada.replace(',', '.');
      const nota = Number(notaConceitoAlterada);
      validaSeEstaAbaixoDaMedia(nota);
      validaSeTeveAlteracao(String(nota));
      setNotaValorAtual(nota);
    }
  }, [notaBimestre, validaSeTeveAlteracao, validaSeEstaAbaixoDaMedia]);

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

  const setarValorNovo = async valorNovo => {
    if (!desabilitarCampo && podeEditar) {
      setNotaValorAtual(valorNovo);
      const resto = valorNovo % 0.5;
      let notaArredondada = valorNovo;
      if (resto) {
        setCarregandoValorArredondamento(true);
        const retorno = await api
          .get(
            `v1/avaliacoes/notas/${Number(
              valorNovo
            )}/arredondamento?data=${eventoData}`
          )
          .catch(e => erros(e));

        if (retorno && retorno.data) {
          notaArredondada = retorno.data;
        }
        setCarregandoValorArredondamento(false);
      }

      validaSeEstaAbaixoDaMedia(notaArredondada);
      validaSeTeveAlteracao(notaArredondada);
      onChangeNotaConceitoFinal(notaBimestre, notaArredondada);
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
    <Tooltip placement="bottom" title={abaixoDaMedia ? 'Abaixo da Média' : ''}>
      <div>
        <Loader loading={carregandoValorArredondamento} tip="">
          <CampoNumero
            esconderSetas={esconderSetas}
            name={name}
            onKeyDown={clicarSetas}
            onKeyUp={apertarTecla}
            label={label || ''}
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
            value={notaAtual}
            min={0}
            max={10}
            step={step}
            disabled={desabilitarCampo || !podeEditar}
            className={`tamanho-conceito-final ${
              abaixoDaMedia
                ? 'border-abaixo-media'
                : notaAlterada
                ? 'border-registro-alterado'
                : ''
            } `}
          />
        </Loader>
      </div>
    </Tooltip>
  );
};

CampoNotaFinal.defaultProps = {
  onChangeNotaConceitoFinal: PropTypes.func,
  montaNotaFinal: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  podeEditar: PropTypes.bool,
  eventoData: PropTypes.string,
  mediaAprovacaoBimestre: PropTypes.number,
  clicarSetas: PropTypes.func,
  name: PropTypes.string,
  esconderSetas: PropTypes.bool,
  step: PropTypes.number,
};

CampoNotaFinal.propTypes = {
  onChangeNotaConceitoFinal: () => {},
  montaNotaFinal: () => {},
  desabilitarCampo: false,
  podeEditar: false,
  eventoData: '',
  mediaAprovacaoBimestre: 0,
  clicarSetas: () => {},
  name: '',
  esconderSetas: false,
  step: 0.5,
};

export default CampoNotaFinal;
