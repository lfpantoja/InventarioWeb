import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';  
import { ProductosService } from '../../../core/servicios/productos.service';
import { TransaccionesService } from '../../../core/servicios/transacciones.service';
import { Producto } from '../../../modelos/producto.model';

@Component({
  standalone: true,
  selector: 'app-historial-transacciones',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './historial-transacciones.component.html',
  styleUrls: ['./historial-transacciones.component.scss']
})
export class HistorialTransaccionesComponent implements OnInit {
  private productosApi = inject(ProductosService);
  private transApi = inject(TransaccionesService);

  productos: Producto[] = [];
  items: any[] = [];

  // paginacion
  productoId = '';
  tipo = '';
  desde = '';
  hasta = '';
  page = 1;
  pageSize = 20;
  total = 0;

  ngOnInit() {
    this.productosApi.listar().subscribe(p => this.productos = p);
    this.buscar();
  }

  buscar(page: number = 1) {
    this.page = page;
    const f: any = { page: this.page, pageSize: this.pageSize };
    if (this.productoId) f.productoId = this.productoId;
    if (this.tipo) f.tipo = this.tipo as 'compra' | 'venta';
    if (this.desde) f.desde = this.desde;
    if (this.hasta) f.hasta = this.hasta;

    this.transApi.historial(f).subscribe(r => {
      this.total = r.total;
      this.items = r.items;
    });
  }

  totalPaginas(): number {
    return Math.max(1, Math.ceil(this.total / this.pageSize));
  }

  cambiarTamanio(size: number) {
    this.pageSize = size;
    this.buscar(1);
  }

  irA(p: number) {
    const max = this.totalPaginas();
    const destino = Math.min(Math.max(1, p), max);
    if (destino !== this.page) this.buscar(destino);
  }
}
