import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Producto } from '../../modelos/producto.model';

@Injectable({ providedIn: 'root' })
export class ProductosService {
  private base = `${environment.productosApi}/api/productos`;

  constructor(private http: HttpClient) { }

  listar(nombre?: string, categoria?: string): Observable<Producto[]> {
    let params = new HttpParams();
    if (nombre) params = params.set('nombre', nombre);
    if (categoria) params = params.set('categoria', categoria);
    return this.http.get<Producto[]>(this.base, { params });
  }

  obtener(id: string): Observable<Producto> {
    return this.http.get<Producto>(`${this.base}/${id}`);
  }

  crear(p: Producto): Observable<Producto> {
    return this.http.post<Producto>(this.base, p);
  }

  actualizar(id: string, p: Producto): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}`, p);
  }

  eliminar(id: string): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }

  ajustarExistencias(id: string, ajuste: number, razon?: string): Observable<{ stock: number }> {
    return this.http.post<{ stock: number }>(`${this.base}/${id}/ajustar-existencias`, { ajuste, razon });
  }
}
