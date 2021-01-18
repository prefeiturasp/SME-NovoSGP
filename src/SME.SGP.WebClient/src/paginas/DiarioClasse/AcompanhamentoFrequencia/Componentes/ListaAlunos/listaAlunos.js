import React from 'react';
import { TabelaColunasFixas } from './listaAlunos.css';
import BtnExpandirFrequenciaAluno from './btnExpandirFrequenciaAluno';
import AusenciasAluno from './ausenciasAluno';

const ListaAlunos = () => {
  const dados = [
    {
      bimestreId: 1,
      alunos: [
        {
          numeroChamada: 1,
          nome: 'Aluno com um nome grande',
          compensacoes: 1,
          frequencia: '90%',
          ausencias: [
            {
              data: '12/10/2020',
              motivo: 'Atestado médico do estudante',
            },
          ],
        },
        {
          numeroChamada: 2,
          nome: 'Aluno com um nome grande 2 ',
          compensacoes: 1,
          frequencia: '90%',
          ausencias: [
            {
              data: '12/10/2020',
              motivo: 'Atestado médico do estudante',
              anotacao:
                '<strong>Atestado médico do estudante Atestado médico do estudante Atestado médico do estudante Atestado médico do estudante</strong> Amaral dos Santos',
            },
          ],
        },
      ],
    },
  ];

  return (
    <>
      <TabelaColunasFixas>
        <div className="wrapper">
          <div className="header-fixo">
            <table className="table">
              <thead className="tabela-dois-thead">
                <tr>
                  <th className="col-linha-quatro" colSpan="2">
                    Nome
                  </th>
                  <th className="col-linha-dois">Ausências no Bimestre</th>
                  <th className="col-linha-dois">Compensações de ausência</th>
                  <th className="col-linha-dois">Frequência</th>
                </tr>
              </thead>
              <tbody className="tabela-um-tbody">
                {dados[0].alunos.map((data, index) => {
                  return (
                    <>
                      <tr id={index}>
                        <td className="col-valor-linha-tres">
                          <strong>{data.numeroChamada}</strong>
                        </td>
                        <td className="col-valor-linha-quatro">{data.nome}</td>
                        <td className="col-valor-linha-dois">
                          {data.ausencias.length}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.compensacoes}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.frequencia}
                          <BtnExpandirFrequenciaAluno indexLinha={index} />
                        </td>
                      </tr>
                      <AusenciasAluno
                        indexLinha={index}
                        dados={data.ausencias}
                      />
                    </>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      </TabelaColunasFixas>
    </>
  );
};

export default ListaAlunos;
