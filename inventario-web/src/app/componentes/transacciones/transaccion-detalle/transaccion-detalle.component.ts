// features/transacciones/transaccion-detalle/transaccion-detalle.component.ts
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TransaccionesService } from '../../../core/servicios/transacciones.service';
import { ProductosService } from '../../../core/servicios/productos.service';
import { Transaccion } from '../../../modelos/transaccion.model';

@Component({
  standalone: true,
  selector: 'app-transaccion-detalle',
  imports: [CommonModule, RouterLink ],
  templateUrl: './transaccion-detalle.component.html',
  styleUrls: ['./transaccion-detalle.component.scss']
})
export class TransaccionDetalleComponent implements OnInit {
  private transApi = inject(TransaccionesService);
  private prodApi = inject(ProductosService);
  private route = inject(ActivatedRoute);

  cargando = signal(true);
  error = signal<string | null>(null);
  transaccion = signal<Transaccion | null>(null);
  nombreProducto = signal<string>('');

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.transApi.obtener(id).subscribe({
      next: (t) => {
        this.transaccion.set(t);
        // cargar nombre del producto (opcional)
        this.prodApi.obtener(t.productoId).subscribe({
          next: (p) => this.nombreProducto.set(p.nombre),
          error: () => this.nombreProducto.set('—')
        });
        this.cargando.set(false);
      },
      error: () => {
        this.error.set('No se pudo cargar la transacción');
        this.cargando.set(false);
      }
    });
  }
}
