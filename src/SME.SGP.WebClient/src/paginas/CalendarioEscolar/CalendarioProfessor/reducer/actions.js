// Tipos
import Tipos from './types';

export const setarEventosMes = payload => ({
  type: Tipos.setarEventosMes,
  payload,
});

export const setarEventosDia = payload => ({
  type: Tipos.setarEventosDia,
  payload,
});

export const setarCarregandoCalendario = payload => ({
  type: Tipos.setarCarregandoCalendario,
  payload,
});

export const setarCarregandoMes = payload => ({
  type: Tipos.setarCarregandoMes,
  payload,
});

export const setarCarregandoDia = payload => ({
  type: Tipos.setarCarregandoDia,
  payload,
});
