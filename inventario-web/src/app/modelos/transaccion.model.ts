export interface Transaccion {
  id?: string;
  fecha?: string;               // lo pone el servidor si no se envia
  tipo: 'compra' | 'venta';
  productoId: string;
  cantidad: number;
  precioUnitario: number;       // si tu backend lo calcula, puedes omitir en la UI
  precioTotal?: number;         // calculado en DB (solo lectura)
  observacion?: string | null;      // o 'observacion' si dejaste ese nombre
}

export interface FiltroHistorial {
  productoId?: string;
  tipo?: 'compra' | 'venta';
  desde?: string;   // ISO
  hasta?: string;   // ISO
  page?: number;
  pageSize?: number;
}
