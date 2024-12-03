import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegistrarLibrosComponent } from './components/registrar-libros/registrar-libros.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { InicioSesionComponent } from './components/inicio-sesion/inicio-sesion.component';
import { PrestamoLibrosComponent } from './components/prestamo-libros/prestamo-libros.component';
import { MenuComponent } from './components/menu/menu.component';
import { RegistroPerfilComponent } from './components/registro-perfil/registro-perfil.component';
import { GestionPrestamoComponent } from './components/gestion-prestamo/gestion-prestamo.component';
import { HomeComponent } from './components/home/home.component';
import { ConsultarPerfilComponent } from './components/consultar-perfil/consultar-perfil.component';
import { provideHttpClient } from '@angular/common/http';
import { ConsultarPrestamosComponent } from './components/consultar-prestamos/consultar-prestamos.component';
import { GestionArchivosComponent } from './components/gestion-archivos/gestion-archivos.component';
import { HistorialArchivosComponent } from './components/historial-archivos/historial-archivos.component';

@NgModule({
  declarations: [
    AppComponent,
    RegistrarLibrosComponent,
    InicioSesionComponent,
    PrestamoLibrosComponent,
    MenuComponent,
    RegistroPerfilComponent,
    GestionPrestamoComponent,
    HomeComponent,
    ConsultarPerfilComponent,
    ConsultarPrestamosComponent,
    GestionArchivosComponent,
    HistorialArchivosComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    provideClientHydration(),
    provideHttpClient()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
