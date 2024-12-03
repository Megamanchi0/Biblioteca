import { Component } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { PerfilService } from '../../services/perfil.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-inicio-sesion',
  templateUrl: './inicio-sesion.component.html',
  styleUrl: './inicio-sesion.component.css'
})
export class InicioSesionComponent {

  form: FormGroup;
  error = false;
  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private router: Router, private login: PerfilService, private toastr: ToastrService) {
    this.form = fb.group({
      Correo: ["", [Validators.required, Validators.email]],
      Contrasena: ["", Validators.required]
    })
  }

  iniciarSesion(){
    this.isLoading=true;
    this.login.iniciarSesion(this.form.value).subscribe({
      next: (respuesta) => {
        if (respuesta==null) {
          this.error=true;
        }else{
          localStorage.setItem("token", respuesta.result);
          this.router.navigate(['registro-libros']);
        }
        this.isLoading=false;
        this.form.reset();
      },
      error: () => {
        this.toastr.error("Hubo un error al iniciar sesión", "Error");
        this.isLoading=false;
      }
    });
  }
}
