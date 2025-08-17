export interface Transaccion {
  id?: string;
  fecha?: string;
  tipo: 'compra' | 'venta';
  productoId: string;
  cantidad: number;
  precioUnitario: number;
  precioTotal?: number;
  observacion?: string | null;
}

export interface FiltroHistorial {
  productoId?: string;
  tipo?: 'compra' | 'venta';
  desde?: string;
  hasta?: string;
  page?: number;
  pageSize?: number;
}
