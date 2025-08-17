import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TransaccionesService } from '../../../core/servicios/transacciones.service';
import { ProductosService } from '../../../core/servicios/productos.service';
import { switchMap, tap, catchError, of } from 'rxjs';

@Component({
    standalone: true,
    selector: 'app-form-transaccion-observacion',
    imports: [CommonModule, ReactiveFormsModule, RouterLink],
    templateUrl: './form-transaccion-observacion.component.html',
    styleUrls: ['./form-transaccion-observacion.component.scss']
})
export class FormTransaccionObservacionComponent implements OnInit {
    private fb = inject(FormBuilder);
    private transApi = inject(TransaccionesService);
    private prodApi = inject(ProductosService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    id = signal<string | null>(null);
    transaccion: any = null;
    nombreProducto = '';

    form = this.fb.group({
        observacion: ['']
    });

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (!id) return;
        this.id.set(id);

        this.transApi.obtener(id).pipe(
            tap(t => {
                this.transaccion = t;
                this.form.patchValue({ observacion: t.observacion ?? '' });
            }),

            switchMap(t =>
                this.prodApi.obtener(t.productoId).pipe(
                    tap(p => this.nombreProducto = p.nombre),
                    catchError(() => {
                        this.nombreProducto = '';
                        return of(null);
                    })
                )
            )
        ).subscribe();
    }

    guardar() {
        const id = this.id();
        if (!id) return;

        const obs = this.form.value.observacion ?? null;
        this.transApi.actualizarObservacion(id, obs).subscribe(() => {
            this.router.navigate(['/transacciones', id, 'ver']);
        });
    }
}
