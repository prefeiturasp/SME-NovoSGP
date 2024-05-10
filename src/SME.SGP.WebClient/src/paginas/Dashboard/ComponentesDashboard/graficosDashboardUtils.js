import React from 'react';
import { CoresGraficos } from '~/componentes';

const obterValorTotal = dadosMapeados => {
  const getTotal = (total, item) => {
    return total + item.value;
  };
  return dadosMapeados.reduce(getTotal, 0);
};

const formataMilhar = valor => {
  return valor.toLocaleString('pt-BR');
};

const obterPercentual = (valorAtual, valorTotal) => {
  const porcentagemAtual = (valorAtual / valorTotal) * 100;
  return `(${porcentagemAtual.toFixed(2)}%)`;
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

const mapearParaDtoGraficoPizzaComValorEPercentual = dados => {
  const dadosMapeados = dados.map((item, index) => {
    return {
      id: String(index + 1),
      label: item.label,
      value: item.value || 0,
      color: CoresGraficos[index],
    };
  });

  const dadosMapeadosComPorcentagem = mapearDadosComPorcentagem(dadosMapeados);
  return dadosMapeadosComPorcentagem;
};

const tooltipCustomizadoDashboard = item => {
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

const adicionarCoresNosGraficos = dados => {
  dados.forEach((item, index) => {
    item.color = CoresGraficos[index];
  });
  return dados;
};

const mapearParaDtoDadosComunicadosGraficoBarras = (
  dados,
  chavePrincipal,
  chavesGrafico
) => {
  const temDados = dados.filter(
    item =>
      item.naoReceberamComunicado ||
      item.receberamENaoVisualizaram ||
      item.visualizaramComunicado
  );
  if (temDados?.length) {
    // TODO Contador para turmas duplicadas, componente gráfico precisa de valores únicos!
    let contador = '';
    const dadosMapeados = dados.map(item => {
      contador += ' ';
      const novo = { ...item };
      if (
        item.naoReceberamComunicado ||
        item.receberamENaoVisualizaram ||
        item.visualizaramComunicado
      ) {
        novo[chavePrincipal] = item[chavePrincipal] + contador;
        if (item.naoReceberamComunicado) {
          novo.naoReceberamComunicado = item.naoReceberamComunicado;
          novo[chavesGrafico[0]] = item.naoReceberamComunicado;
        }
        if (item.receberamENaoVisualizaram) {
          novo.receberamENaoVisualizaram = item.receberamENaoVisualizaram;
          novo[chavesGrafico[1]] = item.receberamENaoVisualizaram;
        }
        if (item.visualizaramComunicado) {
          novo.visualizaramComunicado = item.visualizaramComunicado;
          novo[chavesGrafico[2]] = item.visualizaramComunicado;
        }
      }
      return novo;
    });

    const dadosMapeadosComCores = adicionarCoresNosGraficos(
      dadosMapeados.filter(item => item[chavePrincipal])
    );

    const dadosParaMontarLegenda = [];
    if (dadosMapeadosComCores.find(item => !!item.naoReceberamComunicado)) {
      dadosParaMontarLegenda.push({
        label: chavesGrafico[0],
        color: CoresGraficos[0],
      });
    }
    if (dadosMapeadosComCores.find(item => !!item.receberamENaoVisualizaram)) {
      dadosParaMontarLegenda.push({
        label: chavesGrafico[1],
        color: CoresGraficos[1],
      });
    }
    if (dadosMapeadosComCores.find(item => !!item.visualizaramComunicado)) {
      dadosParaMontarLegenda.push({
        label: chavesGrafico[2],
        color: CoresGraficos[2],
      });
    }

    return {
      dadosLegendaGrafico: dadosParaMontarLegenda,
      dadosComunicadosGraficoBarras: dadosMapeadosComCores,
    };
  }
  return null;
};

const obterDadosComunicadoSelecionado = (
  descricaoComunicado,
  listaComunicado
) => {
  let comunicado = null;
  if (descricaoComunicado) {
    const comunicadoAtual = listaComunicado.find(
      item => item.descricao === descricaoComunicado
    );
    if (comunicadoAtual?.id) {
      comunicado = comunicadoAtual;
    }
  }

  return comunicado;
};

const montarDadosGrafico = (
  item,
  nomeCampo,
  dadosMapeados,
  descricaoColuna
) => {
  if (item[nomeCampo]) {
    const novosDadosMap = {};
    novosDadosMap[descricaoColuna] = item[descricaoColuna];
    novosDadosMap[nomeCampo] = item[nomeCampo];
    novosDadosMap[item[descricaoColuna]] = formataMilhar(item[nomeCampo]);
    dadosMapeados.push(novosDadosMap);
  }
};

export {
  formataMilhar,
  adicionarCoresNosGraficos,
  obterDadosComunicadoSelecionado,
  tooltipCustomizadoDashboard,
  mapearParaDtoDadosComunicadosGraficoBarras,
  mapearParaDtoGraficoPizzaComValorEPercentual,
  montarDadosGrafico,
};
