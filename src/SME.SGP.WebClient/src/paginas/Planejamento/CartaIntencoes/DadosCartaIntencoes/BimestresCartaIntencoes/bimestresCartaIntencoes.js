import PropTypes from 'prop-types';
import React, { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { Auditoria } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';
import {
  setCartaIntencoesEmEdicao,
  setDadosParaSalvarCartaIntencoes,
} from '~/redux/modulos/cartaIntencoes/actions';
import servicoSalvarCartaIntencoes from '../../servicoSalvarCartaIntencoes';

const BimestresCartaIntencoes = props => {
  const { descricao, bimestre, auditoria } = props;

  const dispatch = useDispatch();

  const onChange = useCallback(
    valorNovo => {
      const dadosEmEdicao = {
        bimestre,
        descricao: valorNovo,
      };
      dispatch(setDadosParaSalvarCartaIntencoes(dadosEmEdicao));
    },
    [dispatch, bimestre]
  );

  const validarSeTemErro = valorEditado => {
    if (servicoSalvarCartaIntencoes.campoInvalido(valorEditado)) {
      return true;
    }
    return false;
  };

  return (
    <CardCollapse
      key={`bimestre-${bimestre}-collapse-key`}
      titulo={`${bimestre}º Bimestre`}
      indice={`bimestre-${bimestre}-collapse-indice`}
      alt={`bimestre-${bimestre}-alt`}
      show
    >
      <>
        <Editor
          validarSeTemErro={validarSeTemErro}
          mensagemErro="Campo obrigatório"
          id={`bimestre-${bimestre}-editor`}
          inicial={descricao}
          onChange={valorNovo => {
            onChange(valorNovo);
            dispatch(setCartaIntencoesEmEdicao(true));
          }}
          // desabilitar={alunoDesabilitado || !dentroPeriodo || desabilitarCampos}
        />
        {auditoria ? (
          <div className="row">
            <Auditoria
              criadoEm={auditoria.criadoEm}
              criadoPor={auditoria.criadoPor}
              criadoRf={auditoria.criadoRF}
              alteradoPor={auditoria.alteradoPor}
              alteradoEm={auditoria.alteradoEm}
              alteradoRf={auditoria.alteradoRF}
              ignorarMarginTop
            />
          </div>
        ) : (
          ''
        )}
      </>
    </CardCollapse>
  );
};

BimestresCartaIntencoes.propTypes = {
  descricao: PropTypes.string,
  bimestre: PropTypes.oneOfType([PropTypes.any]),
  auditoria: PropTypes.oneOfType([PropTypes.object]),
};

BimestresCartaIntencoes.defaultProps = {
  descricao: '',
  bimestre: '',
  auditoria: null,
};

export default BimestresCartaIntencoes;
