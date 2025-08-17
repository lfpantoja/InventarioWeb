import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ProductosService } from '../../../core/servicios/productos.service';
import { TransaccionesService } from '../../../core/servicios/transacciones.service';
import { Producto } from '../../../modelos/producto.model';

@Component({
  standalone: true,
  selector: 'app-registrar-transaccion',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './registrar-transaccion.component.html',
  styleUrls: ['./registrar-transaccion.component.scss']
})
export class RegistrarTransaccionComponent implements OnInit {
  private fb = inject(FormBuilder);
  private productosApi = inject(ProductosService);
  private transApi = inject(TransaccionesService);

  productos: Producto[] = [];

  form = this.fb.group({
    productoId: ['', Validators.required],
    tipo: ['venta', Validators.required],  // 'compra' | 'venta'
    cantidad: [1, [Validators.required, Validators.min(1)]],
    precioUnitario: [0, [Validators.required, Validators.min(0)]],
    detalle: ['', Validators.required]
  });

  ngOnInit() {
    this.productosApi.listar().subscribe(p => this.productos = p);
  }

  registrar() {
    const v = this.form.value;
    this.transApi.registrar({
      productoId: v.productoId!,
      tipo: v.tipo as 'compra' | 'venta',
      cantidad: Number(v.cantidad),
      precioUnitario: Number(v.precioUnitario),
      observacion: v.detalle ?? undefined
    }).subscribe({
      next: () => {
        alert('Transaccion registrada');
        this.form.reset({ tipo: 'venta', cantidad: 1, precioUnitario: 0 });
      },
      error: (e) => alert(e?.error?.mensaje ?? 'Error al registrar')
    });
  }
}
