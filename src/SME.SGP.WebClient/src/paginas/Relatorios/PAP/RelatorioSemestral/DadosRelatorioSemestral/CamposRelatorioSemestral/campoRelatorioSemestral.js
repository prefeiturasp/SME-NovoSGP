import PropTypes from 'prop-types';
import React, { useCallback, useState } from 'react';
import { useDispatch } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';
import {
  setDadosParaSalvarRelatorioSemestral,
  setRelatorioSemestralEmEdicao,
} from '~/redux/modulos/relatorioSemestral/actions';

const CampoRelatorioSemestral = props => {
  const { descricao, idSecao, nome, valor, alunoDesabilitado } = props;

  const [exibirCard, setExibirCard] = useState(false);

  const dispatch = useDispatch();

  const onClickExpandir = () => setExibirCard(!exibirCard);

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

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2 mt-4">
      <CardCollapse
        key={`secao-${idSecao}-estudante-collapse-key`}
        onClick={onClickExpandir}
        titulo={nome}
        indice={`secao-${idSecao}-estudante-collapse-indice`}
        show={exibirCard}
        alt={`secao-${idSecao}-estudante-alt`}
      >
        {exibirCard ? (
          <Editor
            label={descricao}
            id={`secao-${idSecao}-estudante-editor`}
            inicial={valor}
            onChange={valorNovo => {
              onChange(valorNovo);
              setarRelatorioSemestralEmEdicao(true);
            }}
            desabilitar={alunoDesabilitado}
          />
        ) : (
          ''
        )}
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
};

CampoRelatorioSemestral.defaultProps = {
  descricao: '',
  idSecao: '',
  nome: '',
  valor: '',
  alunoDesabilitado: false,
};

export default CampoRelatorioSemestral;
