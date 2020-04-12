import React from 'react';
import { Tabela, BarraLateralAzul } from './ComponenteSemNota.css';
import shortid from 'shortid';

const ComponenteSemNota = props => {
  const { dados, nomeColunaComponente, corBorda } = props;
  return (
    <Tabela>
      <div className="table-responsive pl-2 pr-2">
        <table className="table mt-2">
          <thead className="tabela-componente-sem-nota-thead">
            <tr>
              <th
                colSpan="2"
                className="coluna-componente sombra-direita"
                style={{ paddingLeft: '27px' }}
              >
                {nomeColunaComponente}
              </th>
              <th>Faltas</th>
              <th>%Freq.</th>
            </tr>
          </thead>
          <tbody className="tabela-componente-sem-nota-tbody">
            {dados.map(item => {
              return (
                <tr key={shortid.generate()}>
                  <BarraLateralAzul corBorda={corBorda} />
                  <td
                    className="coluna-componente sombra-direita"
                    style={{
                      textAlign: 'left',
                      paddingLeft: '20px',
                      width: '250px',
                    }}
                  >
                    {item.componente}
                  </td>
                  <td
                    style={{
                      width: '250px',
                    }}
                  >
                    {item.faltas}
                  </td>
                  <td>{item.frequencia}%</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </Tabela>
  );
};

export default ComponenteSemNota;
