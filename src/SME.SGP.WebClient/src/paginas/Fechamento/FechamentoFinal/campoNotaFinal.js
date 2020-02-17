import { Tooltip } from 'antd';
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
    eventoData,
    mediaAprovacaoBimestre,
    label,
  } = props;

  const [notaBimestre, setNotaBimestre] = useState();
  const [notaValorAtual, setNotaValorAtual] = useState();

  const validaSeTeveAlteracao = useCallback(
    notaArredondada => {
      if (
        notaBimestre.notaConceitoAtual != undefined &&
        notaBimestre.notaConceitoAtual != null &&
        notaBimestre.notaConceitoAtual.trim() !== ''
      ) {
        notaBimestre.notaAlterada =
          Number(notaArredondada).toFixed(1) !==
          Number(notaBimestre.notaConceitoAtual).toFixed(1);
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
          )}/arredondamento?data=${eventoData}`
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
          disabled={desabilitarCampo || !podeEditar}
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
  eventoData: PropTypes.string,
  mediaAprovacaoBimestre: PropTypes.number,
};

CampoNotaFinal.propTypes = {
  onChangeNotaConceitoFinal: () => {},
  montaNotaFinal: () => {},
  desabilitarCampo: false,
  podeEditar: false,
  eventoData: '',
  mediaAprovacaoBimestre: 0,
};

export default CampoNotaFinal;
