import React, { useState, useCallback } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Ant
import { Tooltip } from 'antd';

// Componentes
import Cabecalho from './componentes/Cabecalho';

// Estilos
import { TabelaEstilo, Tabela, DetalhesAluno, LinhaTabela } from './style';

function TabelaRetratil({
  alunos,
  children,
  onChangeAlunoSelecionado,
  permiteOnChangeAluno,
}) {
  const [retraido, setRetraido] = useState(false);
  const [alunoSelecionado, setAlunoSelecionado] = useState(null);

  const permiteSelecionarAluno = useCallback(async () => {
    if (permiteOnChangeAluno) {
      const permite = await permiteOnChangeAluno();
      return permite;
    }
    return true;
  }, [permiteOnChangeAluno]);

  const selecionarAluno = async aluno => {
    const permite = await permiteSelecionarAluno();
    if (permite) {
      setAlunoSelecionado(aluno);
    }
  };

  const isAlunoSelecionado = aluno => {
    return alunoSelecionado && aluno.codigoEOL === alunoSelecionado.codigoEOL;
  };

  const proximoAlunoHandler = useCallback(async () => {
    const permite = await permiteSelecionarAluno();
    if (permite) {
      if (alunos.indexOf(alunoSelecionado) === alunos.length - 1) return;
      const aluno = alunos[alunos.indexOf(alunoSelecionado) + 1];
      setAlunoSelecionado(aluno);
      onChangeAlunoSelecionado(aluno);
    }
  }, [
    alunoSelecionado,
    alunos,
    onChangeAlunoSelecionado,
    permiteSelecionarAluno,
  ]);

  const anteriorAlunoHandler = useCallback(async () => {
    const permite = await permiteSelecionarAluno();
    if (permite) {
      if (alunos.indexOf(alunoSelecionado) === 0) return;
      const aluno = alunos[alunos.indexOf(alunoSelecionado) - 1];
      setAlunoSelecionado(aluno);
      onChangeAlunoSelecionado(aluno);
    }
  }, [
    alunoSelecionado,
    alunos,
    onChangeAlunoSelecionado,
    permiteSelecionarAluno,
  ]);

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

  const onClickLinhaAluno = async aluno => {
    const permite = await permiteSelecionarAluno();
    if (permite) {
      selecionarAluno(aluno);
      onChangeAlunoSelecionado(aluno);
    }
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
                ativo={!item.desabilitado}
                onClick={() => onClickLinhaAluno(item)}
              >
                <td>
                  {item.numeroChamada}
                  {item.marcador && item.marcador.descricao ? (
                    <Tooltip title={item.marcador.descricao}>
                      <span className="iconeSituacao" />
                    </Tooltip>
                  ) : (
                    ''
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
  onChangeAlunoSelecionado: t.func,
  permiteOnChangeAluno: t.func,
};

TabelaRetratil.defaultProps = {
  alunos: [],
  children: () => {},
  onChangeAlunoSelecionado: () => {},
  permiteOnChangeAluno: () => true,
};

export default TabelaRetratil;
