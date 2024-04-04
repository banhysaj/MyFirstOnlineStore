export interface Product {
  id: number;
  name: string;
  price: number;
  pictureUrl:string;
}

export interface CartItem {
  id: number;
  quantity: number;
  product: Product;
}

export interface ShoppingCart {
  id: number;
  userId: number;
  cartItems: CartItem[];
}