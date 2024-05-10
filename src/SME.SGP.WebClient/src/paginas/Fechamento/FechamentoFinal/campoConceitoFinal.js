import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import SelectComponent from '~/componentes/select';

const CampoConceitoFinal = props => {
  const {
    montaNotaConceitoFinal,
    onChangeNotaConceitoFinal,
    desabilitarCampo,
    podeEditar,
    listaTiposConceitos,
    label,
  } = props;

  const [conceitoValorAtual, setConceitoValorAtual] = useState();
  const [notaConceitoBimestre, setNotaConceitoBimestre] = useState();

  const validaSeTeveAlteracao = useCallback(
    notaConceito => {
      if (notaConceitoBimestre.notaConceitoAtual) {
        notaConceitoBimestre.conceitoAlterado =
          notaConceito != notaConceitoBimestre.notaConceitoAtual;
      }
    },
    [notaConceitoBimestre]
  );

  const validaSeEstaAbaixoDaMedia = useCallback(
    valorAtual => {
      const tipoConceito = listaTiposConceitos.find(
        item => item.id == valorAtual
      );

      if (tipoConceito && !tipoConceito.aprovado) {
        notaConceitoBimestre.abaixoDaMedia = true;
      } else {
        notaConceitoBimestre.abaixoDaMedia = false;
      }
    },
    [listaTiposConceitos, notaConceitoBimestre]
  );

  useEffect(() => {
    setNotaConceitoBimestre(montaNotaConceitoFinal());
  }, [montaNotaConceitoFinal]);

  useEffect(() => {
    if (notaConceitoBimestre) {
      validaSeEstaAbaixoDaMedia(notaConceitoBimestre.notaConceito);
      validaSeTeveAlteracao(notaConceitoBimestre.notaConceito);
      setConceitoValorAtual(notaConceitoBimestre.notaConceito);
    }
  }, [notaConceitoBimestre, validaSeTeveAlteracao, validaSeEstaAbaixoDaMedia]);

  const setarValorNovo = valorNovo => {
    if (!desabilitarCampo && podeEditar) {
      validaSeEstaAbaixoDaMedia(valorNovo);
      validaSeTeveAlteracao(valorNovo);
      onChangeNotaConceitoFinal(notaConceitoBimestre, valorNovo);
      setConceitoValorAtual(valorNovo);
    }
  };

  return (
    <Tooltip
      placement="bottom"
      title={
        notaConceitoBimestre && notaConceitoBimestre.abaixoDaMedia
          ? 'Abaixo da Média'
          : ''
      }
    >
      <div>
        <SelectComponent
          label={label ? label : ''}
          onChange={valorNovo => setarValorNovo(valorNovo)}
          valueOption="id"
          valueText="valor"
          lista={listaTiposConceitos}
          valueSelect={
            conceitoValorAtual ? String(conceitoValorAtual) : undefined
          }
          showSearch
          placeholder="Final"
          className={`tamanho-conceito-final ${
            notaConceitoBimestre && notaConceitoBimestre.abaixoDaMedia
              ? 'border-abaixo-media'
              : notaConceitoBimestre && notaConceitoBimestre.conceitoAlterado
              ? 'border-registro-alterado'
              : ''
          } `}
          disabled={desabilitarCampo || !podeEditar}
        />
      </div>
    </Tooltip>
  );
};

CampoConceitoFinal.defaultProps = {
  onChangeNotaConceitoFinal: PropTypes.func,
  montaNotaConceitoFinal: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  podeEditar: PropTypes.bool,
  listaTiposConceitos: PropTypes.array,
};

CampoConceitoFinal.propTypes = {
  onChangeNotaConceitoFinal: () => {},
  montaNotaConceitoFinal: () => {},
  desabilitarCampo: false,
  podeEditar: true,
  listaTiposConceitos: [],
};

export default CampoConceitoFinal;
