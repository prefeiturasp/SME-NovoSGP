import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';

const RecomendacaoAlunoFamilia = props => {
  const {
    onChangeRecomendacaoAluno,
    onChangeRecomendacaoFamilia,
    dadosIniciais,
    alunoDesabilitado,
  } = props;

  const dentroPeriodo = useSelector(
    store => store.conselhoClasse.dentroPeriodo
  );

  const desabilitarCampos = useSelector(
    store => store.conselhoClasse.desabilitarCampos
  );

  const [exibirCardRecomendacao, setExibirCardRecomendacao] = useState(true);

  const onClickExpandirRecomendacao = () =>
    setExibirCardRecomendacao(!exibirCardRecomendacao);

  const onChangeAluno = valor => onChangeRecomendacaoAluno(valor);

  const onChangeFamilia = valor => onChangeRecomendacaoFamilia(valor);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="recomendacao-aluno-familia-collapse"
        onClick={onClickExpandirRecomendacao}
        titulo="Recomendações ao estudante e a família"
        indice="recomendacao-aluno-familia-collapse"
        show={exibirCardRecomendacao}
        alt="recomendacao-aluno-familia"
      >
        {exibirCardRecomendacao ? (
          <div className="row">
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
              <JoditEditor
                label="Recomendações ao estudante"
                id="recomendacao-aluno"
                value={dadosIniciais.recomendacaoAluno}
                onChange={onChangeAluno}
                desabilitar={
                  alunoDesabilitado || desabilitarCampos || !dentroPeriodo
                }
                height="300px"
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6">
              <JoditEditor
                label="Recomendações a família "
                id="recomendacao-familia"
                value={dadosIniciais.recomendacaoFamilia}
                onChange={onChangeFamilia}
                desabilitar={
                  alunoDesabilitado || desabilitarCampos || !dentroPeriodo
                }
                height="300px"
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
