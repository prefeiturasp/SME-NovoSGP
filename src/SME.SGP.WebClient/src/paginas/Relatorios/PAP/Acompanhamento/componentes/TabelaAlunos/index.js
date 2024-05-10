import React, { useEffect } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Componentes
import styled from 'styled-components';
import IconeStatus from './componentes/IconeStatus';
import SelectRespostas from './componentes/SelectRespostas';

// Estilos
import { Tabela, ContainerTabela } from './styles';
import NomeEstudanteLista from '~/componentes-sgp/NomeEstudanteLista/nomeEstudanteLista';

const NumeroChamada = styled.td`
  min-width: 20px;
`;

function TabelaAlunos({
  alunos,
  respostas,
  objetivoAtivo,
  onChangeResposta,
  somenteConsulta,
}) {
  return (
    <ContainerTabela>
      <Tabela>
        <thead>
          <tr>
            <td colSpan="2">Estudante</td>
            <td>Concluído</td>
            <td>Turma regular</td>
            <td>Respostas</td>
          </tr>
        </thead>
        <tbody>
          {alunos.length > 0 ? (
            alunos.map((aluno, key) => (
              <tr key={shortid.generate()}>
                <NumeroChamada>{aluno.numeroChamada}</NumeroChamada>
                <td className="w-100">
                  <NomeEstudanteLista
                    nome={aluno?.nome}
                    exibirSinalizacao={aluno?.ehAtendidoAEE}
                  />
                </td>
                <td>
                  <IconeStatus status={aluno.concluido} />
                </td>
                <td>{aluno.turma}</td>
                <td id={`resposta-${key}`}>
                  <div className="opcaoSelect">
                    <SelectRespostas
                      aluno={aluno}
                      objetivoAtivo={objetivoAtivo}
                      respostas={respostas}
                      onChangeResposta={onChangeResposta}
                      containerVinculoId={`resposta-${key}`}
                      desabilitar={somenteConsulta}
                      bloquearLimpar={
                        somenteConsulta ||
                        (objetivoAtivo &&
                          (objetivoAtivo.id == 1 || objetivoAtivo.id == 2))
                      }
                    />
                  </div>
                </td>
              </tr>
            ))
          ) : (
            <tr className="semConteudo">
              <td>Sem dados</td>
            </tr>
          )}
        </tbody>
      </Tabela>
    </ContainerTabela>
  );
}

TabelaAlunos.propTypes = {
  alunos: t.oneOfType([t.array]),
};

TabelaAlunos.defaultProps = {
  alunos: [],
};

export default TabelaAlunos;
