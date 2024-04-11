import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/identity/services/auth.service';
import { TokenService } from 'src/app/identity/services/token.service';
import { CartService } from 'src/app/shopping-cart/cart.service';
import { MatDialog } from '@angular/material/dialog';
import { ShoppingCartModalComponent } from 'src/app/shop/shopping-cart-modal/shopping-cart-modal.component';


@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {
  isLoggedIn: boolean = false;
  firstName: string | null = null;
  userId: number | null = null;
  isModalOpen = false;
  cartItemCount = 0;

  @Output() openShoppingCartModal: EventEmitter<void> = new EventEmitter<void>();

  constructor(private dialog: MatDialog,private authService: AuthService, private tokenService: TokenService, private cartService: CartService) {

    }


  ngOnInit() {
    this.isLoggedIn = this.authService.isLoggedIn();
    if (this.isLoggedIn) {
      const fullName = this.tokenService.getFullName();
      this.firstName = fullName ? fullName.split(' ')[0] : null;
      this.userId = this.tokenService.getUserId();
    }
    if(this.isLoggedIn){
    this.cartService.getShoppingCart().subscribe(data => {
      this.cartItemCount = data.cartItems.length;
    });
  }
  }

  handleOpenShoppingCartModal(): void {
    if (!this.isModalOpen) {
      const dialogRef = this.dialog.open(ShoppingCartModalComponent, {
      });

      dialogRef.afterClosed().subscribe(() => {
        this.isModalOpen = false;
      });

      this.isModalOpen = true;
    }
  }


  logout() {
    this.authService.logout();
  }

}
