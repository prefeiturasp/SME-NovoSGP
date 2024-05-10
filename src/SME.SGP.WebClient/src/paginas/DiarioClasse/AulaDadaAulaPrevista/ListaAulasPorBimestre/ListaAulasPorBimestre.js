import React, { useState, useEffect } from 'react';
import {
  Corpo,
  CampoDesabilitado,
  CampoEditavel,
  CampoAlerta,
  CampoCentralizado,
} from './ListaAulasPorBimestre.css';
import CampoNumero from '~/componentes/campoNumero';
import { Tooltip } from 'antd';

const ListaAulasPorBimestre = props => {
  const { dados, setModoEdicao, permissoesTela, somenteConsulta } = props;
  const {
    totalCriadasTitular,
    totalCriadasCj,
    totalDadas,
    totalRepostas,
  } = dados;
  const [totalPrevistas, setTotalPrevistas] = useState(dados.totalPrevistas);
  const [bimestres, setBimestres] = useState(dados.bimestres);

  useEffect(() => {
    setBimestres([...dados.bimestres]);
    setTotalPrevistas(dados.totalPrevistas);
  }, [dados.bimestres]);

  const formatarData = data => {
    return window.moment(data).format('DD/MM');
  };

  const desabilitaCampos = () => {
    if (dados && dados.id > 0) {
      return !permissoesTela.podeAlterar || somenteConsulta;
    }
    return !permissoesTela.podeIncluir || somenteConsulta;
  };

  const temProfessorCj = totalCriadasCj > 0;

  const alterarValorPrevisto = (index, valor) => {
    if (valor >= 0) {
      bimestres[index].previstas.quantidade = valor;
      setBimestres([...bimestres]);
      let total = 0;
      bimestres.forEach(bimestre => {
        total += bimestre.previstas.quantidade;
      });
      setModoEdicao(true);
      setTotalPrevistas(total);
    }
  };

  return (
    <Corpo>
      <table className="table mb-0">
        <thead className="tabela-frequencia-thead" key="thead">
          <tr>
            <th rowSpan="2" className="width-60 bc-w-i" scope="col"></th>
            <th rowSpan="2" className="text-center fundo-cinza">
              Previstas
            </th>
            <th
              colSpan={temProfessorCj ? 2 : 1}
              className="text-center fundo-cinza"
            >
              Criadas
            </th>
            <th rowSpan="2" className="text-center fundo-cinza">
              Dadas
            </th>
            <th rowSpan="2" className="text-center fundo-cinza">
              Repostas
            </th>
          </tr>
          {temProfessorCj ? (
            <tr>
              <th className="text-center fundo-cinza">Prof. Títular</th>
              <th className="text-center fundo-cinza">Prof. Substituto</th>
            </tr>
          ) : null}
        </thead>
        {bimestres
          ? bimestres.map((item, index) => {
            return (
              <tbody key={`lista-${item.bimestre}`}>
                <tr
                  className={
                    item.ehBimestreAtual ? 'bimestre-selecionado' : ''
                  }
                >
                  <td className="fundo-cinza-i">
                    <span className="negrito">{`${item.bimestre}º Bimestre`}</span>
                    <span>{` - ${formatarData(item.inicio)} à ${formatarData(
                      item.fim
                    )}`}</span>
                  </td>
                  <td>
                    {item.previstas.temDivergencia ? (
                      <CampoCentralizado className="p-l-16">
                        <CampoAlerta>
                          <CampoNumero
                            value={item.previstas.quantidade}
                            onChange={e => {
                              alterarValorPrevisto(index, e);
                            }}
                            onKeyDown={e => {
                              alterarValorPrevisto(index, e);
                            }}
                            step={1}
                            min={0}
                            max={999}
                            disabled={desabilitaCampos() || !item.podeEditar}
                          />
                          <div className="icone">
                            <Tooltip
                              title={item.previstas.mensagens[0]}
                              placement="bottom"
                              overlayStyle={{ fontSize: '12px' }}
                            >
                              <i className="fas fa-exclamation-triangle"></i>
                            </Tooltip>
                          </div>
                        </CampoAlerta>
                      </CampoCentralizado>
                    ) : (
                        <CampoEditavel>
                          <CampoNumero
                            value={item.previstas.quantidade}
                            onChange={e => {
                              alterarValorPrevisto(index, e);
                            }}
                            onKeyDown={e => {
                              alterarValorPrevisto(index, e);
                            }}
                            step={1}
                            min={0}
                            max={999}
                            disabled={desabilitaCampos() || !item.podeEditar}
                          />
                        </CampoEditavel>
                      )}
                  </td>
                  <td>
                    <CampoDesabilitado>
                      <span>{item.criadas.quantidadeTitular}</span>
                    </CampoDesabilitado>
                  </td>
                  {temProfessorCj ? (
                    <td>
                      <CampoDesabilitado>
                        <span>{item.criadas.quantidadeCJ}</span>
                      </CampoDesabilitado>
                    </td>
                  ) : null}
                  <td>
                    <CampoDesabilitado>
                      <span>{item.dadas}</span>
                    </CampoDesabilitado>
                  </td>
                  <td>
                    <CampoDesabilitado>
                      <span>{item.reposicoes}</span>
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
                <span>{totalCriadasTitular}</span>
              </CampoDesabilitado>
            </td>
            {temProfessorCj ? (
              <td>
                <CampoDesabilitado>
                  <span>{totalCriadasCj}</span>
                </CampoDesabilitado>
              </td>
            ) : null}
            <td>
              <CampoDesabilitado>
                <span>{totalDadas}</span>
              </CampoDesabilitado>
            </td>
            <td>
              <CampoDesabilitado>
                <span>{totalRepostas}</span>
              </CampoDesabilitado>
            </td>
          </tr>
        </tbody>
      </table>
    </Corpo>
  );
};

export default ListaAulasPorBimestre;
