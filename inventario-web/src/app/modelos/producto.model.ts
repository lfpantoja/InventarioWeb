export interface Producto {
  id?: string;
  nombre: string;
  descripcion?: string | null;
  categoria: string;
  urlImagen?: string | null;
  precio: number;
  existencias: number;
  fechaCreacion?: string;
  fechaActualizacion?: string | null;
}
