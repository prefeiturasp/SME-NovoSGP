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
      id: index + 1,
      label: item.label,
      value: item.value || 0,
      color: CoresGraficos[index],
    };
  });

  const dadosMapeadosComPorcentagem = mapearDadosComPorcentagem(dadosMapeados);
  return dadosMapeadosComPorcentagem;
};

const tooltipCustomizadoDashboardEscolaAqui = item => {
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

export {
  mapearParaDtoGraficoPizzaComValorEPercentual,
  formataMilhar,
  tooltipCustomizadoDashboardEscolaAqui,
  adicionarCoresNosGraficos,
};
