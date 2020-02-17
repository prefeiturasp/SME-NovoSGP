import { Tooltip } from 'antd';
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
    mediaAprovacaoBimestre,
    label,
    podeLancarNotaFinal,
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

  const validaSeEstaAbaixoDaMedia = useCallback(
    valorAtual => {
      if (valorAtual && valorAtual < mediaAprovacaoBimestre) {
        notaBimestre.abaixoDaMedia = true;
      } else {
        notaBimestre.abaixoDaMedia = false;
      }
    },
    [mediaAprovacaoBimestre, notaBimestre]
  );

  useEffect(() => {
    setNotaBimestre(montaNotaFinal());
  }, [montaNotaFinal]);

  useEffect(() => {
    if (notaBimestre) {
      validaSeEstaAbaixoDaMedia(notaBimestre.notaConceito);
      validaSeTeveAlteracao(notaBimestre.notaConceito);
      setNotaValorAtual(notaBimestre.notaConceito);
    }
  }, [notaBimestre, validaSeTeveAlteracao, validaSeEstaAbaixoDaMedia]);

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

      validaSeEstaAbaixoDaMedia(notaArredondada);
      validaSeTeveAlteracao(notaArredondada);
      onChangeNotaConceitoFinal(notaBimestre, notaArredondada);
      setNotaValorAtual(notaArredondada);
    }
  };

  return (
    <Tooltip
      placement="bottom"
      title={
        notaBimestre && notaBimestre.abaixoDaMedia ? 'Abaixo da Média' : ''
      }
    >
      <div>
        <CampoNumero
          label={label ? label : ''}
          onBlur={valorNovo => setarValorNovo(valorNovo.target.value)}
          value={notaValorAtual}
          min={0}
          max={10}
          step={0.5}
          placeholder="Nota Final"
          disabled={
            desabilitarCampo ||
            !podeEditar ||
            !podeLancarNotaFinal ||
            modoEdicaoGeral
          }
          className={`tamanho-conceito-final ${
            notaBimestre && notaBimestre.abaixoDaMedia
              ? 'border-abaixo-media'
              : notaBimestre && notaBimestre.notaAlterada
              ? 'border-registro-alterado'
              : ''
          } `}
        />
      </div>
    </Tooltip>
  );
};

CampoNotaFinal.defaultProps = {
  onChangeNotaConceitoFinal: PropTypes.func,
  montaNotaFinal: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  podeEditar: PropTypes.bool,
  periodoFim: PropTypes.string,
  mediaAprovacaoBimestre: PropTypes.number,
};

CampoNotaFinal.propTypes = {
  onChangeNotaConceitoFinal: () => {},
  montaNotaFinal: () => {},
  desabilitarCampo: false,
  podeEditar: false,
  periodoFim: '',
  mediaAprovacaoBimestre: 0,
};

export default CampoNotaFinal;
