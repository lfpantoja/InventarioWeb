import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductosService } from '../../../core/servicios/productos.service';
import { Producto } from '../../../modelos/producto.model';

@Component({
  standalone: true,
  selector: 'app-lista-productos',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './lista-productos.component.html',
  styleUrls: ['./lista-productos.component.scss']
})
export class ListaProductosComponent implements OnInit {
  private api = inject(ProductosService);

  // filtros
  nombre = '';
  categoria = '';

  productos = signal<Producto[]>([]);
  cargando = signal(false);

  // paginación en cliente
  page = signal(1);
  pageSize = signal(10);
  total = computed(() => this.productosFiltrados().length);
  totalPaginas = computed(() => Math.max(1, Math.ceil(this.total() / this.pageSize())));

  productosFiltrados = computed(() => {
    const n = this.nombre.trim().toLowerCase();
    const c = this.categoria.trim().toLowerCase();
    return this.productos().filter(p =>
      (!n || p.nombre.toLowerCase().includes(n)) &&
      (!c || p.categoria.toLowerCase().includes(c))
    );
  });

  paginaActual = computed(() => {
    const start = (this.page() - 1) * this.pageSize();
    return this.productosFiltrados().slice(start, start + this.pageSize());
  });

  ngOnInit() { this.buscar(); }

  buscar() {
    this.cargando.set(true);
    this.api.listar(this.nombre, this.categoria).subscribe(res => {
      this.productos.set(res);
      this.page.set(1);
      this.cargando.set(false);
    });
  }

  eliminar(id?: string) {
    if (!id || !confirm('¿Eliminar producto?')) return;
    this.api.eliminar(id).subscribe(() => this.buscar());
  }

  irA(p: number) {
    const max = this.totalPaginas();
    this.page.set(Math.min(Math.max(1, p), max));
  }
}
