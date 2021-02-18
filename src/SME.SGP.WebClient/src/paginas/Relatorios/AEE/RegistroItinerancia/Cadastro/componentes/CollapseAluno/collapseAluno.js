import { Divider } from 'antd';
import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React from 'react';
import shortid from 'shortid';
import {
  Base,
  Button,
  Colors,
  JoditEditor,
  PainelCollapse,
} from '~/componentes';

const CollapseAluno = ({
  aluno,
  removerAlunos,
  desabilitar,
  setModoEdicaoItinerancia,
}) => {
  const id = 'collapseAluno';
  const onChangeResposta = (valor, questao) => {
    questao.resposta = valor;
    setModoEdicaoItinerancia(true);
  };

  return (
    <div className="row mb-4">
      <div className="col-12">
        <PainelCollapse>
          <PainelCollapse.Painel
            key={`${id}-${aluno.alunoCodigo}`}
            espacoPadrao
            corBorda={Base.AzulBordaCollapse}
            temBorda
            header={aluno.nomeComModalidadeTurma}
          >
            {aluno.questoes?.map(questao => {
              return (
                <div className="row mb-4">
                  <div className="col-12">
                    <JoditEditor
                      label={questao.descricao}
                      value={questao.resposta}
                      name={questao.descricao + questao.questaoId}
                      id={questao.questaoId}
                      onChange={e => onChangeResposta(e, questao)}
                      desabilitar={desabilitar}
                    />
                  </div>
                </div>
              );
            })}
            <div className="row mt-n2">
              <Divider />
            </div>
            <div className="row">
              <div className="col-sm-12 d-flex justify-content-end">
                <Button
                  id={shortid.generate()}
                  label="Remover estudante"
                  icon="user-minus"
                  color={Colors.Azul}
                  border
                  onClick={removerAlunos}
                  disabled={desabilitar}
                />
              </div>
            </div>
          </PainelCollapse.Painel>
        </PainelCollapse>
      </div>
    </div>
  );
};

CollapseAluno.defaultProps = {
  aluno: [],
  removerAlunos: () => {},
  desabilitar: false,
  setModoEdicaoItinerancia: () => {},
};

CollapseAluno.propTypes = {
  aluno: PropTypes.instanceOf(PropTypes.any),
  removerAlunos: PropTypes.func,
  desabilitar: PropTypes.bool,
  setModoEdicaoItinerancia: PropTypes.bool,
};

export default CollapseAluno;
