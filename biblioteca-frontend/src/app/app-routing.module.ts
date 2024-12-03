import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InicioSesionComponent } from "./components/inicio-sesion/inicio-sesion.component";
import { RegistrarLibrosComponent } from "./components/registrar-libros/registrar-libros.component";
import { adminGuard } from './guards/admin.guard';
import { PrestamoLibrosComponent } from './components/prestamo-libros/prestamo-libros.component';
import { loggedGuard } from './guards/logged.guard';
import { RegistroPerfilComponent } from './components/registro-perfil/registro-perfil.component';
import { GestionPrestamoComponent } from './components/gestion-prestamo/gestion-prestamo.component';
import { HomeComponent } from './components/home/home.component';
import { ConsultarPerfilComponent } from './components/consultar-perfil/consultar-perfil.component';
import { ConsultarPrestamosComponent } from './components/consultar-prestamos/consultar-prestamos.component';
import { GestionArchivosComponent } from './components/gestion-archivos/gestion-archivos.component';
import { HistorialArchivosComponent } from './components/historial-archivos/historial-archivos.component';

const routes: Routes = [
  {path: "", component: HomeComponent},
  {path: "login", component: InicioSesionComponent},
  {path: "registro-perfil", component: RegistroPerfilComponent},
  {path: "registro-libros", component: RegistrarLibrosComponent, canActivate: [adminGuard]},
  {path: "prestamo-libros", component: PrestamoLibrosComponent, canActivate: [loggedGuard]},
  {path: "gestion-prestamo", component: GestionPrestamoComponent, canActivate: [loggedGuard]},
  {path: "consultar-perfil", component: ConsultarPerfilComponent, canActivate: [loggedGuard]},
  {path: "consultar-prestamos", component: ConsultarPrestamosComponent, canActivate: [adminGuard]},
  {path: "gestion-archivos", component: GestionArchivosComponent, canActivate: [loggedGuard]},
  {path: "historial-archivos", component: HistorialArchivosComponent, canActivate: [adminGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
