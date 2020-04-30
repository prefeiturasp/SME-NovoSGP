import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useState } from 'react';
import styled from 'styled-components';
import SelectComponent from '~/componentes/select';

export const Combo = styled.div`
  width: 105px;
  height: 35px;
  display: inline-block;
`;

const CampoConceito = props => {
  const { notaPosConselho, listaTiposConceitos, desabilitarCampo } = props;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);
  const [abaixoDaMedia, setAbaixoDaMedia] = useState(false);

  const validaSeEstaAbaixoDaMedia = useCallback(
    valorAtual => {
      const tipoConceito = listaTiposConceitos.find(
        item => item.id == valorAtual
      );

      if (tipoConceito && !tipoConceito.aprovado) {
        setAbaixoDaMedia(true);
      } else {
        setAbaixoDaMedia(false);
      }
    },
    [listaTiposConceitos]
  );

  const onChangeConceito = valorNovo => {
    if (!desabilitarCampo) {
      validaSeEstaAbaixoDaMedia(valorNovo);
      setNotaValorAtual(valorNovo);
    }
  };

  return (
    <Tooltip placement="bottom" title={abaixoDaMedia ? 'Abaixo da Média' : ''}>
      <Combo>
        <SelectComponent
          onChange={onChangeConceito}
          valueOption="id"
          valueText="valor"
          lista={listaTiposConceitos}
          valueSelect={notaValorAtual ? String(notaValorAtual) : ''}
          showSearch
          placeholder="Conceito"
          className={abaixoDaMedia ? 'borda-abaixo-media' : ''}
        />
      </Combo>
    </Tooltip>
  );
};

CampoConceito.propTypes = {
  notaPosConselho: PropTypes.oneOfType([PropTypes.any]),
  listaTiposConceitos: PropTypes.oneOfType([PropTypes.array]),
  desabilitarCampo: PropTypes.bool,
};

CampoConceito.defaultProps = {
  notaPosConselho: '',
  listaTiposConceitos: [],
  desabilitarCampo: false,
};

export default CampoConceito;
