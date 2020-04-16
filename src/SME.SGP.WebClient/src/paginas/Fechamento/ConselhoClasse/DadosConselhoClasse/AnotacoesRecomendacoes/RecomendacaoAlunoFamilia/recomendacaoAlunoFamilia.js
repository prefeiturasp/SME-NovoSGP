import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';

const RecomendacaoAlunoFamilia = props => {
  const {
    onChangeRecomendacaoAluno,
    onChangeRecomendacaoFamilia,
    dadosIniciais,
    alunoDesabilitado,
  } = props;

  const [exibirCardRecomendacao, setExibirCardRecomendacao] = useState(false);

  const onClickExpandirRecomendacao = () =>
    setExibirCardRecomendacao(!exibirCardRecomendacao);

  const onChangeAluno = valor => onChangeRecomendacaoAluno(valor);

  const onChangeFamilia = valor => onChangeRecomendacaoFamilia(valor);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="recomendacao-aluno-familia-collapse"
        onClick={onClickExpandirRecomendacao}
        titulo="Recomendações ao aluno e a família"
        indice="recomendacao-aluno-familia-collapse"
        show={exibirCardRecomendacao}
        alt="recomendacao-aluno-familia"
      >
        {exibirCardRecomendacao ? (
          <div className="row">
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
              <Editor
                label="Recomendações ao aluno"
                id="recomendacao-aluno"
                inicial={dadosIniciais.recomendacaoAluno}
                onChange={onChangeAluno}
                desabilitar={alunoDesabilitado}
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
              <Editor
                label="Recomendações a família "
                id="recomendacao-familia"
                inicial={dadosIniciais.recomendacaoFamilia}
                onChange={onChangeFamilia}
                desabilitar={alunoDesabilitado}
              />
            </div>
          </div>
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

RecomendacaoAlunoFamilia.propTypes = {
  onChangeRecomendacaoAluno: PropTypes.func,
  onChangeRecomendacaoFamilia: PropTypes.func,
  dadosIniciais: PropTypes.oneOfType([PropTypes.object]),
  alunoDesabilitado: PropTypes.bool,
};

RecomendacaoAlunoFamilia.defaultProps = {
  onChangeRecomendacaoAluno: () => {},
  onChangeRecomendacaoFamilia: () => {},
  dadosIniciais: {},
  alunoDesabilitado: false,
};

export default RecomendacaoAlunoFamilia;
