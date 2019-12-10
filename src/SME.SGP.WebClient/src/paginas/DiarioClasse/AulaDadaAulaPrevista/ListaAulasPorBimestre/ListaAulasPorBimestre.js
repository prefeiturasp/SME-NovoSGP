import React, { useState, useEffect } from 'react';
import { Corpo, CampoDesabilitado, CampoEditavel, CampoAlerta, CampoCentralizado } from './ListaAulasPorBimestre.css';
import { CampoTexto } from '~/componentes';
import CampoNumero from '~/componentes/campoNumero';

const ListaAulasPorBimestre = props => {
  const { dados } = props;
  const { totalPrevistas, totalCriadasProfTitular, totalCriadasProfCj, totalDadas, totalRespostas } = dados;

  const formatarData = data => {
    return window.moment(data).format('DD/MM');
  }

  const temProfessorCj = dados.bimestres && dados.bimestres[0].criadas.professorCj;

  return (
    <Corpo>
      <table className="table mb-0">
        <thead className="tabela-frequencia-thead" key="thead">
          <tr>
            <th rowSpan="2" className="width-60 bc-w-i" scope="col"></th>
            <th rowSpan="2" className="text-center fundo-cinza">
              Previstas
            </th>
            <th colSpan={temProfessorCj ? 2 : 1} className="text-center fundo-cinza">
              Criadas
            </th>
            <th rowSpan="2" className="text-center fundo-cinza">
              Dadas
            </th>
            <th rowSpan="2" className="text-center fundo-cinza">
              Repostas
            </th>
          </tr>
          {temProfessorCj ?
            <tr>
              <th className="text-center fundo-cinza">
                Prof. Títular
            </th>
              <th className="text-center fundo-cinza">
                Prof. Substituto
            </th>
            </tr>
            : null}
        </thead>
        {dados && dados.bimestres ? dados.bimestres.map(item => {
          return (
            <tbody key={`lista-${item.bimestre}`}>
              <tr>
                <td className="fundo-cinza">
                  <span className="negrito">{`${item.bimestre}º Bimestre`}</span>
                  <span>{` - ${formatarData(item.inicio)} à ${formatarData(item.fim)}`}</span>
                </td>
                <td>
                  {item.previstas.temDivergencia ?
                    <CampoCentralizado className="p-l-16">
                      <CampoAlerta>
                        <CampoNumero
                          value={item.previstas.quantidade}
                          onChange={() => { }}
                          onKeyDown={() => { }}
                          min={0}
                        />
                        <div className="icone">
                          <i className="fas fa-exclamation-triangle"></i>
                        </div>
                      </CampoAlerta>
                    </CampoCentralizado>
                    : <CampoEditavel>
                      <CampoNumero
                        value={item.previstas.quantidade}
                        onChange={() => { }}
                        onKeyDown={() => { }}
                        min={0}
                      />
                    </CampoEditavel>}
                </td>
                <td>
                  <CampoDesabilitado>
                    <span>{item.criadas.professorTitular}</span>
                  </CampoDesabilitado>
                </td>
                {temProfessorCj ?
                  <td>
                    <CampoDesabilitado>
                      <span>{item.criadas.professorCj}</span>
                    </CampoDesabilitado>
                  </td>
                  : null}
                <td>
                  <CampoDesabilitado>
                    <span>{item.dadas}</span>
                  </CampoDesabilitado>
                </td>
                <td>
                  <CampoDesabilitado>
                    <span>{item.respostas}</span>
                  </CampoDesabilitado>
                </td>
              </tr>
            </tbody>
          );
        })
          : null}
        <tbody key="coluna-lateral">
          <tr className="fundo-cinza-i">
            <th className="fundo-cinza">
              <span className="negrito">Total</span>
            </th>
            <td>
              <CampoDesabilitado>
                <span>{totalPrevistas}</span>
              </CampoDesabilitado>
            </td>
            <td>
              <CampoDesabilitado>
                <span>{totalCriadasProfTitular}</span>
              </CampoDesabilitado>
            </td>
            {temProfessorCj ?
              <td>
                <CampoDesabilitado>
                  <span>{totalCriadasProfCj}</span>
                </CampoDesabilitado>
              </td>
              : null}
            <td>
              <CampoDesabilitado>
                <span>{totalDadas}</span>
              </CampoDesabilitado>
            </td>
            <td>
              <CampoDesabilitado>
                <span>{totalRespostas}</span>
              </CampoDesabilitado>
            </td>
          </tr>
        </tbody>
      </table>
    </Corpo>
  )
}

export default ListaAulasPorBimestre;
