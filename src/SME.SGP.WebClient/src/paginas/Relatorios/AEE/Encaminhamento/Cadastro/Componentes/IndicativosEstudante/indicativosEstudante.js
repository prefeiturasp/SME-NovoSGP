import React from 'react';
import AusenciasEstudante from './ausenciasEstudante';
import BtnExpandirAusenciaEstudante from './btnExpandirAusenciaEstudante';
import { TabelaColunasFixas } from './indicativosEstudante.css';
import ModalAnotacoesEncaminhamentoAEE from './modalAnotacoes';

const InformacoesEscolares = () => {
  const dados = [
    {
      bimestre: 1,
      ausencias: [
        {
          data: '20/10/2020',
          registradoPor: '123 Amaral dos Santos',
          motivo: 'Atestado médico do estudante',
        },
        {
          data: '12/10/2020',
          registradoPor: '33 Amaral dos Santos',
          motivo: 'Atestado médico do estudante',
          anotacao:
            '<strong>AOSUDHÇOU ASHD 18273 HASD0871 23HA0S8YD H12093 YHANSD09 1Y23 GAS09DY 2</strong> 123',
        },
      ],
      compensacoes: 1,
      frequencia: '90%',
    },
    {
      bimestre: 1,
      ausencias: [
        {
          data: '20/10/2020',
          registradoPor: 'Fernanda Amaral dos Santos',
          motivo: 'Atestado médico do estudante',
        },
        {
          data: '12/10/2020',
          registradoPor: 'Fernanda Amaral dos Santos',
          motivo: 'Atestado médico do estudante',
        },
      ],
      compensacoes: 1,
      frequencia: '90%',
    },
  ];

  return (
    <>
      <ModalAnotacoesEncaminhamentoAEE />
      <TabelaColunasFixas>
        <div className="wrapper">
          <div className="header-fixo">
            <table className="table">
              <thead className="tabela-um-thead">
                <tr>
                  <th className="col-linha-um">
                    Indicativo de deficiência (EOL)
                  </th>
                  <th className="col-linha-um">Recursos utilizados (EOL)</th>
                  <th className="col-linha-um">Frequência Global</th>
                </tr>
              </thead>
              <tbody className="tabela-um-tbody">
                <tr>
                  <td className="col-valor-linha-um">Baixa Visão</td>
                  <td className="col-valor-linha-um">Auxílio Ledor</td>
                  <td className="col-valor-linha-um">97%</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </TabelaColunasFixas>

      <TabelaColunasFixas>
        <div className="wrapper">
          <div className="header-fixo">
            <table className="table">
              <thead className="tabela-um-thead">
                <tr>
                  <th className="col-linha-dois">Bimestre</th>
                  <th className="col-linha-dois">Ausências no Bimestre</th>
                  <th className="col-linha-dois">Compensações de ausência</th>
                  <th className="col-linha-dois">Frequência</th>
                </tr>
              </thead>
              <tbody className="tabela-um-tbody">
                {dados.map((data, index) => {
                  return (
                    <>
                      <tr id={index}>
                        <td className="col-valor-linha-dois">
                          {data.bimestre}°
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.ausencias.length}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.compensacoes}
                        </td>
                        <td className="col-valor-linha-dois">
                          {data.frequencia}
                          <BtnExpandirAusenciaEstudante indexLinha={index} />
                        </td>
                      </tr>
                      <AusenciasEstudante indexLinha={index} dados={data} />
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

export default InformacoesEscolares;
