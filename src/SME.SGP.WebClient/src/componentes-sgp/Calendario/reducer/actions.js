// Types
import Tipos from './types';

export const selecionarMes = payload => ({
  type: Tipos.selecionarMes,
  payload,
});

export const selecionarDia = payload => ({
  type: Tipos.selecionarDia,
  payload,
});

export const zeraCalendario = () => ({
  type: Tipos.zeraCalendario,
});
