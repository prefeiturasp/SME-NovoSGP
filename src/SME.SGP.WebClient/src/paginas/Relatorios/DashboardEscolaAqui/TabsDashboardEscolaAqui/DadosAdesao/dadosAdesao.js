import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Graficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import {
  DataUltimaAtualizacao,
  LegendaGrafico,
} from '../../dashboardEscolaAqui.css';

const DadosAdesao = props => {
  const { codigoDre, codigoUe } = props;

  const [dadosGraficoAdesao, setDadosGraficoAdesao] = useState([]);
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

    if (dados.totalUsuariosPrimeiroAcesso) {
      const totalUsuariosPrimeiroAcesso = {
        id: '3',
        label: 'Usuários com primeiro acesso incompleto',
        value: dados.totalUsuariosPrimeiroAcesso || 0,
        color: '#EFB971',
      };
      dadosMapeados.push(totalUsuariosPrimeiroAcesso);
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

  useEffect(() => {
    if (codigoDre && codigoUe) {
      obterDadosGraficoAdesao();
    } else {
      setDadosGraficoAdesao([]);
    }

    obterDataUltimaAtualizacao();
  }, [codigoDre, codigoUe, obterDadosGraficoAdesao]);

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
          <div
            className="col-md-12"
            style={{
              fontSize: '24px',
              fontWeight: 700,
              textAlign: 'center',
              color: '#000000',
            }}
          >
            Total de Usuários
          </div>
          <div className="row">
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
