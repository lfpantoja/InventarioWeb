import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductosService } from '../../../core/servicios/productos.service';
import { Producto } from '../../../modelos/producto.model';

@Component({
  standalone: true,
  selector: 'app-producto-detalle',
  imports: [CommonModule, RouterLink],
  templateUrl: './producto-detalle.component.html',
  styleUrls: ['./producto-detalle.component.scss']
})
export class ProductoDetalleComponent implements OnInit {
  private api = inject(ProductosService);
  private route = inject(ActivatedRoute);

  cargando = signal(true);
  error = signal<string | null>(null);
  producto = signal<Producto | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.api.obtener(id).subscribe({
        next: (p) => {
          this.producto.set(p);
          this.cargando.set(false);
        },
        error: () => {
          this.error.set('No se pudo cargar el producto');
          this.cargando.set(false);
        }
      });
    }
  }
}
