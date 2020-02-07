import React, { useState, useEffect } from 'react';
import { Ordenacao } from '~/componentes-sgp';
import { Lista } from './fechamentoFinal.css';
import { Card } from '~/componentes';
import CampoConceito from '~/componentes-sgp/avaliacao/campoConceito';
import CampoNota from '~/componentes-sgp/avaliacao/campoNota';
import CampoNumero from '~/componentes/campoNumero';

const FechamentoFinal = () => {
  const onChangeOrdenacao = () => {};
  const [dados, setDados] = useState([]);
  const [alunos, setAlunos] = useState([
    {
      nome: 'alvaro',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      conceitoFinal: 8.5,
      notasConceito: [8.5],
    },
  ]);

  return (
    <Card>
      <div className="col-md-12">
        <Lista>
          <div className="botao-ordenacao-avaliacao">
            <Ordenacao
              conteudoParaOrdenar={alunos}
              ordenarColunaNumero="numeroChamada"
              ordenarColunaTexto="nome"
              retornoOrdenado={retorno => {
                onChangeOrdenacao(alunos);
              }}
            />
          </div>
          <table className="table mt-4">
            <thead className="tabela-fechamento-final-thead">
              <tr>
                <th className="col-nome-aluno" colSpan="2">
                  Nome
                </th>
                <th className="sticky-col col-nota-conceito">Nota</th>
                <th className="sticky-col width-120">Total de Faltas</th>
                <th className="sticky-col">Total de Ausências Compensadas</th>
                <th className="sticky-col">%Freq.</th>
                <th className="sticky-col">Conceito Final</th>
              </tr>
            </thead>
            <tbody className="tabela-fechamento-final-tbody">
              {alunos.map((aluno, i) => {
                return (
                  <>
                    <tr>
                      <td className="sticky-col col-numero-chamada">
                        {aluno.numeroChamada}
                      </td>
                      <td className="sticky-col col-nome-aluno">
                        {aluno.nome}
                      </td>
                      <td className="sticky-col col-notas">
                        {aluno.notas.map(c => (
                          <div className="input-notas">{c}</div>
                        ))}
                      </td>
                      <td className="sticky-col">{aluno.totalFaltas}</td>
                      <td className="sticky-col">
                        {aluno.totalAusenciasCompensadas}
                      </td>
                      <td className="sticky-col">{aluno.frequencia}%</td>
                      <td className="sticky-col">
                        {aluno.notasConceito.map(nota => (
                          <CampoNumero
                            min={0}
                            max={10}
                            step={0.5}
                            placeholder="Nota"
                            classNameCampo={`${
                              nota.ausente
                                ? 'aluno-ausente-notas'
                                : 'aluno-notas'
                            }`}
                          />
                        ))}
                      </td>
                    </tr>
                  </>
                );
              })}
            </tbody>
          </table>
        </Lista>
      </div>
    </Card>
  );
};

export default FechamentoFinal;
