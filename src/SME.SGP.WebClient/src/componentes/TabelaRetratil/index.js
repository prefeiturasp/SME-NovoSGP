import React from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Estilos
import { TabelaEstilo, Tabela, DetalhesAluno, LinhaTabela } from './style';

function TabelaRetratil({ alunos, children }) {
  const selecionarAluno = aluno => {
    console.log(aluno);
  };

  return (
    <TabelaEstilo>
      <Tabela>
        <thead>
          <tr>
            <th>Nº</th>
            <th>Nome</th>
          </tr>
        </thead>
        <tbody>
          {alunos.map(item => (
            <LinhaTabela
              key={shortid.generate()}
              ativo={item.ativo}
              onClick={item => selecionarAluno(item)}
            >
              <td>{item.numeroChamada}</td>
              <td>{item.nome}</td>
            </LinhaTabela>
          ))}
        </tbody>
      </Tabela>
      <DetalhesAluno>{children}</DetalhesAluno>
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
