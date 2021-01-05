import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { Auditoria } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setCartaIntencoesEmEdicao,
  setDadosParaSalvarCartaIntencoes,
} from '~/redux/modulos/cartaIntencoes/actions';
import servicoSalvarCartaIntencoes from '../../servicoSalvarCartaIntencoes';

const BimestresCartaIntencoes = props => {
  const { permissoesTela, carta } = props;
  const {
    planejamento,
    bimestre,
    auditoria,
    periodoEscolarId,
    id,
    periodoAberto,
    usuarioTemAtribuicao,
    somenteConsulta,
  } = carta;

  const dispatch = useDispatch();

  const [desabilitarCampo, setDesabilitarCampo] = useState(false);

  useEffect(() => {
    const desabilitar =
      id > 0
        ? somenteConsulta || !permissoesTela.podeAlterar
        : somenteConsulta || !permissoesTela.podeIncluir;

    if (!periodoAberto || !usuarioTemAtribuicao) {
      setDesabilitarCampo(true);
    } else {
      setDesabilitarCampo(desabilitar);
    }
  }, [permissoesTela, id, periodoAberto, somenteConsulta]);

  const onChange = useCallback(
    valorNovo => {
      const dadosEmEdicao = {
        id,
        periodoEscolarId,
        bimestre,
        planejamento: valorNovo,
      };
      dispatch(setDadosParaSalvarCartaIntencoes(dadosEmEdicao));
    },
    [dispatch, bimestre, periodoEscolarId, id]
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
        <JoditEditor
          validarSeTemErro={validarSeTemErro}
          mensagemErro="Campo obrigatório"
          id={`bimestre-${bimestre}-editor`}
          value={planejamento}
          onChange={valorNovo => {
            onChange(valorNovo);
            dispatch(setCartaIntencoesEmEdicao(true));
          }}
          desabilitar={desabilitarCampo}
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
  carta: PropTypes.oneOfType([PropTypes.object]),
  permissoesTela: PropTypes.oneOfType([PropTypes.object]),
  somenteConsulta: PropTypes.bool,
};

BimestresCartaIntencoes.defaultProps = {
  carta: {},
  permissoesTela: {},
  somenteConsulta: false,
};

export default BimestresCartaIntencoes;
