import React, { useState, useEffect } from 'react';
import { Ordenacao } from '~/componentes-sgp';
import { Lista } from './fechamentoFinal.css';
import { Card, Auditoria } from '~/componentes';
import CampoConceito from '~/componentes-sgp/avaliacao/campoConceito';
import CampoNota from '~/componentes-sgp/avaliacao/campoNota';
import CampoNumero from '~/componentes/campoNumero';

const FechamentoFinal = () => {
  const [ehNota, setEhNota] = useState(true);
  const [auditoria, setAuditoria] = useState({
    criadoPor: '123',
    criadoEm: '',
    alteradoPor: '123',
    alteradoEm: '123',
    criadoRf: '123',
    alteradoRf: '123',
  });
  const [alunos, setAlunos] = useState([
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 2,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 3,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 4,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceito: [8.5],
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      notas: [1, 2, 3, 4],
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
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
                setAlunos(retorno);
              }}
            />
          </div>
          <table className="table mt-4">
            <thead className="tabela-fechamento-final-thead">
              <tr>
                <th className="col-nome-aluno" colSpan="2">
                  Nome
                </th>
                <th className="sticky-col">{ehNota ? 'Nota' : 'Conceito'}</th>
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
                      <td className="sticky-col col-nota-conceito">
                        {aluno.notas.map(c => (
                          <div className="input-notas">{c}</div>
                        ))}
                      </td>
                      <td className="sticky-col">{aluno.totalFaltas}</td>
                      <td className="sticky-col">
                        {aluno.totalAusenciasCompensadas}
                      </td>
                      <td className="sticky-col">{aluno.frequencia}%</td>
                      <td className="sticky-col col-conceito-final">
                        {aluno.notasConceito.map(nota => (
                          <CampoNumero
                            value={nota}
                            min={0}
                            max={10}
                            step={0.5}
                            placeholder="Nota"
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
      <pre>{JSON.stringify(alunos)}</pre>
      <Auditoria
        criadoPor={auditoria.criadoPor}
        criadoEm={auditoria.criadoEm}
        alteradoPor={auditoria.alteradoPor}
        alteradoEm={auditoria.alteradoEm}
        criadoRf={auditoria.criadoRf}
        alteradoRf={auditoria.alteradoRf}
      />
    </Card>
  );
};

export default FechamentoFinal;
