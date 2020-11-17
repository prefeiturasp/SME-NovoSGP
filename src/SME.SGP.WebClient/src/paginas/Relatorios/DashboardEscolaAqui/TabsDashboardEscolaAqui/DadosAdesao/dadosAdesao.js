import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { CoresGraficos, Graficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import {
  DataUltimaAtualizacao,
  LegendaGrafico,
  TituloGrafico,
  ContainerGraficoBarras,
} from '../../dashboardEscolaAqui.css';

const DadosAdesao = props => {
  const { codigoDre, codigoUe } = props;

  const [dadosGraficoAdesao, setDadosGraficoAdesao] = useState([]);
  const [chavesGrafico, setChavesGrafico] = useState([]);

  const [
    dadosGraficoTotalUsuariosSemAppInstalado,
    setDadosGraficoTotalUsuariosSemAppInstalado,
  ] = useState([]);

  const [
    dadosGraficoTotalUsuariosComCpfInvalidos,
    setDadosGraficoTotalUsuariosComCpfInvalidos,
  ] = useState([]);

  const [
    dadosGraficoTotalUsuariosPrimeiroAcessoIncompleto,
    setDadosGraficoTotalUsuariosPrimeiroAcessoIncompleto,
  ] = useState([]);

  const [
    dadosGraficoTotalUsuariosValidos,
    setDadosGraficoTotalUsuariosValidos,
  ] = useState([]);

  const [exibirLoader, setExibirLoader] = useState(false);
  const [dataUltimaAtualizacao, setDataUltimaAtualizacao] = useState();

  const obterDataUltimaAtualizacao = async () => {
    const retorno = await ServicoDashboardEscolaAqui.obterUltimaAtualizacaoPorProcesso(
      'ConsolidarAdesaoEOL'
    );
    if (retorno && retorno.data && retorno.data.dataUltimaAtualizacao) {
      setDataUltimaAtualizacao(
        moment(retorno.data.dataUltimaAtualizacao).format('DD/MM/YYYY HH:mm')
      );
    }
  };

  const obterPercentual = (valorAtual, valorTotal) => {
    const porcentagemAtual = (valorAtual / valorTotal) * 100;
    return `(${porcentagemAtual.toFixed(2)}%)`;
  };

  const formataMilhar = valor => {
    return valor.toLocaleString('pt-BR');
  };

  const obterValorTotal = dadosMapeados => {
    const getTotal = (total, item) => {
      return total + item.value;
    };
    return dadosMapeados.reduce(getTotal, 0);
  };

  const mapearDadosComPorcentagem = dadosMapeados => {
    if (dadosMapeados && dadosMapeados.length) {
      const valorTotal = obterValorTotal(dadosMapeados);

      dadosMapeados.forEach(item => {
        item.radialLabel = `${formataMilhar(item.value)} ${obterPercentual(
          item.value,
          valorTotal
        )}`;
      });
    }

    return dadosMapeados;
  };

  const mapearParaDtoGraficoPizza = dados => {
    const dadosMapeados = [];

    if (dados.totalUsuariosComCpfInvalidos) {
      const totalUsuariosComCpfInvalidos = {
        id: '1',
        label: 'Responsáveis sem CPF ou com CPF inválido no EOL',
        value: dados.totalUsuariosComCpfInvalidos || 0,
        color: '#F98F84',
      };
      dadosMapeados.push(totalUsuariosComCpfInvalidos);
    }

    if (dados.totalUsuariosSemAppInstalado) {
      const totalUsuariosSemAppInstalado = {
        id: '2',
        label: 'Usuários que não realizaram a instalação',
        value: dados.totalUsuariosSemAppInstalado || 0,
        color: '#57CDBC',
      };
      dadosMapeados.push(totalUsuariosSemAppInstalado);
    }

    if (dados.totalUsuariosPrimeiroAcessoIncompleto) {
      const totalUsuariosPrimeiroAcessoIncompleto = {
        id: '3',
        label: 'Usuários com primeiro acesso incompleto',
        value: dados.totalUsuariosPrimeiroAcessoIncompleto || 0,
        color: '#EFB971',
      };
      dadosMapeados.push(totalUsuariosPrimeiroAcessoIncompleto);
    }

    if (dados.totalUsuariosValidos) {
      const totalUsuariosValidos = {
        id: '4',
        label: 'Usuários válidos',
        value: dados.totalUsuariosValidos || 0,
        color: '#3982AC',
      };
      dadosMapeados.push(totalUsuariosValidos);
    }

    const dadosMapeadosComPorcentagem = mapearDadosComPorcentagem(
      dadosMapeados
    );
    setDadosGraficoAdesao(dadosMapeadosComPorcentagem);
  };

  const obterDadosGraficoAdesao = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoAdesao(
      codigoDre === '-99' ? '' : codigoDre,
      codigoUe === '-99' ? '' : codigoUe
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno && retorno.data && retorno.data.length) {
      mapearParaDtoGraficoPizza(retorno.data[0]);
    } else {
      setDadosGraficoAdesao([]);
    }
  }, [codigoDre, codigoUe]);

  const montarDadosGrafico = (item, nomeCampo, dados, index) => {
    if (item[nomeCampo]) {
      const totalDados = {
        nomeCompletoDre: item.nomeCompletoDre,
        color: CoresGraficos[index],
      };
      totalDados[nomeCampo] = item[nomeCampo];
      totalDados[item.nomeCompletoDre] = formataMilhar(item[nomeCampo]);
      dados.push(totalDados);
    }
  };

  const mapearDadosGraficos = useCallback(dados => {
    const chaves = [];
    const dadosTotalUsuariosComCpfInvalidos = [];
    const dadosTotalUsuariosSemAppInstalado = [];
    const dadosTotalUsuariosPrimeiroAcessoIncompleto = [];
    const dadosTotalUsuariosValidos = [];

    dados.forEach((item, index) => {
      chaves.push(item.nomeCompletoDre);

      montarDadosGrafico(
        item,
        'totalUsuariosComCpfInvalidos',
        dadosTotalUsuariosComCpfInvalidos,
        index
      );

      montarDadosGrafico(
        item,
        'totalUsuariosSemAppInstalado',
        dadosTotalUsuariosSemAppInstalado,
        index
      );

      montarDadosGrafico(
        item,
        'totalUsuariosPrimeiroAcessoIncompleto',
        dadosTotalUsuariosPrimeiroAcessoIncompleto,
        index
      );

      montarDadosGrafico(
        item,
        'totalUsuariosValidos',
        dadosTotalUsuariosValidos,
        index
      );
    });

    setChavesGrafico(chaves);
    setDadosGraficoTotalUsuariosComCpfInvalidos(
      dadosTotalUsuariosComCpfInvalidos
    );
    setDadosGraficoTotalUsuariosSemAppInstalado(
      dadosTotalUsuariosSemAppInstalado
    );
    setDadosGraficoTotalUsuariosPrimeiroAcessoIncompleto(
      dadosTotalUsuariosPrimeiroAcessoIncompleto
    );
    setDadosGraficoTotalUsuariosValidos(dadosTotalUsuariosValidos);
  }, []);

  const limparGraficosTotais = () => {
    setDadosGraficoTotalUsuariosComCpfInvalidos([]);
    setDadosGraficoTotalUsuariosSemAppInstalado([]);
    setDadosGraficoTotalUsuariosPrimeiroAcessoIncompleto([]);
    setDadosGraficoTotalUsuariosValidos([]);
  };

  const obterDadosGraficoAdesaoAgrupados = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoAdesaoAgrupados()
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno && retorno.data && retorno.data.length) {
      mapearDadosGraficos(retorno.data);
    } else {
      limparGraficosTotais();
    }
  }, [mapearDadosGraficos]);

  useEffect(() => {
    if (codigoDre && codigoUe) {
      if (codigoDre === '-99' && codigoUe === '-99') {
        obterDadosGraficoAdesaoAgrupados();
      } else {
        limparGraficosTotais();
      }
      obterDadosGraficoAdesao();
    } else {
      setDadosGraficoAdesao([]);
    }

    obterDataUltimaAtualizacao();
  }, [
    codigoDre,
    codigoUe,
    obterDadosGraficoAdesao,
    obterDadosGraficoAdesaoAgrupados,
  ]);

  const tooltipCustomizado = item => {
    return (
      <div style={{ whiteSpace: 'pre', display: 'flex', alignItems: 'center' }}>
        <span
          style={{
            display: 'block',
            width: '12px',
            height: '12px',
            background: item.color,
            marginRight: '7px',
          }}
        />
        {item.id} - <strong>{item.value}</strong>
      </div>
    );
  };

  const graficoBarras = (dados, titulo) => {
    return (
      <div className="scrolling-chart">
        <div className="col-md-12">
          <TituloGrafico>{titulo}</TituloGrafico>
          <ContainerGraficoBarras>
            <Graficos.Barras
              dados={dados}
              indice="nomeCompletoDre"
              chaves={chavesGrafico}
              groupMode="stacked"
              legendsTranslateX={105}
              showAxisBottom={false}
              customProps={{
                colors: item => item?.data?.color,
                tooltip: item => {
                  return tooltipCustomizado(item);
                },
                labelFormat: d => <tspan y={-7}>{d}</tspan>,
              }}
            />
          </ContainerGraficoBarras>
        </div>
      </div>
    );
  };

  return (
    <Loader loading={exibirLoader} className="text-center">
      {dataUltimaAtualizacao ? (
        <div className="col-md-12">
          <div className=" d-flex justify-content-end pb-4">
            <DataUltimaAtualizacao>
              Data da última atualização: {dataUltimaAtualizacao}
            </DataUltimaAtualizacao>
          </div>
        </div>
      ) : (
        ''
      )}

      {dadosGraficoAdesao && dadosGraficoAdesao.length ? (
        <>
          <TituloGrafico>Total de Usuários</TituloGrafico>
          <div className="row mb-5">
            <div className="col-md-6">
              <Graficos.Pie
                data={dadosGraficoAdesao}
                style={{ fontSize: '14px !important' }}
              />
            </div>
            <div className="col-md-6 d-flex align-items-center">
              <LegendaGrafico>
                <div className="legend-scale">
                  <ul className="legend-labels">
                    {dadosGraficoAdesao.map(item => {
                      return (
                        <li>
                          <span style={{ backgroundColor: item.color }} />
                          {item.label}
                        </li>
                      );
                    })}
                  </ul>
                </div>
              </LegendaGrafico>
            </div>
          </div>

          {dadosGraficoTotalUsuariosComCpfInvalidos?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosComCpfInvalidos,
                'Responsáveis sem CPF ou com CPF inválido no EOL'
              )
            : ''}

          {dadosGraficoTotalUsuariosSemAppInstalado?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosSemAppInstalado,
                'Responsáveis que não realizaram a instalação'
              )
            : ''}

          {dadosGraficoTotalUsuariosPrimeiroAcessoIncompleto?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosPrimeiroAcessoIncompleto,
                ' Responsáveis com primeiro acesso incompleto'
              )
            : ''}

          {dadosGraficoTotalUsuariosValidos?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosValidos,
                ' Responsáveis válidos'
              )
            : ''}
        </>
      ) : (
        'Sem dados'
      )}
    </Loader>
  );
};

DadosAdesao.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
};

DadosAdesao.defaultProps = {
  codigoDre: '',
  codigoUe: '',
};

export default DadosAdesao;
