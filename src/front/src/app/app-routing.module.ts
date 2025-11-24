import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AccueilComponent} from "./feature/accueil/accueil.component";
import {WeddingListComponent} from "./feature/wedding-list/wedding-list.component";
import {LoginComponent} from "./core/login/login.component";
import {UsersComponent} from "./feature/users/users.component";
import {ProfilComponent} from "./feature/profil/profil.component";
import {AuthGuardService} from "./shared/services/auth-guard.service";
import {GiftComponent} from "./feature/gift/gift.component";
import {MariageComponent} from "./feature/mariage/mariage.component";
import {CeremonieComponent} from "./feature/mariage/timeline/ceremonie/ceremonie.component";
import {VinHonneurComponent} from "./feature/mariage/timeline/vin-honneur/vin-honneur.component";
import {ReceptionComponent} from "./feature/mariage/timeline/reception/reception.component";
import {PhotosComponent} from "./feature/mariage/timeline/photos/photos.component";
import {MariesComponent} from "./feature/maries/maries.component";
import {ContactComponent} from "./feature/contact/contact.component";
import {PhotosMariageComponent} from "./feature/photos/photos.component";

const routes: Routes = [
  {
    path: 'accueil',
    component: AccueilComponent
  },
  {
    path: 'liste-de-mariage',
    component: WeddingListComponent
  },
  {
    path: 'liste-de-mariage/cadeau/:id',
    component: GiftComponent },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'utilisateurs',
    component: UsersComponent
  },
  {
    path: 'mariage',
    component: MariageComponent,
    children: [
      {
        path: '',
        redirectTo: 'ceremonie-religieuse',
        pathMatch: 'full'
      },
      {
        path: "ceremonie-religieuse",
        component: CeremonieComponent,
        pathMatch: 'full'
      },
      {
        path: "vin-honneur",
        component: VinHonneurComponent,
        pathMatch: 'full'
      },
      {
        path: "reception",
        component: ReceptionComponent,
        pathMatch: 'full'
      },
      {
        path: "photos",
        component: PhotosComponent,
        pathMatch: 'full'
      }
    ]
  },
  {
    path: 'maries',
    component: MariesComponent,
  },
  {
    path: 'contact',
    component: ContactComponent,
  },
  {
    path: 'photos',
    component: PhotosMariageComponent,
  },
  {
    path: 'profils',
    component: ProfilComponent,
    canActivate: [AuthGuardService]
  },
  { path: '**', redirectTo: '/accueil', pathMatch: 'full' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'enabled',
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
