import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import AusenciasEstudante from './ausenciasEstudante';
import BtnExpandirAusenciaEstudante from './btnExpandirAusenciaEstudante';
import { TabelaColunasFixas } from './indicativosEstudante.css';
import ModalAnotacoesEncaminhamentoAEE from './modalAnotacoes';

const InformacoesEscolares = () => {
  const [dados, setDados] = useState([]);

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const obterInformacoesEscolaresDoAluno = useCallback(async () => {
    // TODO Loader e trocar mock!
    const resposta = await ServicoEncaminhamentoAEE.obterInformacoesEscolaresDoAluno(
      dadosCollapseLocalizarEstudante?.codigoAluno,
      dadosCollapseLocalizarEstudante?.codigoTurma
    ).catch(e => erros(e));

    if (resposta?.data) {
      setDados(resposta.data);
    } else {
      setDados([]);
    }
  }, [dadosCollapseLocalizarEstudante]);

  useEffect(() => {
    obterInformacoesEscolaresDoAluno();
  }, [obterInformacoesEscolaresDoAluno]);

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
                  <td className="col-valor-linha-um">
                    {dados.descricaoNecessidadeEspecial || '-'}
                  </td>
                  <td className="col-valor-linha-um">
                    {dados.descricaoRecurso || '-'}
                  </td>
                  <td className="col-valor-linha-um">
                    {dados.frequenciaGlobal}
                  </td>
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
                {dados?.frequenciaAlunoPorBimestres?.length ? (
                  dados?.frequenciaAlunoPorBimestres?.map((data, index) => {
                    return (
                      <>
                        <tr id={index}>
                          <td className="col-valor-linha-dois">
                            {data.bimestre}°
                          </td>
                          <td className="col-valor-linha-dois">
                            {data.quantidadeAusencias}
                          </td>
                          <td className="col-valor-linha-dois">
                            {data.quantidadeCompensacoes}
                          </td>
                          <td className="col-valor-linha-dois">
                            {data.frequencia}
                            <BtnExpandirAusenciaEstudante indexLinha={index} />
                          </td>
                        </tr>
                        <AusenciasEstudante indexLinha={index} dados={data} />
                      </>
                    );
                  })
                ) : (
                  <tr>
                    <td colSpan="4">Sem dados</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </TabelaColunasFixas>
    </>
  );
};

export default InformacoesEscolares;
