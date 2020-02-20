import React, { useState, useCallback } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Ant
import { Tooltip } from 'antd';

// Componentes
import Cabecalho from './componentes/Cabecalho';

// Estilos
import { TabelaEstilo, Tabela, DetalhesAluno, LinhaTabela } from './style';

function TabelaRetratil({ alunos, children }) {
  const [retraido, setRetraido] = useState(false);
  const [alunoSelecionado, setAlunoSelecionado] = useState(null);

  const selecionarAluno = aluno => {
    setAlunoSelecionado(aluno);
  };

  const isAlunoSelecionado = aluno => {
    return alunoSelecionado && aluno.id === alunoSelecionado.id;
  };

  const proximoAlunoHandler = useCallback(() => {
    if (alunos.indexOf(alunoSelecionado) === alunos.length - 1) return;
    selecionarAluno(alunos[alunos.indexOf(alunoSelecionado) + 1]);
  }, [alunoSelecionado, alunos]);

  const anteriorAlunoHandler = useCallback(() => {
    if (alunos.indexOf(alunoSelecionado) === 0) return;
    selecionarAluno(alunos[alunos.indexOf(alunoSelecionado) - 1]);
  }, [alunoSelecionado, alunos]);

  const desabilitarAnterior = () => {
    return (
      alunos.length === 0 ||
      alunos.indexOf(alunoSelecionado) === 0 ||
      !alunoSelecionado
    );
  };

  const desabilitarProximo = () => {
    return (
      alunos.length === 0 ||
      alunos.indexOf(alunoSelecionado) === alunos.length - 1 ||
      !alunoSelecionado
    );
  };

  return (
    <TabelaEstilo>
      <div className="tabelaCollapse">
        <Tabela className={retraido && `retraido`}>
          <thead>
            <tr>
              <th>Nº</th>
              <th>Nome</th>
            </tr>
          </thead>
          <tbody>
            {alunos.map(item => (
              <LinhaTabela
                className={isAlunoSelecionado(item) && `selecionado`}
                key={shortid.generate()}
                ativo={item.ativo}
                onClick={() => selecionarAluno(item)}
              >
                <td>
                  {item.numeroChamada}
                  {item.situacao !== null && (
                    <Tooltip title={item.situacao}>
                      <span className="iconeSituacao" />
                    </Tooltip>
                  )}
                </td>
                <td>{item.nome}</td>
              </LinhaTabela>
            ))}
          </tbody>
        </Tabela>
      </div>
      <DetalhesAluno>
        <Cabecalho
          titulo="Detalhes do Estudante"
          retraido={retraido}
          onClickCollapse={() => setRetraido(!retraido)}
          onClickAnterior={anteriorAlunoHandler}
          onClickProximo={proximoAlunoHandler}
          desabilitarAnterior={desabilitarAnterior()}
          desabilitarProximo={desabilitarProximo()}
        />
        {children}
      </DetalhesAluno>
    </TabelaEstilo>
  );
}

TabelaRetratil.propTypes = {
  alunos: t.oneOfType([t.array]),
  children: t.oneOfType([t.element, t.func]),
};

TabelaRetratil.defaultProps = {
  alunos: [],
  children: () => {},
};

export default TabelaRetratil;
