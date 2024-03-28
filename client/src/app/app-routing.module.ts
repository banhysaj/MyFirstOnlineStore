import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { LoginComponent } from './identity/login/login.component';
import { SignupComponent } from './identity/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {path:'', component: HomeComponent, data:{breadcrumb: 'Home'}},
  {path:'shop', loadChildren: () => import('./shop/shop.module').then(m=>m.ShopModule)},
  {path:'login', component: LoginComponent},
  {path:'signup', component: SignupComponent},
  {path:'cart', component: ShoppingCartComponent, canActivate: [AuthGuard]},
  {path:'**', redirectTo: '', pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
