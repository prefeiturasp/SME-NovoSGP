import PropTypes from 'prop-types';
import React, { useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setDadosParaSalvarRelatorioSemestral,
  setRelatorioSemestralEmEdicao,
} from '~/redux/modulos/relatorioSemestralPAP/actions';

const CampoRelatorioSemestral = props => {
  const {
    descricao,
    idSecao,
    nome,
    valor,
    alunoDesabilitado,
    obrigatorio,
  } = props;

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestralPAP.desabilitarCampos
  );

  const dentroPeriodo = useSelector(
    store => store.relatorioSemestralPAP.dentroPeriodo
  );

  const dispatch = useDispatch();

  const onChange = useCallback(
    valorNovo => {
      const dadosEmEdicao = {
        id: idSecao,
        valor: valorNovo,
      };
      dispatch(setDadosParaSalvarRelatorioSemestral(dadosEmEdicao));
    },
    [dispatch, idSecao]
  );

  const setarRelatorioSemestralEmEdicao = emEdicao => {
    dispatch(setRelatorioSemestralEmEdicao(emEdicao));
  };

  const validarSeTemErro = valorEditado => {
    if (obrigatorio && !valorEditado) {
      return true;
    }
    return false;
  };

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2 mt-4">
      <CardCollapse
        key={`secao-${idSecao}-estudante-collapse-key`}
        titulo={nome}
        indice={`secao-${idSecao}-estudante-collapse-indice`}
        alt={`secao-${idSecao}-estudante-alt`}
        show
      >
        <JoditEditor
          validarSeTemErro={validarSeTemErro}
          mensagemErro="Campo obrigatório"
          label={descricao}
          id={`secao-${idSecao}-estudante-editor`}
          value={valor || ''}
          onChange={valorNovo => {
            onChange(valorNovo);
            setarRelatorioSemestralEmEdicao(true);
          }}
          desabilitar={alunoDesabilitado || !dentroPeriodo || desabilitarCampos}
          height="350px"
        />
      </CardCollapse>
    </div>
  );
};

CampoRelatorioSemestral.propTypes = {
  descricao: PropTypes.string,
  idSecao: PropTypes.oneOfType([PropTypes.any]),
  nome: PropTypes.string,
  valor: PropTypes.oneOfType([PropTypes.any]),
  alunoDesabilitado: PropTypes.bool,
  obrigatorio: PropTypes.bool,
};

CampoRelatorioSemestral.defaultProps = {
  descricao: '',
  idSecao: '',
  nome: '',
  valor: '',
  alunoDesabilitado: false,
  obrigatorio: false,
};

export default CampoRelatorioSemestral;
