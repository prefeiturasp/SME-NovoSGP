import React, { useState } from 'react';
import { Divider } from 'antd';
import shortid from 'shortid';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import {
  Base,
  Button,
  Colors,
  JoditEditor,
  PainelCollapse,
} from '~/componentes';

const CollapseAluno = ({ aluno, removerAlunos, desabilitar }) => {
  const [acompanhamentoSituacao, setAcompanhamentoSituacao] = useState();
  const [descritivoEstudante, setDescritivoEstudante] = useState();
  const [encaminhamentos, setEncaminhamentos] = useState();
  const dados = useSelector(
    store => store.itinerancia.questoesItineranciaAluno
  );

  const id = 'collapseAluno';

  return (
    <div className="row mb-4">
      <div className="col-12">
        <PainelCollapse onChange={() => {}}>
          <PainelCollapse.Painel
            key={id}
            espacoPadrao
            corBorda={Base.AzulBordaCollapse}
            temBorda
            header={aluno.alunoNome}
          >
            {dados &&
              dados.map(questao => {
                return (
                  <div className="row mb-4">
                    <div className="col-12">
                      <JoditEditor
                        label={questao.descricao}
                        value=""
                        name={questao.descricao + questao.questaoId}
                        id={questao.questaoId}
                        onChange={e => setAcompanhamentoSituacao(e)}
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
};

CollapseAluno.propTypes = {
  aluno: PropTypes.instanceOf(PropTypes.any),
  removerAlunos: PropTypes.func,
  desabilitar: PropTypes.bool,
};

export default CollapseAluno;
