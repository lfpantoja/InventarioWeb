import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Transaccion, FiltroHistorial } from '../../modelos/transaccion.model';

@Injectable({ providedIn: 'root' })
export class TransaccionesService {
  private base = `${environment.transaccionesApi}/api/transacciones`;

  constructor(private http: HttpClient) { }

  registrar(t: Transaccion): Observable<Transaccion> {
    return this.http.post<Transaccion>(this.base, t);
  }

  listar(f: FiltroHistorial): Observable<Transaccion[]> {
    let params = new HttpParams();
    if (f.productoId) params = params.set('productoId', f.productoId);
    if (f.tipo) params = params.set('tipo', f.tipo);
    if (f.desde) params = params.set('desde', f.desde);
    if (f.hasta) params = params.set('hasta', f.hasta);
    return this.http.get<Transaccion[]>(this.base, { params });
  }

  historial(f: { page?: number; pageSize?: number; tipo?: 'compra' | 'venta'; productoId?: string; desde?: string; hasta?: string }) {
    let params = new HttpParams()
      .set('page', String(f.page ?? 1))
      .set('pageSize', String(f.pageSize ?? 20));
    if (f.tipo) params = params.set('tipo', f.tipo);
    if (f.productoId) params = params.set('productoId', f.productoId);
    if (f.desde) params = params.set('desde', f.desde);
    if (f.hasta) params = params.set('hasta', f.hasta);

    return this.http.get<{ total: number; page: number; pageSize: number; items: any[] }>(
      `${this.base}/historial`, { params }
    );
  }

  obtener(id: string): Observable<Transaccion> {
    return this.http.get<Transaccion>(`${this.base}/${id}`);
  }

  actualizarObservacion(id: string, observacion: string | null): Observable<void> {
    return this.http.patch<void>(`${this.base}/${id}/observacion`, { observacion });
  }
}
